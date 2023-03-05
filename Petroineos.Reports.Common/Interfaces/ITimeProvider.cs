namespace Petroineos.Reports.Common.Interfaces
{
    public interface ITimeProvider
    {
        public DateTime Now();
        public DateTime UtcNow();
    }
}
