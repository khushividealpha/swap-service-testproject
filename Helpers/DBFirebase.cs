using Newtonsoft.Json;
using SwapAnalyzer.WPFUtilities.Support;
using SwapWorkerService.Helpers;
using SwapWorkerService.Models;
using System.Text;



namespace SwapAnalyzer.Helpers
{
    public sealed class DBFireBase
    {

        public static string FormattedTime { get; set; }
        public static async void InitTime()
        {
            FormattedTime = await API.GetNTPTime 
                ();
        }
        public static async Task<HttpResponseMessage> GetSwapSettingSnapshot()
        {
            try
            {
                // Setup HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Base URL for the API
                    client.BaseAddress = new Uri("https://localhost:7175/");

                    // Sending a GET request to the API endpoint for instrument swaps
                    HttpResponseMessage response = await client.GetAsync("api/instrument-swap");

                    // Ensure the response indicates success
                    response.EnsureSuccessStatusCode();

                    // Return the response (can deserialize if necessary)
                    return response;
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Log the specific HttpRequest exception (API call related)
                AppLogWriter.WriteInLog(httpEx);
                throw new Exception("Failed to retrieve data from the InstrumentSwap API: " + httpEx.Message);
            }
            catch (Exception ex)
            {
                // Log any other general exceptions
                AppLogWriter.WriteInLog(ex);
                throw new Exception("An error occurred: " + ex.Message);
            }
        }


/*        internal static async Task UpdateUserDaySwapSum(string userId, List<UserSwap> lstUserSwap)
        {
            DocumentReference documentRefRoot = db.Collection("swapData").Document(userId);
            try
            {

                double userSum = Math.Round(lstUserSwap.Sum(x => AppUtilities.SafeMultiply(x.setting.SwapRate, x.OpenQty)), 2);
                string dateFormatted = FormattedTime;
                try
                {
                    await documentRefRoot.UpdateAsync(new Dictionary<string, object> {
                { dateFormatted,  userSum }
            });
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("NotFound"))
                    {
                        await documentRefRoot.SetAsync(new Dictionary<string, object> {
                { dateFormatted,  userSum }
            });
                    }
                }

                Console.WriteLine(AppUtilities.AppendTime("updateUserDaySwapSum finished successfully"));
                AppLogWriter.WriteInLog("updateUserDaySwapSum finished successfully");
            }
            catch (Exception ex)
            {

                AppLogWriter.WriteInLog(ex);
            }
        }*/
        internal static async Task UpdateUserDaySwapSum(UserSwap swapData, List<UserSwap> lstUserSwap)
        {
            try
            {
                // Calculate the user's daily swap sum
                Decimal userSum = Math.Round(lstUserSwap.Sum(x => AppUtilities.SafeMultiply(x.setting.SwapRate, x.OpenQty)), 2);

                // Prepare the data to send to the API
            /*    var swapData = new
                {
                    UserId = userId,
                    userSum = userSum
                };*/

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7175/");

                    // Serialize the data into JSON format
                    var jsonPayload = JsonConvert.SerializeObject(swapData);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Send a PUT request to update the user's daily swap sum
                    HttpResponseMessage response = await client.PutAsync("/api/user-swap", content);

                    // Ensure the request was successful
                    response.EnsureSuccessStatusCode();
                }

                // Log success
                Console.WriteLine(AppUtilities.AppendTime("UpdateUserDaySwapSum finished successfully"));
                AppLogWriter.WriteInLog("UpdateUserDaySwapSum finished successfully");
            }
            catch (Exception ex)
            {
                AppLogWriter.WriteInLog(ex);
            }
        }


        internal static async Task AddAccountsSwap(string userId, List<UserSwap> lstUserSwap, string adminId, string adminIp)
        {
            try
            {
                Decimal swapAmount = Math.Round(lstUserSwap.Sum(x => AppUtilities.SafeMultiply(x.setting.SwapRate, x.OpenQty)), 2);

                // Generate a unique transaction ID using the formatted time and userId
                string accountTransID = $"{FormattedTime}swap{userId}";

                // If swap amount is 0, exit the method as there's no swap to process
                if (swapAmount == 0)
                {
                    return;
                }

                // Prepare the data to be sent to the API
                var requestData = new
                {
                    UserId = userId,
                    SwapAmount = swapAmount,
                    AdminId = adminId,
                    AdminIp = adminIp,
                    AccountTransID = accountTransID,
                    UserSwaps = lstUserSwap
                };

                string jsonPayload = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Use HttpClient to send the data to the API
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7175/");

                    // Send a POST request to the API endpoint with the serialized content
                    HttpResponseMessage response = await client.PostAsync("api/AccountSwap", content);

                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API response: " + responseBody);
                }

                Console.WriteLine(AppUtilities.AppendTime("AddAccountsSwap finished successfully"));
                AppLogWriter.WriteInLog("AddAccountsSwap finished successfully");
            }
            catch (HttpRequestException ex)
            {
                AppLogWriter.WriteInLog($"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log general exceptions
                AppLogWriter.WriteInLog(ex);
            }
        }
        internal static async Task AddUserSwapData(UserSwap swapData)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7175/");

                var jsonPayload = JsonConvert.SerializeObject(swapData);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/UserSwapData", content);

                response.EnsureSuccessStatusCode();
            }
        }

       /* internal static async Task SaveUserSwap(Dictionary<string, List<UserSwap>> dctUserSwap)
        {
            FirebaseAuth firebaseAuth = FireSingle.Instance;
            var firestore = DBFireBase.Instance;

            List<Dictionary<string, object>> dctSwapMap;
            foreach (var userKey in dctUserSwap.Keys)
            {
                try
                {
                    await DBFireBase.AddAccountsSwap(userKey, dctUserSwap[userKey]);

                    await DBFireBase.UpdateUserDaySwapSum(userKey, dctUserSwap[userKey]);

                    dctSwapMap = new List<Dictionary<string, object>>();
                    foreach (var userSymbolSwap in dctUserSwap[userKey])
                    {
                        //if (userKey != "pi7x3YwbtOWAVHSvzkdaqx5ZaHj1")
                        //{
                        //    continue;

                        //}
                        dctSwapMap.Add(userSymbolSwap.UserSymbolSwapMap);
                    }

                    // User ID and Date (replace these with actual values)
                    string userId = userKey;
                    //  System.DateTime currentDate = await API.GetNTPTime();
                    string dateFormatted = FormattedTime;

                    // Sample data to save
                    // dctSwapmap
                    // long datetimeUnixTimestamp = ((DateTimeOffset)currentDate).ToUnixTimeSeconds();
                    //     long utcDateTimeUnixTimestamp = ((DateTimeOffset)utcDateTime).ToUnixTimeSeconds();
                    var swapData = new
                    {
                        Timestamp = FieldValue.ServerTimestamp,
                        Trigger = "SomeTrigger",
                        Swaps = dctSwapMap
                    };

                    // Reference to the document
                    DocumentReference documentRef = db.Collection("swapData")
                        .Document(userId)
                        .Collection("dateWiseSwapDetails")
                        .Document(dateFormatted);

                    ///vin




                    // Convert data to a Dictionary for Firestore
                    Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "timestamp", swapData.Timestamp },
            { "trigger", swapData.Trigger },
            { "swaps", swapData.Swaps }
        };

                    // Save the data
                    WriteResult writeResult = await documentRef.SetAsync(data);
                    //  AppLogWriter.WriteInLog(writeResult.ToString());
                    Console.WriteLine(AppUtilities.AppendTime("SaveUserSwap finished successfully"));
                    AppLogWriter.WriteInLog(AppUtilities.AppendTime("SaveUserSwap finished successfully"));
                }
                catch (Exception ex)
                {

                    AppLogWriter.WriteInLog(ex);
                }
            }

        }*/
    }
}
