using swap_service.Models;

namespace SwapAnalyzer.Models
{
    public class OrderInfo
    {
        internal bool isCompleted;

        public int OrdLevel { get; set; }
        public EnumSLTPStatus SLTPStatus { get; set; }
        public double originalFillPrice { get; set; }
        public DateTime statusUpdatedAt { get; set; }
        public string clOrderId { get; set; }
        public string symbol { get; set; }
        public double price { get; set; }
        public double filledUnits { get; set; }
        public string currency { get; set; }
        public double units { get; set; }
        public double filledPrice { get; set; }
        public string userId { get; set; }
        public string status { get; set; }
        public string timestamp { get; set; }
        public string market { get; set; }
        public double ltp { get; set; }
        public double slPerc { get; set; }
        public double multiplier { get; set; }
        public double tgPerc { get; set; }
        public string orderType { get; set; }
        public string productType { get; set; }
        public bool buy { get; set; }
        public bool deleted { get; set; }
        public string userIp { get; set; }
        public string updatedAt { get; set; }
        public double brokerageAmount { get; set; }
        public string brokerageType { get; set; }
        public double markup { get; set; }
        public string action { get; set; }
        public DateTime cancelledAt { get; set; }
        public DateTime modifiedAt { get; set; }
        public string cancelReason { get; set; }
        public string setOffOrderId { get; set; }
        public string modifiedOrderId { get; set; }
        public string adminId { get; set; }
        public bool createdByAdmin { get; set; }
        public bool isSquareOff { get; set; }
        public string appName { get; set; }

        public string modifiedReason { get; set; }

        public Dictionary<string, object> GetOrderMap()
        {

            return new Dictionary<string, object> {
                {"ClOrdID",clOrderId },
                {
                     "symbol", symbol

                },
                {   "price", price },
                            {
                "filledUnits", filledUnits},
       {
                "currency", currency},
      {
                "units", units},
       {
                "filledPrice", filledPrice},
     {
                "userId", userId},
            {
                "status", status},
       //{"timestamp", timestamp.ToString("yyyy-MM-ddTHH:mm:ss.ffffff")},

             {"timestamp", timestamp},
       {
                "market", market},
       {
                "ltp", ltp},
       {
                "multiplier", multiplier},
       {
                "slPerc", slPerc},
       {
                "tgPerc", tgPerc},
       {
                "orderType", orderType},
       {
                "productType", productType},
      {
                "buy", buy},
     {
                "deleted", false},
       {
                "userIp", userIp},
     {
                "updatedAt", timestamp},
       {
                "brokerageAmount", brokerageAmount},
                {"modifiedOrderId",modifiedOrderId },
                { "modifiedReason",modifiedReason},
       {
                "brokerageType", brokerageType},
      { "markup", markup},
                {"orderRecievedOnServerAt",Helpers.AppUtilities.GetCurrentUTCTime() },

                { "adminId",adminId},
                 { "createdByAdmin",adminId},
                  {"isSquareOff",isSquareOff },
                   {"appName",appName }

    };
       } 


   }
}
