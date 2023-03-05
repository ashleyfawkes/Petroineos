using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Petroineos.Reports.Common.Interfaces;
using Petroineos.DAPowerPositionReportService.Domain;
using Petroineos.DAPowerPositionReportService.Services;
using Petroineos.DAPowerPositionReportService.Interfaces;

namespace Petroineos.DAPowerPositionReportService.Services
{
    public class PowerPositionReportService : IPowerPositionReportService
    {
        private readonly IFileSystemProvider _fileSystemProvider;
        private readonly IPositionAggregator _positionAggregator;
        private readonly ITimeProvider _timeProvider;
        private readonly ILogger<PowerPositionReportService> _logger;
        private readonly IJobConfigurationProvider _jobConfigurationProvider;
        private readonly ICsvReportWriter _csvReportWriter;
        private readonly IFilePathProvider _filePathProvider;

        public PowerPositionReportService(
            ILogger<PowerPositionReportService> logger,
            IJobConfigurationProvider jobConfigurationProvider, 
            IPositionAggregator positionAggregator, 
            ITimeProvider timeProvider, 
            IFilePathProvider filePathProvider,
            IFileSystemProvider fileSystemProvider,
            ICsvReportWriter csvReportWriter)
        {
            _fileSystemProvider = fileSystemProvider;
            _filePathProvider = filePathProvider;
            _csvReportWriter = csvReportWriter;
            _logger = logger;
            _jobConfigurationProvider = jobConfigurationProvider;
            _timeProvider = timeProvider;
            _positionAggregator = positionAggregator;
        }

        public async Task RunExport()
        {
            var now = _timeProvider.Now();
            var fullFilePath = _filePathProvider.GetFullFilePath(_jobConfigurationProvider.ReportsFilePath, now);

            _logger.LogInformation($"Generating report at {now: dd-MM-yyyy HH:mm:ss}");

            var aggregatedTrades = await _positionAggregator.GetTradesAndAggregateAsync(now.Date, _jobConfigurationProvider.OffsetTimeSpan);

            using (var writer = _fileSystemProvider.CreateStreamWriter(fullFilePath))
            {
                _csvReportWriter.WriteHeader(writer);
                foreach (var aggregatedTrade in aggregatedTrades)
                {
                    _csvReportWriter.WriteRow(writer, aggregatedTrade);
                }
            }

            _logger.LogInformation($"Report written to file: {fullFilePath}");
        }

    }
}
