using Petroineos.DAPowerPositionReportService.Interfaces;
using Petroineos.DAPowerPositionReportService.Domain;

namespace Petroineos.DAPowerPositionReportService.Services
{
    public class CsvReportWriter : ICsvReportWriter
    {
        public CsvReportWriter()
        {
        }

        public void WriteHeader(TextWriter writer)
        {
            writer.WriteLine($"Local Time,Volume");
        }

        public void WriteRow(TextWriter writer, AggregatedPosition position)
        {
            writer.WriteLine($"{position.Period},{position.Volume:0}");
        }
    }
}
