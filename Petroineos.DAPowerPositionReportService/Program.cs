using Petroineos.DAPowerPositionReportService.Interfaces;
using Petroineos.DAPowerPositionReportService.Services;
using Petroineos.Reports.Common.Interfaces;
using Quartz;
using Serilog;
using Serilog.Events;
using Services;
using Petroineos.Reports.Common.IO;
using Petroineos.Reports.Common.Threading;
using Petroineos.Reports.Common.Dates;
using Petroineos.DAPowerPositionReportService.Jobs;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        //Encapsulate the config
        var jobConfigurationProvider = new JobConfigurationProvider(hostContext.Configuration);

        //Register services
        services.AddSingleton<IJobConfigurationProvider>(jobConfigurationProvider);
        services.AddSingleton<IFileSystemProvider, FileSystemProvider>();
        services.AddSingleton<IPowerService, PowerService>();
        services.AddSingleton<ITaskHelper, TaskHelper>();
        services.AddSingleton<ITimeProvider, TimeProvider>();
        services.AddSingleton<ICsvReportWriter, CsvReportWriter>();
        services.AddSingleton<IFilePathProvider, FilePathProvider>();
        services.AddSingleton<IPositionAggregator, PositionAggregator>();
        services.AddSingleton<IPowerPositionReportService, PowerPositionReportService>();
        
        //Configure SerilLog
        Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
          .MinimumLevel.Override("Quartz", LogEventLevel.Information)
          .MinimumLevel.Debug()
          .Enrich.FromLogContext()
          .WriteTo.File(jobConfigurationProvider.LogFilePath, rollingInterval: RollingInterval.Day)
          .WriteTo.Console()
          .CreateLogger();
        Log.Logger.Information($"Logging to Console");
        Log.Logger.Information($"Logging to file path: {jobConfigurationProvider.LogFilePath}");

        // Add the Quartz.NET service (why write my own?) - see: https://www.quartz-scheduler.net
        Log.Logger.Information($"Triggering on Cron expression: {jobConfigurationProvider.JobCronExpression}");
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.AddJob<ReportExportJob>(opts => opts.WithIdentity(nameof(ReportExportJob)));
            q.AddTrigger(opts => opts
                .ForJob(nameof(ReportExportJob))
                .StartNow()); //start immediately;
            q.AddTrigger(opts => opts
                .ForJob(nameof(ReportExportJob))
                .WithCronSchedule(jobConfigurationProvider.JobCronExpression)); //and then run according to the schedule - cannot overlap with StartNow() as the quartz scheduler will make sure
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        
    })
    .UseSerilog()
    .Build();

await host.RunAsync();
