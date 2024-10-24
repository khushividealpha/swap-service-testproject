using SwapAnalyzer.Models;

namespace swap_service.Models
{
    internal class SwapFactory
    {
        public static ISwapUserSetting GetSwapObject(string method)
        {

           ISwapUserSetting userSetting= null;
            if (System.Enum.TryParse(method, out EnumSwapMethod swapMethod))
            {
                switch (swapMethod)
                {
                    case EnumSwapMethod.ByPoint:
                        userSetting = new ByPoint();
                        userSetting.Method = swapMethod;
                        break;
                    case EnumSwapMethod.ByPercentage:
                        userSetting = new ByPercentage();
                        userSetting.Method = swapMethod;
                        break;
                    case EnumSwapMethod.ByMoney:
                        userSetting = new ByMoney();
                        userSetting.Method = swapMethod;
                        break;
                }
            }
            return userSetting;
        }
    }
}
