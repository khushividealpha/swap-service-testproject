using SwapAnalyzer.Models;

namespace SwapWorkerService.Models
{
    internal class UserSwap
    {
        public string UserID { get; set; }

        public ISwapUserSetting setting { get; set; }

        public double SwapRate { get; set; }

        public Decimal OpenQty { get; set; }

        private Dictionary<string, object> swapMap;

        public Dictionary<string, object> UserSymbolSwapMap
        {
            get
            {
                swapMap = new Dictionary<string, object>()
                {
                     { "instrument", setting.Symbol },
                    { "swapRate", Math.Round ( setting.SwapRate,2) },
                    { "openQty", OpenQty  }
                };
                return swapMap;
            }
            set { swapMap = value; }
        }


    }
}
