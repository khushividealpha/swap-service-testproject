using System.Data;
using System.Reflection;
using MySqlConnector;
using SwapAnalyzer.WPFUtilities.Support;

namespace SwapWorkerService
{
    internal class MySqlDB
    {
        public static string MyConString { get; set; }
        public static async Task<List<T>> GetAllParam<T>(string query)
        {
            List<T> resultList = new List<T>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(MyConString))
                {
                    connection.Open();
                   // resultList = connection.Query<T>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                AppLogWriter.WriteInLog(ex, MethodBase.GetCurrentMethod());
            }

            return resultList;
        }

        internal static async Task<decimal> GetSymbolOpenAverage(string userID, string symbol)
        {
            try
            {
                // Create MySqlConnection instance
                using (MySqlConnection connection = new MySqlConnection(MyConString))
                {
                    // Open connection
                    connection.Open();

                    // Create MySqlCommand instance for calling stored procedure
                    using (MySqlCommand command = new MySqlCommand("GetSymbolOpenAverage", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add input parameters
                        command.Parameters.AddWithValue("@p_userID", userID);
                        command.Parameters.AddWithValue("@p_symbol", symbol);

                        // Add output parameter
                        command.Parameters.Add("@p_openAvg", MySqlDbType.Decimal);
                        command.Parameters["@p_openAvg"].Direction = ParameterDirection.Output;

                        // Execute the stored procedure asynchronously
                        command.ExecuteNonQuery();

                        // Get the output parameter value
                        if (command.Parameters["@p_openAvg"].Value != DBNull.Value)
                        {
                            return Convert.ToDecimal(command.Parameters["@p_openAvg"].Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogWriter.WriteInLog(ex, MethodBase.GetCurrentMethod());
            }
            return 0; // Default value if query fails or result is null
        }
    }
}
