using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Petroineos.DAPowerPositionReportService.Domain;
using Petroineos.DAPowerPositionReportService.Interfaces;
using Petroineos.DAPowerPositionReportService.Services;
using Petroineos.Reports.Common.Interfaces;

namespace Petroineos.DAPowerPositionReportServiceTests
{
    [TestFixture]
    public class PowerPositionReportServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PowerPositionReportService_ConstructorTest()
        {
            var mockLogger = new Mock<ILogger<PowerPositionReportService>>();
            var mockJobConfigurationProvider = new Mock<IJobConfigurationProvider>();
            var mockPositionAggregator = new Mock<IPositionAggregator>();
            var mockTimeProvider = new Mock<ITimeProvider>();
            var mockCsvReportWriter = new Mock<ICsvReportWriter>();
            var mockFilePathProvider = new Mock<IFilePathProvider>();
            var mockFileSystemProvider = new Mock<IFileSystemProvider>();
            
            new PowerPositionReportService(
                mockLogger.Object,
                mockJobConfigurationProvider.Object,
                mockPositionAggregator.Object,
                mockTimeProvider.Object,
                mockFilePathProvider.Object,
                mockFileSystemProvider.Object,
                mockCsvReportWriter.Object
                );
        }

        [Test]
        public async Task PowerPositionReportService_RunExportTest()
        {
            var mockLogger = new Mock<ILogger<PowerPositionReportService>>();
            var date = new DateTime(2023, 2, 3, 22, 33, 44);

            var mockJobConfigurationProvider = new Mock<IJobConfigurationProvider>();
            mockJobConfigurationProvider
                .Setup(x => x.OffsetTimeSpan)
                .Returns(new TimeSpan(1,0,0));
            mockJobConfigurationProvider
                .Setup(x => x.ReportsFilePath)
                .Returns("c:\\temp\\testfile.csv");

            List<AggregatedPosition> list = new List<AggregatedPosition>();
            list.Add(new AggregatedPosition { Period = "01:00", Volume = 234 });
            list.Add(new AggregatedPosition { Period = "02:00", Volume = 334 });
            list.Add(new AggregatedPosition { Period = "03:00", Volume = 434 });
            var mockPositionAggregator = new Mock<IPositionAggregator>();
            mockPositionAggregator
                .Setup(x => x.GetTradesAndAggregateAsync(It.IsAny<DateTime>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(list);

            var mockTimeProvider = new Mock<ITimeProvider>();
            mockTimeProvider
                .Setup(x => x.Now())
                .Returns(date);
            int rowCount = 0;
            var mockCsvReportWriter = new Mock<ICsvReportWriter>();
            mockCsvReportWriter
                .Setup(x => x.WriteHeader(It.IsAny<TextWriter>()));
            mockCsvReportWriter
                .Setup(x => x.WriteRow(It.IsAny<TextWriter>(), It.IsAny<AggregatedPosition>()))
                .Callback(() => ++rowCount);

            var mockFilePathProvider = new Mock<IFilePathProvider>();
            mockFilePathProvider
                .Setup(x => x.GetFullFilePath(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns("c:\\temp\\testfile.csv");

            var sb = new StringBuilder();

            var mockFileSystemProvider = new Mock<IFileSystemProvider>();
            mockFileSystemProvider
                .Setup(x => x.CreateStreamWriter(It.IsAny<string>()))
                .Returns(new StringWriter(sb));

            var powerPositionReportService = new PowerPositionReportService(
                mockLogger.Object,
                mockJobConfigurationProvider.Object,
                mockPositionAggregator.Object,
                mockTimeProvider.Object,
                mockFilePathProvider.Object,
                mockFileSystemProvider.Object,
                mockCsvReportWriter.Object
                );

            await powerPositionReportService.RunExport();
            
            Assert.That(rowCount, Is.EqualTo(3));

            Mock.VerifyAll(mockLogger, 
                mockJobConfigurationProvider, 
                mockPositionAggregator, 
                mockTimeProvider, 
                mockCsvReportWriter, 
                mockFilePathProvider, 
                mockFileSystemProvider);
        }
    }
}