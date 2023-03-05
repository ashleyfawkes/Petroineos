namespace Petroineos.Reports.Common.Interfaces
{
    public interface ITaskHelper
    {
        Task DoWithRetryAsync(Func<Task> action, TimeSpan sleepPeriod, int tryCount);
    }
}
