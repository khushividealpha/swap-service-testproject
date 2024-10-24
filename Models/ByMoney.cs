// Decompiled with JetBrains decompiler
// Type: SwapAnalyzer.Models.ByMoney
// Assembly: SwapWorkerService, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0CC2037A-ACD2-4EB5-98F3-D7E3310C5F01
// Assembly location: D:\Gulshan\Programs\VidePrograms\ServicesLive\SwapService\publish\SwapWorkerService.dll

using swap_service.Models;
using SwapAnalyzer.Helpers;


namespace SwapAnalyzer.Models
{
    internal class ByMoney : ISwapUserSetting
    {
        public string UserID { get; set; }

        public string Currency { get; set; }

        public bool IsMotherCurrency { get; set; }

        public EnumSwapMethod Method { get; set; }

        public Decimal LongValue { get; set; }

        public Decimal ContractMultiplier { get; set; }

        public Decimal OpenQty { get; set; }

        public Decimal ShortValue { get; set; }

        public Decimal TickValue { get; set; }

        public List<int> lstSwapDay { get; set; }

        public Decimal ClosePrc { get; set; }

        public Decimal OpenAverage { get; set; }

        public string MotherCurrency { get; set; }

        public List<Decimal> dayMultiplier { get; set; }

        public string UpdatedAt { get; set; }

        public string CreatedAt { get; set; }

        public Decimal selectedMultiplier { get; set; }

        public Decimal SwapRate { get; set; }

        public string Symbol { get; set; }

        public Decimal CalculateLongSwap(Decimal netQty)
        {
            return AppUtilities.SafeMultiply(AppUtilities.SafeMultiply(this.OpenQty, this.LongValue), this.selectedMultiplier);
        }

        public Decimal CalculateShortSwap(Decimal netQty)
        {
            return AppUtilities.SafeMultiply(AppUtilities.SafeMultiply(this.OpenQty, this.ShortValue), this.selectedMultiplier);
        }

  /*      public async void GetSymbolOpenAverage(string symbol)
        {
            this.OpenAverage = (double)(await MySqlDB.GetSymbolOpenAverage(this.UserID, symbol));
        }*/
    }
}
