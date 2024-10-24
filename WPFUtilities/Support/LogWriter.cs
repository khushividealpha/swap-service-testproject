using swap_service.Models;
using System.Text;

namespace SwapAnalyzer.WPFUtilities.Support
{
    public static class LogWriter
    {
        private static StreamWriter writer = (StreamWriter)null;
        private static string LogFilePath = (string)null;
        // private static string AppName = string.Empty;
        //  private static bool IsLogEnable = false;

        static LogWriter()
        {
            //AppName = Config.AppName;
            //IsLogEnable = Config.IsLogEnabled;
        }

        public static void Init(string FilePath)
        {
            LogFilePath = FilePath;
        }

        public static void WriteLine(string LogText)
        {
            if (writer == null)
                writer = new StreamWriter((Stream)File.Open(LogFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
            try
            {
                writer.WriteLine(LogText);
            }
            catch (Exception)
            {
                if (writer == null)
                    return;
                writer.Dispose();
            }
        }

        public static void Close()
        {
            if (writer == null)
                return;
            writer.Dispose();
            writer = (StreamWriter)null;
        }

        public static void WriteToLog(
          string Namespace,
          string Classfile,
          string Method,
          string message)
        {
            if (!Config.IsLogEnabled)
            {
                return;
            }
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                stringBuilder.AppendLine(DateTime.Now.ToString() + "  -  Namespace : " + Namespace + " ");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Error Description :- ");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(message);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Class :- " + Classfile + " : " + Method);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("------------------------------------------------------------------");
                stringBuilder.AppendLine();
                WriteToFile(string.Format("{0}Log", (object)Config.AppName), stringBuilder.ToString());
            }
            catch (Exception)
            {
                //   Console.WriteLine(ex.Message);
            }
        }

        public static string GetAppDataPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Config.AppName);
        }

        public static void WriteToLog(string message, string filename = "Trace")
        {
            if (!Config.IsLogEnabled)
            {
                return;
            }
            WriteToFile(filename, message);
        }

        private static readonly object _lock = new object();

        public static void WriteToFile(string fileFixedKeyword, string message)
        {
            string empty1 = string.Empty;
            string empty2 = string.Empty;
            string path = Path.Combine(GetAppDataPath(), "Logs");
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                lock (_lock)
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)new FileStream(path + "\\" + fileFixedKeyword + " " + (object)DateTime.Now.Day + "-" + (object)DateTime.Now.Month + "-" + (object)DateTime.Now.Year + ".txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                    {

                        streamWriter.Write(string.Format("\n\r{0} {1}", message, DateTime.Now));

                    }
                }
            }
            catch
            {
                //throw;
            }
        }
    }
}
