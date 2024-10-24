namespace SwapWorkerService.Helpers
{
    internal static class Extensions
    {
        public static DateTime ConvertUtcToNormalDate(this Dictionary<string, object> dctChange, string key)
        {
            DateTime date = DateTime.MinValue;
            if (dctChange.ContainsKey(key))
            {
                string? utcDate = dctChange[key].ToString()!.Contains("Timestamp: ") ? dctChange[key].ToString()!.Split("Timestamp:")[1] : dctChange[key].ToString();
                if (DateTime.TryParseExact(utcDate.Replace(" ", ""), "yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out date))
                {
                    return date;
                }
            }
            return date;
        }
        public static T? GetFixValue<T>(this Dictionary<string, object> dctChange, string key)
        {
            try
            {
                if (dctChange.ContainsKey(key))
                {
                    object value = dctChange[key];
                    if (value is T)
                    {
                        return (T)value;
                    }
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                return default(T);
            }
            catch (Exception ex)
            {

                int test = 100;
            //    throw;
            }
            return (T)Convert.ChangeType(0, typeof(T));
        }
    }
}
