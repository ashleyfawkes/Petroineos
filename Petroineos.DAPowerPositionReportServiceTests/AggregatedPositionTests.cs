using Petroineos.DAPowerPositionReportService.Domain;

namespace Petroineos.DAPowerPositionReportServiceTests
{
    public class AggregatedPositionTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AggregatedPosition_ConstructorTest()
        {
            new AggregatedPosition();
        }

        [Test]
        public void AggregatedPosition_PropertiesTest()
        {
            var aggregatedPosition = new AggregatedPosition();
            aggregatedPosition.Volume = 100;
            aggregatedPosition.Period = "03:00";
            Assert.That(aggregatedPosition.Volume, Is.EqualTo(100));
            Assert.That(aggregatedPosition.Period, Is.EqualTo("03:00"));
        }
    }
}