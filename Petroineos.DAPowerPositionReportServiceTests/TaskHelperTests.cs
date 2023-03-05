using Microsoft.Extensions.Logging;
using Moq;
using Petroineos.Reports.Common.Threading;

namespace Petroineos.DAPowerPositionReportServiceTests
{
    [TestFixture]
    public class TaskHelperTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TaskHelper_ConstructorTest()
        {
            var mockLogger = new Mock<ILogger<TaskHelper>>();
            new TaskHelper(mockLogger.Object);
        }

        [Test]
        public async Task TaskHelper_NonFailing_DoWithRetryAsyncTest()
        {
            var mockLogger = new Mock<ILogger<TaskHelper>>();
            var taskHelper = new TaskHelper(mockLogger.Object);
            var retryCount = 5;
            var expectedCount = 1;
            var actualCount = 0;
            await taskHelper.DoWithRetryAsync(
                async () => await Task.Run(() => ++actualCount),
                TimeSpan.FromMilliseconds(1),
                retryCount);
            Assert.That(expectedCount, Is.EqualTo(actualCount));

            Mock.VerifyAll(mockLogger);
        }

        [Test]
        public async Task TaskHelper_Failing_DoWithRetryAsyncTest()
        {
            var mockLogger = new Mock<ILogger<TaskHelper>>();
            var taskHelper = new TaskHelper(mockLogger.Object);
            var retryCount = 5;
            var actualCount = 0;

            try
            {
                await taskHelper.DoWithRetryAsync(
                    async () => await Task.Run(() =>
                    {
                        ++actualCount;
                        throw new Exception();
                    }),
                    TimeSpan.FromMilliseconds(1),
                    retryCount);
            } 
            catch (Exception) { }

            Assert.That(retryCount, Is.EqualTo(actualCount));

            Mock.VerifyAll(mockLogger);
        }
    }
}