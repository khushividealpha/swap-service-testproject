using swap_service.Models;

namespace SwapAnalyzer.Models
{

    internal interface ISwapUserSetting
    {
        public string UserID { get; set; }

        public string Symbol { get; set; }
        public string Currency { get; set; }

        //public double OpenQty { get; set; }
        public string MotherCurrency { get; set; }
        public bool IsMotherCurrency { get; set; }
        public Decimal ClosePrc { get; set; }
        public EnumSwapMethod Method { get; set; }

        public Decimal LongValue { get; set; }
        public Decimal ContractMultiplier { get; set; }
        
        public Decimal ShortValue { get; set; }

        public decimal TickValue { get; set; }

        public Decimal  SwapRate { get; set; }
        public Decimal OpenAverage { get; set; }

        public Decimal CalculateLongSwap(Decimal netQty);
        public Decimal CalculateShortSwap(Decimal netQty);
       // public  void GetSymbolOpenAverage();

        public Decimal selectedMultiplier { get; set; }
        public List<Decimal> dayMultiplier { get; set; }
        //string? UpdatedAt { get; set; }
        //string? CreatedAt { get; set; }
    }

}
