using Newtonsoft.Json;
using swap_service.Models;
using SwapAnalyzer.Models;
using SwapAnalyzer.WPFUtilities.Support;
using SwapWorkerService;
using SwapWorkerService.Helpers;
using SwapWorkerService.Models;
using System.Globalization;
using System.Net.Http.Json;
using System.Text;

namespace SwapAnalyzer.Helpers
{
    public class AppUtilities
    {



        public static string AppendTime(string msg)
        {
            string time = System.DateTime.Now.ToString("ddMMMyyyy HH:mm:ss");
            return $"{msg} {time}";
        }

        public static decimal SafeDifference(dynamic num1, dynamic num2)
        {

            return (num1 - num2);
        }
        public static double SafeSum(dynamic num1, dynamic num2)
        {
            decimal decimalNum1 = Convert.ToDecimal(num1);
            decimal decimalNum2 = Convert.ToDecimal(num2);
            return (double)(decimalNum1 + decimalNum2);
        }

        public static decimal SafeMultiply(dynamic num1, dynamic num2)
        {

            return (num1 * num2);
        }

        public static decimal SafeDivide(dynamic num1, dynamic num2)
        {
 

            if (num1 == num2)
            {
                // Handle division by zero gracefully, for example, return NaN.
                return 0;
            }
            else
            {
                return (num1 / num2);
            }
        }
        public static System.DateTime ConvertToDateTime(string dateString, string Format)
        {
            if (System.DateTime.TryParseExact(dateString, Format, CultureInfo.InvariantCulture,
                                DateTimeStyles.RoundtripKind, out System.DateTime dateValue))
            {

            }
            return dateValue;

        }


        public static async Task CalculateSwap()
        {
            try
            {
                using (HttpClient client = new HttpClient())

                {   // Fetch swap settings from your API instead of Firebase
                    var response = await client.GetAsync("https://localhost:7175/api/instrument-swap");
                    if (!response.IsSuccessStatusCode)
                    {
                        // Handle error (e.g., log it)
                        AppLogWriter.WriteInLog("Failed to fetch swap settings from the API.");
                        return;
                    }

                    var snapshot = await response.Content.ReadFromJsonAsync<List<InstrumentSwapModel>>();
/*                    Dictionary<string, ISwapUserSetting> dicsnapshot = snapshot.ToDictionary(item => item.Symbol);
*/                    Dictionary<string, ISwapUserSetting> lstSettings = new Dictionary<string, ISwapUserSetting>();

                    foreach (var documentData in snapshot)
                    {
                        // Console.WriteLine(documentData);
                       // Dictionary<string, object> Data = snapshot

                        ISwapUserSetting swapUserSetting = SwapFactory.GetSwapObject(documentData.CalculationMethod ?? "ByPoint");


                        // Now you can work with the data in documentData dictionary
                      //  string method = documentData.GetFixValue<string>("calculationMethod");

                        // Set the dayMultiplier, defaulting to [1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0] if not provided
                        swapUserSetting.dayMultiplier = documentData.DayMultiplier;
                        swapUserSetting.Symbol = documentData.Symbol;
                        swapUserSetting.ShortValue = documentData.ShortSwap != default ? documentData.ShortSwap : 5;
                        swapUserSetting.TickValue = documentData.TickSize != default ? documentData.TickSize : .1M;
                        swapUserSetting.LongValue = documentData.LongSwap != default ? documentData.LongSwap : -5;
                        swapUserSetting.MotherCurrency = documentData.MotherCurrency ?? "USD";

                        // Get current date and calculate the selected multiplier
                        System.DateTime currentDate = AppUtilities.ConvertToDateTime(DBFireBase.FormattedTime, "ddMMyyyy");
                        int dayIndex = (int)currentDate.DayOfWeek;
                        swapUserSetting.selectedMultiplier = swapUserSetting.dayMultiplier[dayIndex];
                     //   swapUserSetting.ClosePrc = SQLDatabase.GetSymbolClose(swapUserSetting.Symbol);
                        if (swapUserSetting.ClosePrc <= 0)
                        {
                            continue;
                        }

                        lstSettings.Add(swapUserSetting.Symbol, swapUserSetting);
                    }



                    // Fetch net positions from your MySQL database
                    List<NetPositions> lstNetPosition = await MySqlDB.GetAllParam<NetPositions>("CALL GetNetQueryData()");
                    UserSwap userSwap = null;
                    Dictionary<string, List<UserSwap>> dctUserSwap = null;

                    if (lstNetPosition != null && lstNetPosition.Count > 0)
                    {
                        dctUserSwap = new Dictionary<string, List<UserSwap>>();
                        foreach (var pos in lstNetPosition) 

                        {
                            if (lstSettings.ContainsKey(pos.Symbol))
                            {
                                userSwap = new UserSwap
                                {
                                    UserID = pos.UserId,
                                    setting = lstSettings[pos.Symbol],
                                    //setting.ContractMultiplier = pos.Multiplier,
                                    OpenQty = pos.NetQty
                                };

                                if (!dctUserSwap.ContainsKey(pos.UserId))
                                {
                                    dctUserSwap.Add(pos.UserId, new List<UserSwap>());
                                }

                                dctUserSwap[pos.UserId].Add(userSwap);

                                // Calculate the SwapRate based on the NetQty
                                if (pos.NetQty > 0)
                                {
                                    userSwap.setting.SwapRate = SafeDivide(userSwap.setting.CalculateLongSwap(pos.NetQty), pos.NetQty);
                                }
                                else if (pos.NetQty < 0)
                                {
                                    userSwap.setting.SwapRate = SafeDivide(userSwap.setting.CalculateShortSwap(pos.NetQty), pos.NetQty);
                                }
                            }
                        }

                        // Save the user swaps using your API or database method
                        await SaveUserSwapsToAPI(dctUserSwap);  // Change this to your API call if necessary
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogWriter.WriteInLog(ex);
            }
        }
        private static async Task SaveUserSwapsToAPI(Dictionary<string, List<UserSwap>> dctUserSwap)
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(dctUserSwap), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:7175/api/save-user-swap", content);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle error in saving the data
                    AppLogWriter.WriteInLog("Failed to save user swaps to the API.");
                }
                else
                {
                    Console.WriteLine("User swaps saved successfully.");
                }
            }
        }
        public async Task<List<NetPositions>> FetchNetPositionsFromAPI()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync("https://exchdata.videalpha.com/pos");
            var positions = JsonConvert.DeserializeObject<List<NetPositions>>(response);

            return positions; 
        }
        public static string GetCurrentUTCTime()
        {
            return System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffffZ");
        }

        /*        public static T ConvertNumeric<T>(object val)
                {

                    T outVal = (T)Convert.ChangeType(0, typeof(T));
                    //if (val == null)
                    //    return outVal;
                    try
                    {
                        if (!string.IsNullOrEmpty(val.ToString()))
                        {
                            return (T)Convert.ChangeType(val, typeof(T));
                        }
                        else
                        {
                            return outVal;
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }*/

        /*        public static string EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
                {
                    byte[] encrypted;

                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.Key = key;
                        aesAlg.IV = iv;

                        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                        using (MemoryStream msEncrypt = new MemoryStream())
                        {
                            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                                {
                                    swEncrypt.Write(plainText);
                                }
                                encrypted = msEncrypt.ToArray();
                            }
                        }
                    }
                    return Convert.ToBase64String(encrypted);
                }*/
        /*        public static string DecryptString(string key, string cipherText)
                {
                    byte[] iv = new byte[16];
                    byte[] buffer = Convert.FromBase64String(cipherText);

                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = Encoding.UTF8.GetBytes(key);
                        aes.IV = iv;
                        aes.Mode = CipherMode.CBC;
                        aes.Padding = PaddingMode.PKCS7;
                        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                        using (MemoryStream memoryStream = new MemoryStream(buffer))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                                {
                                    return streamReader.ReadToEnd();
                                }
                            }
                        }
                    }
                }*/

        /*     public static async Task<string> GetUserName(string userID)
             {
                 try
                 {
                     var firestore = DBFireBase.Instance;

                     FirebaseAuth firebaseAuth = FireSingle.Instance;

                     Query appRegQuery = firestore.Collection("appRegistrations");
                     QuerySnapshot allCitiesQuerySnapshot = await appRegQuery.GetSnapshotAsync();


                     UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(userID);

                     return userRecord.DisplayName;
                 }
                 catch (Exception ex)
                 {
                     WPFUtilities.Support.AppLogWriter.WriteInLog(ex, MethodBase.GetCurrentMethod());
                 }
                 return string.Empty;

             }*/

        /*        public static string GetConString()
                {
                    try
                    {
                        return $"Data Source={Path.Combine(AppLogWriter.GetAppDataPath(), "PrimeXMDB.db")}";
                    }
                    catch (Exception ex)
                    {
                        WPFUtilities.Support.AppLogWriter.WriteInLog(ex, MethodBase.GetCurrentMethod());
                    }
                    return null;
                }

               
        */
        /*        public static void SetEnvironmentVar()
                {

                    string envPath = Path.Combine(Config.BaseFilePath, "firebaseadminsdk.json");
                    AppLogWriter.WriteInLog($"ENV PATH {envPath}");
                    if (System.Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS") != null && string.Compare(System.Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS"), envPath) != 0)
                    {
                        System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", envPath);
                    }
                    if (System.Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS") == null)
                    {
                        System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", envPath);
                    }

                }*/
        /*
                public static string GetCurrentDirectory(string fileName)
                {
                    // Get the path of the executable file
                    string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

                    // Resolve the directory
                    string directory = Path.GetDirectoryName(exePath);

                    // Construct the full file path relative to the directory
                    string relativeFilePath = Path.Combine(directory, fileName);
                    AppLogWriter.WriteInLog($"Path {relativeFilePath}");
                    return relativeFilePath;
                }*/
       


    }

}
