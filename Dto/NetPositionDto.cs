using System.ComponentModel.DataAnnotations;

namespace swap_service.Dto
{
    public class NetPositionDto
    {
        //public NewMargin newMargin { get; set; }
        //public ConcurrentBag<string> lstPending = new ConcurrentBag<string>();
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? clientId { get; set; }
        public string? UserName { get; set; }
        public string? Symbol { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal OpenMTM { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalSellAmount { get; set; }
        public decimal TotalSellUnits { get; set; }
        public decimal TotalBuyUnits { get; set; }
        public decimal TotalBuyAmount { get; set; }
        public decimal SellAvg { get; set; }
        public decimal BuyAvg { get; set; }
        public decimal Multiplier { get; set; }
        public decimal BookedPNL { get; set; }
        public decimal Equity { get; set; }
        public decimal OpenAvg { get; set; }
        public decimal leverage { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal TotalBuyUsedMargin { get; set; }
        public decimal TotalSellUsedMargin { get; set; }
        public decimal OpenCommission { get; set; }
        public decimal NetQty { get; set; }
        public decimal UsedMargin { get; set; }
        public decimal NewMargin { get; set; }
        public decimal OpenUnits { get; set; }
        public decimal MarginLevel { get; set; }
        public decimal MarginAvailable { get; set; }
        public decimal TotalMargin { get; set; }
        public decimal BuyPrice { get; internal set; }
        public decimal SellPrice { get; internal set; }
        //public decimal BuyQty { get; internal set; }
        //public decimal SellQty { get; internal set; }
        public string? OrderSide { get; internal set; }
        public string? Currency { get; set; }
        public decimal BrokageAmount { get; internal set; }
        public decimal OpenValue { get; internal set; }
        public decimal NonUSDMargin { get; internal set; }
    }


}



