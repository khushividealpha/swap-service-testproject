using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace SwapWorkerService.Helpers
{
    public class TimeZoneInfo
    {
        public string? Abbreviation { get; set; }
        public string? ClientIp { get; set; }
        public string? Datetime { get; set; } // Use string for datetime
        public int DayOfWeek { get; set; }
        public int DayOfYear { get; set; }
        public bool Dst { get; set; }
        public object DstFrom { get; set; }
        public int DstOffset { get; set; }
        public object DstUntil { get; set; }
        public int RawOffset { get; set; }
        public string? Timezone { get; set; }
        public long Unixtime { get; set; }
        public string? utc_datetime { get; set; } // Use string for utc_datetime
        public string? UtcOffset { get; set; }
        public int WeekNumber { get; set; }
    }

    internal class API
    {
        public static async Task<string> GetNTPTime()
        {
            System.DateTime dtTime = System.DateTime.Now;
             string formattedDate = dtTime.AddDays(-1).ToString("ddMMyyyy");
           // string formattedDate = dtTime.ToString("ddMMyyyy");
            TimeZoneInfo timeZoneInfo = null;
            try
            {
                using (HttpClientHandler httpClientHandler = new HttpClientHandler())
                {
                    // Bypass SSL certificate validation (use this only for debugging or if you trust the source)
                    httpClientHandler.ServerCertificateCustomValidationCallback = (HttpRequestMessage message, X509Certificate2 cert, X509Chain chain, SslPolicyErrors errors) => true;

                    using (HttpClient httpsClient = new HttpClient(httpClientHandler))
                    {
                        HttpResponseMessage response = await httpsClient.GetAsync("https://worldtimeapi.org/api/timezone/utc");

                        if (response.IsSuccessStatusCode)
                        {
                            if (response.Content.Headers.ContentLength > 0)
                            {
                                string content = await response.Content.ReadAsStringAsync();
                                timeZoneInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<TimeZoneInfo>(content);
                                if (timeZoneInfo.Datetime != string.Empty)
                                {
                                    dtTime = System.DateTime.Parse(timeZoneInfo.Datetime);
                                    formattedDate = dtTime.ToString("ddMMyyyy");
                                }
                                Console.WriteLine(content);
                            }
                            else
                            {
                                Console.WriteLine("Response content is empty.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"HTTP request failed with status code {response.StatusCode}");
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request exception: {ex.Message}");
            }
   
            return formattedDate;
        }
    }
}
