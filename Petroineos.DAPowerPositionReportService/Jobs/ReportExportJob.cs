using Petroineos.Reports.Common.Interfaces;
using Quartz;

namespace Petroineos.DAPowerPositionReportService.Jobs
{
    [DisallowConcurrentExecution]
    public partial class ReportExportJob : IJob
    {
        private readonly ILogger<ReportExportJob> _logger;
        private readonly int _retryCount;
        private readonly int _retryDelayInMs;
        private readonly string _reportsPath;
        private readonly ITaskHelper _taskHelper;
        private readonly IPowerPositionReportService _powerPositionReportService;

        public ReportExportJob(ILogger<ReportExportJob> logger, IConfiguration configuration, ITaskHelper taskHelper, IPowerPositionReportService powerPositionReportService)
        {
            _logger = logger;
            _retryCount = configuration.GetValue<int>("RetryCount");
            _retryDelayInMs = configuration.GetValue<int>("RetryDelayInMs");
            _reportsPath = configuration.GetValue<string>("ReportsPath");
            _taskHelper = taskHelper;
            _powerPositionReportService = powerPositionReportService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"{nameof(ReportExportJob)} Started");

            await _taskHelper.DoWithRetryAsync(() => _powerPositionReportService.RunExport(), TimeSpan.FromMilliseconds(_retryDelayInMs), _retryCount);

            _logger.LogInformation($"{nameof(ReportExportJob)} Ended");
        }
    }
}
