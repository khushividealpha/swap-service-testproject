using Azure;
using Newtonsoft.Json;
using swap_service.Models;
using SwapAnalyzer.Models;
using SwapWorkerService;
using SwapWorkerService.Models;
using System.Globalization;

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


        public static async Task CalculateSwap(string token)
        {
            var lstSettings = new Dictionary<string, ISwapUserSetting>();
            try
            {
                using (HttpClient client = new HttpClient())

                {
                    try
                    {
                        var response = await client.GetStringAsync("http://13.232.66.46/api/instrument-swap");

                        Console.WriteLine(response);

                        var acctualresponse = JsonConvert.DeserializeObject<ResponseModel>(response);

                        var datacontent = acctualresponse.data;




                        var snapshot = JsonConvert.DeserializeObject<List<InstrumentSwapModel>>(datacontent.ToString());

                        foreach (var documentData in snapshot)
                        {


                            ISwapUserSetting swapUserSetting = null;
                            //  Dictionary<string, object> settings = snapshot.ToDictionary();
                            string method = documentData.CalculationMethod;
                            if (method == default(string))
                            {
                                method = "ByPoint";
                            }
                            swapUserSetting = SwapFactory.GetSwapObject(method);
                            swapUserSetting.dayMultiplier = documentData.DayMultiplier;
                            swapUserSetting.Symbol = documentData.InstrumentName;
                            swapUserSetting.MotherCurrency = documentData.MotherCurrency;
                            swapUserSetting.TickValue = documentData.TickSize;
                            swapUserSetting.ShortValue = documentData.ShortSwap;
                            swapUserSetting.LongValue = documentData.LongSwap;
                            swapUserSetting.ClosePrc = await GetClosePriceAsync(swapUserSetting.Symbol);
                            lstSettings.Add(swapUserSetting.Symbol, swapUserSetting);
                        }
                    }
                    catch
                    {

                    }
                }
                /////--------------------------------------------------------unauthorize aa rha ky kre----------------------------------------------------
                /*  using (HttpClient client = new HttpClient())
                  {
                      client.BaseAddress = new Uri("http://13.232.66.46/");
                      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                      HttpResponseMessage responsePosition = await client.GetAsync("api/net-position");
                      var positionContent = await responsePosition.Content.ReadAsStringAsync();
                      var netPositionResponse = JsonConvert.DeserializeObject<ResponseModel>(positionContent);
                      var lstNetPosition = JsonConvert.DeserializeObject<List<NetPosition>>(netPositionResponse.data.ToString());
                      UserSwap userSwap = null;
                      Dictionary<string, List<UserSwap>> dctUserSwap = null;
                      if (lstNetPosition == null || lstNetPosition.Count == 0)
                      {
                          Console.WriteLine("No net positions found.");
                          return;
                      }
                      if (lstNetPosition != null && lstNetPosition.Count > 0)
                      {
                          foreach (var pos in lstNetPosition)
                          {
                              if (lstSettings.ContainsKey(pos.symbol))
                              {
                                  ISwapUserSetting setting = lstSettings[pos.symbol];
                                  userSwap = new UserSwap();
                                  userSwap.UserID = pos.userId;
                                  if (!dctUserSwap.ContainsKey(pos.userId))
                                  {
                                      dctUserSwap.Add(pos.userId, new List<UserSwap>());
                                  }

                                  dctUserSwap[pos.userId].Add(userSwap);

                                  userSwap.setting = lstSettings[pos.symbol];
                                  userSwap.setting.ContractMultiplier = pos.Multiplier;
                                  userSwap.OpenQty = pos.NetQty;

                                  Console.WriteLine($"Processing swap for symbol: {pos.symbol}");
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
                          //integrate post api of user swap
                          // Serialize dctUserSwap to JSON and send as a POST request
                          var jsonContent = JsonConvert.SerializeObject(dctUserSwap);
                          var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                          using (HttpClient clientPost = new HttpClient())
                          {
                              clientPost.BaseAddress = new Uri("http://13.232.66.46/");

                              HttpResponseMessage postResponse = await clientPost.PostAsync("api/user-swap", content);

                              if (postResponse.IsSuccessStatusCode)
                              {
                                  Console.WriteLine("User swaps saved successfully.");
                              }
                              else
                              {
                                  Console.WriteLine($"Failed to save user swaps. Status: {postResponse.StatusCode}");
                              }
                          }
                      }
                  }

  */
              // Fetch data directly from the positions table
        List<NetPosition> lstNetPosition = await MySqlDB.GetPositionsAsync();
                UserSwap userSwap = null;
                Dictionary<string, List<UserSwap>> dctUserSwap = null;
                if (lstNetPosition != null && lstNetPosition.Count > 0)
                {
                    dctUserSwap = new Dictionary<string, List<UserSwap>>();
                    foreach (var pos in lstNetPosition)
                    {
                        if (lstSettings.ContainsKey(pos.symbol))
                        {
                            userSwap = new UserSwap();
                            userSwap.UserID = pos.userId;
                            if (!dctUserSwap.ContainsKey(pos.userId))
                            {
                                dctUserSwap.Add(pos.userId, new List<UserSwap>());
                            }

                            dctUserSwap[pos.userId].Add(userSwap);

                            userSwap.setting = lstSettings[pos.symbol];
                            userSwap.setting.ContractMultiplier = pos.Multiplier;
                            userSwap.OpenQty = pos.NetQty;

                            // Get the day of the week as an index (0 for Sunday, 1 for Monday, etc.)

                            // if open qty is in buy i.e buytotal > selltotal , use long swap value for calculations
                            //else if total sell qty > total buy qty, use short swap value for calculaion

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

                    //integrate post api of user swap
                    // Serialize dctUserSwap to JSON and send as a POST request
                    try
                    {
                        var responseModel = new ResponseModel
                        {
                            data = dctUserSwap.Values,  // Set data to dctUserSwap values
                            status = 200,  // Assuming success status; adjust as needed
                            message = "User swap data submission"  // Custom message
                        };
                        var jsonContent = JsonConvert.SerializeObject(responseModel);
                        Console.WriteLine(jsonContent);
                        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                        string url = "http://13.232.66.46/api/user-swap";
                        using (HttpClient clientPost = new HttpClient())
                        {
                           // clientPost.BaseAddress = new Uri("http://13.232.66.46/");

                            HttpResponseMessage postResponse = await clientPost.PostAsync(url, content);
                            postResponse.EnsureSuccessStatusCode();
                            if (postResponse.IsSuccessStatusCode)
                            { 

                                Console.WriteLine("User swaps saved successfully.");
                            }
                            else
                            {
                                Console.WriteLine($"Failed to save user swaps. Status: {postResponse.StatusCode}");
                            }
                        }
                    }
                    catch
                    {

                    }
                    
                }
            }
            catch
            {

            }
        }
        public static async Task<decimal> GetClosePriceAsync(string? symbol)
        {
            using (var httpClient = new HttpClient())
            {
                // Set up the URL for the POST request
                string url = "https://historicaldata.videalpha.com/SymbolClose";

                // Create the content for the POST request
                var requestData = new { Symbol = symbol };
                var jsonContent = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(requestData),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                try
                {
                    // Send the POST request
                    HttpResponseMessage response = await httpClient.PostAsync(url, jsonContent);
                    response.EnsureSuccessStatusCode();

                    // Assuming the response content is a decimal value for the close price
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Convert the response to a decimal
                    if (decimal.TryParse(responseContent, out decimal closePrice))
                    {
                        return closePrice;
                    }
                    else
                    {
                        throw new Exception("Failed to parse the close price from API response.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching close price: {ex.Message}");
                    throw;
                }
            }
        }
    }

}            

