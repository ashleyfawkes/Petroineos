using Petroineos.DAPowerPositionReportService.Domain;
using Petroineos.Reports.Common.Interfaces;
using Services;

namespace Petroineos.DAPowerPositionReportService.Services
{
    public class PositionAggregator : IPositionAggregator
    {
        private IPowerService _powerService;
        public PositionAggregator(IPowerService powerService) 
        {
            _powerService = powerService;
        }

        public async Task<IEnumerable<AggregatedPosition>> GetTradesAndAggregateAsync(DateTime date, TimeSpan timeOffset)
        {
            var powerTrades = await _powerService.GetTradesAsync(date.Date);
            return powerTrades.SelectMany(x => x.Periods)
                  .GroupBy(p => FormatPeriodString(date.Date.Subtract(timeOffset).AddHours(p.Period - 1)))
                  .Select(periodGrp => new AggregatedPosition
                  {
                      Period = periodGrp.Key,
                      Volume = periodGrp.Sum(prd => prd.Volume)
                  });
        }

        public IEnumerable<AggregatedPosition> GetTradesAndAggregate(DateTime date, TimeSpan timeOffset)
        {
            var powerTrades = _powerService.GetTrades(date.Date);
            return powerTrades.SelectMany(x => x.Periods)
                  .GroupBy(p => FormatPeriodString(date.Date.Subtract(timeOffset).AddHours(p.Period - 1)))
                  .Select(periodGrp => new AggregatedPosition
                  {
                      Period = periodGrp.Key,
                      Volume = periodGrp.Sum(prd => prd.Volume)
                  });
        }

        public string FormatPeriodString(DateTime date)
        {
            return date.ToString("HH:mm");
        }
    }
}
