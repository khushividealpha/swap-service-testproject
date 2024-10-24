using swap_service.Models;
using SwapAnalyzer.Helpers;


namespace SwapAnalyzer.Models
{
    internal class ByPoint : ISwapUserSetting
    {
        public string UserID { get; set; }

        public string Currency { get; set; }

        public bool IsMotherCurrency { get; set; }

        public EnumSwapMethod Method { get; set; }

        public Decimal LongValue { get; set; }

        public Decimal ShortValue { get; set; }

        public Decimal TickValue { get; set; }

        public List<int> lstSwapDay { get; set; }

        public Decimal selectedMultiplier { get; set; }

        public Decimal Price { get; set; }

        public Decimal ClosePrc { get; set; }

        public Decimal Quantity { get; set; }

        public Decimal OpenAverage { get; set; }

        public string MotherCurrency { get; set; }

        public List<Decimal> dayMultiplier { get; set; }

        public string UpdatedAt { get; set; }

        public string CreatedAt { get; set; }

        public Decimal SwapRate { get; set; }

        public string Symbol { get; set; }

        public Decimal ContractMultiplier { get; set; }

        public Decimal CalculateLongSwap(Decimal netQty)
        {
            return AppUtilities.SafeMultiply(this.GetPricePoint(Math.Abs(netQty)), this.LongValue);
        }

        private Decimal GetPricePoint(Decimal netQty)
        {
            return !(this.MotherCurrency == "USD") ? AppUtilities.SafeMultiply(AppUtilities.SafeMultiply(AppUtilities.SafeMultiply(netQty, this.selectedMultiplier), (Decimal)this.TickValue), this.ContractMultiplier) / this.ClosePrc : AppUtilities.SafeMultiply(AppUtilities.SafeMultiply(AppUtilities.SafeMultiply(netQty, this.selectedMultiplier), (Decimal)this.TickValue), this.ContractMultiplier);
        }

        public Decimal CalculateShortSwap(Decimal netQty)
        {
            return AppUtilities.SafeMultiply(this.GetPricePoint(netQty), this.ShortValue);
        }
    }
}
