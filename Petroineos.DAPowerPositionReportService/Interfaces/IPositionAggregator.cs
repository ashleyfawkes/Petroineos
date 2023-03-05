using Petroineos.DAPowerPositionReportService.Domain;

namespace Petroineos.Reports.Common.Interfaces
{
    public interface IPositionAggregator
    {
        Task<IEnumerable<AggregatedPosition>> GetTradesAndAggregateAsync(DateTime date, TimeSpan timeOffset);
        IEnumerable<AggregatedPosition> GetTradesAndAggregate(DateTime date, TimeSpan timeOffset);
    }
}
