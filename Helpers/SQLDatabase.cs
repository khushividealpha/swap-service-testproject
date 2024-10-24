using System.Data;
using System.Data.SqlClient;

namespace SwapWorkerService.Helpers
{
    internal class SQLDatabase
    {
        public static string ConString { get; internal set; }

        public static double GetSymbolClose(string symbol)
        {
            double close = 0.0;
            string datetime = string.Empty;
            if (symbol.StartsWith("#"))
            {
                symbol = symbol.Substring(1, symbol.Length - 1);
            }
            using (SqlConnection connection = new SqlConnection(ConString))
            {
                connection.Open();
                //getsymbolclose =>procedure name
                using (SqlCommand cmd = new SqlCommand("GetSymbolClose", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters if your stored procedure has input parameters ----------here parameter use=>@Symbol 
                    cmd.Parameters.Add(new SqlParameter("@Symbol", SqlDbType.Text)
                    {
                        Value = symbol
                    });

                    // Execute the stored procedure
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Process each row in the result set
                            close = Convert.ToDouble(reader["close"]);
                            datetime = reader["date"].ToString();
                        }
                    }
                }
            }

            return close;
        }
    }
}
