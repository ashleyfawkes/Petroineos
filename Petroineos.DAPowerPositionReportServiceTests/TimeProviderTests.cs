using Petroineos.Reports.Common.Dates;

namespace Petroineos.DAPowerPositionReportServiceTests
{
    [TestFixture]
    public class TimeProviderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TimeProvider_ConstructorTest()
        {
            new TimeProvider();
        }

        [Test]
        public void TimeProvider_NowTest()
        {
            var timeProvider = new TimeProvider();
            var actual = timeProvider.Now();
            var expected = DateTime.Now;

            Assert.IsTrue(actual.Subtract(expected).TotalHours < 1);
        }
    }
}