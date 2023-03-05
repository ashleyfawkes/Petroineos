using Moq;
using Petroineos.DAPowerPositionReportService.Services;
using Services;

namespace Petroineos.DAPowerPositionReportServiceTests
{
    [TestFixture]
    public class PositionAggregatorTests
    {
        List<PowerTrade> _examplePowerTradelist;
        Tuple<int, string>[] _expectedlist;
        DateTime _exampleDate;

        [SetUp]
        public void Setup()
        {
            //Set up sample data
            _exampleDate = new DateTime(2015, 4, 1);
            _examplePowerTradelist = new List<PowerTrade>();
            var pt1 = PowerTrade.Create(_exampleDate, 24);
            pt1.Periods[0].Volume = 100;
            pt1.Periods[1].Volume = 100;
            pt1.Periods[2].Volume = 100;
            pt1.Periods[3].Volume = 100;
            pt1.Periods[4].Volume = 100;
            pt1.Periods[5].Volume = 100;
            pt1.Periods[6].Volume = 100;
            pt1.Periods[7].Volume = 100;
            pt1.Periods[8].Volume = 100;
            pt1.Periods[9].Volume = 100;
            pt1.Periods[10].Volume = 100;
            pt1.Periods[11].Volume = 100;
            pt1.Periods[12].Volume = 100;
            pt1.Periods[13].Volume = 100;
            pt1.Periods[14].Volume = 100;
            pt1.Periods[15].Volume = 100;
            pt1.Periods[16].Volume = 100;
            pt1.Periods[17].Volume = 100;
            pt1.Periods[18].Volume = 100;
            pt1.Periods[19].Volume = 100;
            pt1.Periods[20].Volume = 100;
            pt1.Periods[21].Volume = 100;
            pt1.Periods[22].Volume = 100;
            pt1.Periods[23].Volume = 100;
            _examplePowerTradelist.Add(pt1);

            var pt2 = PowerTrade.Create(_exampleDate, 24);
            pt2.Periods[0].Volume = 50;
            pt2.Periods[1].Volume = 50;
            pt2.Periods[2].Volume = 50;
            pt2.Periods[3].Volume = 50;
            pt2.Periods[4].Volume = 50;
            pt2.Periods[5].Volume = 50;
            pt2.Periods[6].Volume = 50;
            pt2.Periods[7].Volume = 50;
            pt2.Periods[8].Volume = 50;
            pt2.Periods[9].Volume = 50;
            pt2.Periods[10].Volume = 50;
            pt2.Periods[11].Volume = -20;
            pt2.Periods[12].Volume = -20;
            pt2.Periods[13].Volume = -20;
            pt2.Periods[14].Volume = -20;
            pt2.Periods[15].Volume = -20;
            pt2.Periods[16].Volume = -20;
            pt2.Periods[17].Volume = -20;
            pt2.Periods[18].Volume = -20;
            pt2.Periods[19].Volume = -20;
            pt2.Periods[20].Volume = -20;
            pt2.Periods[21].Volume = -20;
            pt2.Periods[22].Volume = -20;
            pt2.Periods[23].Volume = -20;
            _examplePowerTradelist.Add(pt2);

            _expectedlist = new Tuple<int, string>[24];
            _expectedlist[0] = new Tuple<int, string>(150, "23:00");
            _expectedlist[1] = new Tuple<int, string>(150, "00:00");
            _expectedlist[2] = new Tuple<int, string>(150, "01:00");
            _expectedlist[3] = new Tuple<int, string>(150, "02:00");
            _expectedlist[4] = new Tuple<int, string>(150, "03:00");
            _expectedlist[5] = new Tuple<int, string>(150, "04:00");
            _expectedlist[6] = new Tuple<int, string>(150, "05:00");
            _expectedlist[7] = new Tuple<int, string>(150, "06:00");
            _expectedlist[8] = new Tuple<int, string>(150, "07:00");
            _expectedlist[9] = new Tuple<int, string>(150, "08:00");
            _expectedlist[10] = new Tuple<int, string>(150, "09:00");
            _expectedlist[11] = new Tuple<int, string>(80, "10:00");
            _expectedlist[12] = new Tuple<int, string>(80, "11:00");
            _expectedlist[13] = new Tuple<int, string>(80, "12:00");
            _expectedlist[14] = new Tuple<int, string>(80, "13:00");
            _expectedlist[15] = new Tuple<int, string>(80, "14:00");
            _expectedlist[16] = new Tuple<int, string>(80, "15:00");
            _expectedlist[17] = new Tuple<int, string>(80, "16:00");
            _expectedlist[18] = new Tuple<int, string>(80, "17:00");
            _expectedlist[19] = new Tuple<int, string>(80, "18:00");
            _expectedlist[20] = new Tuple<int, string>(80, "19:00");
            _expectedlist[21] = new Tuple<int, string>(80, "20:00");
            _expectedlist[22] = new Tuple<int, string>(80, "21:00");
            _expectedlist[23] = new Tuple<int, string>(80, "22:00");
        }

        [Test]
        public void PositionAggregator_ConstructorTest()
        {
            Mock<IPowerService> mockPowerService = new Mock<IPowerService>();
            new PositionAggregator(mockPowerService.Object);
        }

        [Test]
        public async Task PositionAggregator_With1Trade_GetTradesAndAggregateAsyncTest()
        {
            DateTime date = new DateTime(2023,3,4,11,12,13);
            List<PowerTrade> list = new List<PowerTrade>();
            var pt = PowerTrade.Create(date, 3);
            pt.Periods[0].Volume = 123; 
            pt.Periods[1].Volume = 123; 
            pt.Periods[2].Volume = 123;
            list.Add(pt);

            Mock<IPowerService> mockPowerService = new Mock<IPowerService>();
            mockPowerService
                .Setup(x => x.GetTradesAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(list);
            var pa = new PositionAggregator(mockPowerService.Object);
            var result = await pa.GetTradesAndAggregateAsync(date, new TimeSpan(1,0,0));

            Assert.That(result.First().Period, Is.EqualTo("23:00"));
            Assert.That(result.First().Volume, Is.EqualTo(123));

            Mock.VerifyAll(mockPowerService);
        }

        [Test]
        public void PositionAggregator_With1Trade__GetTradesAndAggregateTest()
        {
            DateTime date = new DateTime(2023, 3, 4, 11, 12, 13);
            List<PowerTrade> list = new List<PowerTrade>();
            var pt = PowerTrade.Create(date, 3);
            pt.Periods[0].Volume = 123;
            pt.Periods[1].Volume = 123;
            pt.Periods[2].Volume = 123;
            list.Add(pt);

            var mockPowerService = new Mock<IPowerService>();
            mockPowerService
                .Setup(x => x.GetTrades(It.IsAny<DateTime>()))
                .Returns(list);

            var pa = new PositionAggregator(mockPowerService.Object);
            var result = pa.GetTradesAndAggregate(date, new TimeSpan(1,0,0));

            Assert.That(result.First().Period, Is.EqualTo("23:00"));
            Assert.That(result.First().Volume, Is.EqualTo(123));

            Mock.VerifyAll(mockPowerService);
        }

        [Test]
        public async Task PositionAggregator_With2Trades_GetTradesAndAggregateAsyncTest()
        {
            DateTime date = new DateTime(2023, 3, 4, 11, 12, 13);
            List<PowerTrade> list = new List<PowerTrade>();
            var pt1 = PowerTrade.Create(date, 3);
            pt1.Periods[0].Volume = 123;
            pt1.Periods[1].Volume = 123;
            pt1.Periods[2].Volume = 123;
            list.Add(pt1);
            var pt2 = PowerTrade.Create(date, 3);
            pt2.Periods[0].Volume = 321;
            pt2.Periods[1].Volume = 321;
            pt2.Periods[2].Volume = 321;
            list.Add(pt2);

            var mockPowerService = new Mock<IPowerService>();
            mockPowerService
                .Setup(x => x.GetTradesAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(list);

            var pa = new PositionAggregator(mockPowerService.Object);
            var result = await pa.GetTradesAndAggregateAsync(date, new TimeSpan(1, 0, 0));
            Assert.That(result.First().Period, Is.EqualTo("23:00"));
            Assert.That(result.First().Volume, Is.EqualTo(444));
            Mock.VerifyAll(mockPowerService);
        }

        [Test]
        public void PositionAggregator_Example_GetTradesAndAggregateTest()
        {
            var mockPowerService = new Mock<IPowerService>();
            mockPowerService
                .Setup(x => x.GetTrades(It.IsAny<DateTime>()))
                .Returns(_examplePowerTradelist);

            var pa = new PositionAggregator(mockPowerService.Object);
            var result = pa.GetTradesAndAggregate(_exampleDate, new TimeSpan(1, 0, 0)).ToArray();

            Assert.That(result.Length, Is.EqualTo(24));

            for (int i = 0; i < 24; i++)
            {
                Assert.That(result[i].Volume, Is.EqualTo(_expectedlist[i].Item1));
                Assert.That(result[i].Period, Is.EqualTo(_expectedlist[i].Item2));
            }

            Mock.VerifyAll(mockPowerService);
        }

        [Test]
        public async Task PositionAggregator_Example_GetTradesAndAggregateAsyncTest()
        {
            var mockPowerService = new Mock<IPowerService>();
            mockPowerService
                .Setup(x => x.GetTradesAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(_examplePowerTradelist);

            var pa = new PositionAggregator(mockPowerService.Object);
            var result = (await pa.GetTradesAndAggregateAsync(_exampleDate, new TimeSpan(1, 0, 0))).ToArray();

            Assert.That(result.Length, Is.EqualTo(24));

            for(int i = 0; i < 24; i++)
            {
                Assert.That(result[i].Volume, Is.EqualTo(_expectedlist[i].Item1));
                Assert.That(result[i].Period, Is.EqualTo(_expectedlist[i].Item2)); 
            }
            
            Mock.VerifyAll(mockPowerService);
        }
    }
}