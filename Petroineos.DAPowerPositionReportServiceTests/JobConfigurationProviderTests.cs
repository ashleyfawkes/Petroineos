using Moq;
using Petroineos.DAPowerPositionReportService.Services;
using Microsoft.Extensions.Configuration;

namespace Petroineos.DAPowerPositionReportServiceTests
{
    [TestFixture]
    public class JobConfigurationProviderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void JobConfigurationProviderTests_ConstructorTest()
        {
            var mockPowerService = new Mock<IConfiguration>();
            new JobConfigurationProvider(mockPowerService.Object);
        }

        [Test]
        public void TimeProvider_TestProperties()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(x => x["LogFilePath"]).Returns("c:\\TestLogFilePath");
            mockConfiguration.Setup(x => x["JobCronExpression"]).Returns("1 0/2 * * * ?");
            mockConfiguration.Setup(x => x["OffsetTimeSpan"]).Returns("12:34:56");
            mockConfiguration.Setup(x => x["ReportsFilePath"]).Returns("c:\\TestReportsFilePath");

            var jobConfigurationProvider = new JobConfigurationProvider(mockConfiguration.Object);

            Assert.That(jobConfigurationProvider.LogFilePath, Is.EqualTo("c:\\TestLogFilePath"));
            Assert.That(jobConfigurationProvider.JobCronExpression, Is.EqualTo("1 0/2 * * * ?"));
            Assert.That(jobConfigurationProvider.OffsetTimeSpan, Is.EqualTo(new TimeSpan(12, 34, 56)));
            Assert.That(jobConfigurationProvider.ReportsFilePath, Is.EqualTo("c:\\TestReportsFilePath"));
            
            Mock.VerifyAll(mockConfiguration);
        }
    }
}