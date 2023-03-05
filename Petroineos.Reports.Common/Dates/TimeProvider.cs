using Petroineos.Reports.Common.Interfaces;

namespace Petroineos.Reports.Common.Dates
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime Now() => DateTime.Now;
        public DateTime UtcNow() => DateTime.UtcNow;
    }
}
