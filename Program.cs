
using Serilog;
using SwapWorkerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(context.Configuration).CreateLogger();
        services.AddSerilog();
        services.AddHostedService<Worker>();

    })
     .ConfigureAppConfiguration((hostingContext, config) =>
     {
         config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
     }).Build();

await host.RunAsync();

