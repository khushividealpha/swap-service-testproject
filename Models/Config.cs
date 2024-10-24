namespace swap_service.Models
{
    internal static class Config
    {
        public static string? AppName { get; set; }

        public static string? BaseFilePath { get; set; }

        public static string? MySqlConStr { get; set; }
        

        public static string? ProjID { get; set; }
        public static bool  IsLogEnabled { get; set; }
    }
}
