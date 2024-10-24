namespace SwapAnalyzer.Helpers
{
    internal class Database
    {
    //   public static string ConString = string.Empty;
  
    //    public Database()
    //    {
    //        Batteries.Init();
    //    }
    //    public static async Task<List<NetPosition>> GetNetPositons()
    //    {
    //        try
    //        {
    //            using (IDbConnection cnn = new SqliteConnection(ConString))
    //            {
    //                return (await cnn.QueryAsync<NetPosition>(Constants.QGetNetPosition, new DynamicParameters())).ToList();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            AppLogWriter.WriteInLog(ex, MethodBase.GetCurrentMethod());
    //        }
    //        return null;
    //    }

    /*   internal static async Task<double> GetSymbolOpenAverage(string userID, string symbol)
       {
           string query = $"SELECT SUM(OpenQty* filledPrice)/SUM(OpenQty) OpenAvg FROM Orders where userId='{userID}' AND symbol = '{symbol}'  AND openQty>0";
           return await GetSingleValue<double>(query);
       }*/

    //    private static async Task<T> GetSingleValue<T>(string query)
    //    {
    //        try
    //        {
    //            using (IDbConnection cnn = new SqliteConnection(ConString))
    //            {
    //                return (await cnn.QueryAsync<T>(query, new DynamicParameters())).FirstOrDefault();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            AppLogWriter.WriteInLog(ex, MethodBase.GetCurrentMethod());
    //        }
    //        return default(T);
    //    }
    }
}
