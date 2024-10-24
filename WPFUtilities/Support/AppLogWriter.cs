using System.Reflection;

namespace SwapAnalyzer.WPFUtilities.Support
{
    public class AppLogWriter
    {
        public static string GetAppDataPath()
        {
            return LogWriter.GetAppDataPath();
        }

        public static void WriteInLog(object data, MethodBase methodBase = null)
        {
            try
            {
                if (data == null)
                    return;
                string message = data.ToString();
                if (methodBase == null)
                {
                    LogWriter.WriteToLog(message);
                }
                else
                {
                    LogWriter.WriteToLog(methodBase.DeclaringType.Namespace, methodBase.DeclaringType.Name, methodBase.Name, message);
                }
            }
            catch (Exception ex)
            {
                MethodBase currentMethod = MethodBase.GetCurrentMethod();
                LogWriter.WriteToLog(currentMethod.DeclaringType.Namespace, currentMethod.DeclaringType.Name, currentMethod.Name, ex.StackTrace);
            }
        }

        public static void WriteInLog(object data, string filename)
        {
            try
            {
                if (data == null)
                    return;
                string message = data.ToString();
                LogWriter.WriteToLog(message, filename);
            }
            catch (Exception ex)
            {
                MethodBase currentMethod = MethodBase.GetCurrentMethod();
                LogWriter.WriteToLog(currentMethod.DeclaringType.Namespace, currentMethod.DeclaringType.Name, currentMethod.Name, ex.StackTrace);
            }
        }
    }
}
