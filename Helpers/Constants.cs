namespace SwapAnalyzer.Helpers
{
    public static class Constants
    {
        public const string QGetNetPosition = @"Select ord.*,Instr.BidMultiplier as ContractMultiplier from (SELECT userId,SUM(CASE WHEN buy = 1 THEN OpenQty WHEN buy = 0 THEN -1 * OpenQty END) AS NetQty,symbol, SUM(OpenQty * filledPrice) / SUM(OpenQty) AS OpenAvg  FROM  Orders  WHERE status like '%FILLED%' and openQty > 0 GROUP BY userId,symbol) ord
Left join Instruments  instr on ord.symbol= Instr.Instrument";
       
    }
}
