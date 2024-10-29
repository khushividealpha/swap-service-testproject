using swap_service.Models;
using SwapAnalyzer.Helpers;
using SwapAnalyzer.WPFUtilities.Support;
using SwapWorkerService.Helpers;

namespace SwapWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        public string timeFile { get; set; }

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {


            _configuration = configuration;
            _logger = logger;
            Config.AppName = _configuration["MyConfig:AppName"];
            Config.ProjID = _configuration["MyConfig:ProjID"];
            Config.BaseFilePath = _configuration["MyConfig:FileBasePath"];


            Config.IsLogEnabled = Convert.ToBoolean(_configuration["MyConfig:IsLogEnabled"]);
            MySqlDB.MyConString = _configuration["MyConfig:MySqlConStr"];
            SQLDatabase.ConString = _configuration["MyConfig:SQLServerConString"];

            //  AppUtilities.SetEnvironmentVar();
      

        }
        static bool isRunToday = false;
        DateTime TriggerTime = DateTime.MinValue;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            try
            {
                AppLogWriter.WriteInLog("Service1 Started");
                Console.WriteLine(AppUtilities.AppendTime("Exe started "));
                //TriggerTime = AppUtilities.ConvertToDateTime(File.ReadAllText(timeFile), "HH:mm:ss");
                isRunToday = true;
                AppLogWriter.WriteInLog("Swap analyzer exe started");
                Console.WriteLine(AppUtilities.AppendTime("Swap analyzer exe started"));
               await AppUtilities.CalculateSwap();
                isRunToday = false;
                AppLogWriter.WriteInLog("Service Started");
                timeFile = Path.Combine(LogWriter.GetAppDataPath(), "Trigger.csv");
                if (!File.Exists(timeFile))
                {

                    using (StreamWriter sw = new StreamWriter(new FileStream(timeFile, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite)))
                    {
                        sw.Write("21:00:00");
                    }
                }
              
                    if (DateTime.Now >= TriggerTime)
                    {   
                        TriggerTime = TriggerTime.AddDays(1);
                        DBFireBase.InitTime();
                        AppLogWriter.WriteInLog("Swap calculation Started");
                        Console.WriteLine(AppUtilities.AppendTime("Swap calculation Started "));
                      // await AppUtilities.CalculateSwap();
                    }
                   // await Task.Delay(1000, stoppingToken);
                
            }
            catch (Exception ex)
            {

                AppLogWriter.WriteInLog(ex);
            }
        }
    }
}