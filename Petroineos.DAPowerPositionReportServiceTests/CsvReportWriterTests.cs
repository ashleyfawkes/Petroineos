using Microsoft.Extensions.Logging;
using Moq;
using Petroineos.DAPowerPositionReportService.Domain;
using Petroineos.DAPowerPositionReportService.Services;
using System.Text;

namespace Petroineos.DAPowerPositionReportServiceTests
{
    public class CsvReportWriterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CsvReportWriter_ConstructorTest()
        {
            new CsvReportWriter();
        }

        [Test]
        public void CsvReportWriter_WriteHeaderTest()
        {
            var csvReportWriter = new CsvReportWriter();
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);
            csvReportWriter.WriteHeader(writer);

            Assert.That(sb.ToString(), Is.EqualTo($"Local Time,Volume\r\n"));
        }

        [Test]
        public void CsvReportWriter_WriteRowTest()
        {
            var csvReportWriter = new CsvReportWriter();
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);
            var pos = new AggregatedPosition { Period="04:00", Volume=123.45 };
            csvReportWriter.WriteRow(writer, pos);

            Assert.That(sb.ToString(), Is.EqualTo($"04:00,123\r\n"));
        }
    }
}