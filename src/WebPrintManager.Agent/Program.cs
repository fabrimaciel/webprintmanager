using Microsoft.Extensions.Logging.EventLog;
using WebPrintManager.Agent;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(options =>
    {
        if (OperatingSystem.IsWindows())
        {
            options.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Information);
        }
    })
    .ConfigureServices((builder, services) =>
    {
        services.Configure<ManagerServerOptions>(builder.Configuration.GetSection(ManagerServerOptions.ManagerServer));
        services.AddScoped<ManagerServer>();
        services.AddHostedService<Worker>();

        if (OperatingSystem.IsWindows())
        {
            services.Configure<EventLogSettings>(config =>
            {
                if (OperatingSystem.IsWindows())
                {
                    config.LogName = "WebPrintManager Agent Service";
                    config.SourceName = "WebPrintManager Agent Service Source";
                }
            });
        }
    })
    .UseWindowsService()
    .Build();

host.Run();