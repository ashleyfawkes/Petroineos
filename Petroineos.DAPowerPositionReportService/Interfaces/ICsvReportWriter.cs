using Petroineos.DAPowerPositionReportService.Domain;

namespace Petroineos.DAPowerPositionReportService.Interfaces
{
    public interface ICsvReportWriter
    {
        void WriteHeader(TextWriter writer);
        public void WriteRow(TextWriter writer, AggregatedPosition position);
    }
}
