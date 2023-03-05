using Microsoft.Extensions.Logging;
using Petroineos.Reports.Common.Interfaces;

namespace Petroineos.Reports.Common.Threading
{
    public class TaskHelper : ITaskHelper
    {
        private readonly ILogger<TaskHelper> _logger;
        public TaskHelper(ILogger<TaskHelper> logger)
        {
            _logger = logger;
        }

        public async Task DoWithRetryAsync(Func<Task> action, TimeSpan sleepPeriod, int tryCount)
        {
            if (tryCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(tryCount));

            while (true)
            {
                try
                {
                    await action();
                    return; // success!
                }
                catch (Exception exception)
                {
                    if (--tryCount > 0)
                    {
                        _logger.LogWarning($"Action Failed: {exception.Message}");
                        _logger.LogWarning($"Retrying");
                    }
                    else
                    {
                        _logger.LogError($"Action Failed: {exception}");
                        _logger.LogError($"Stopped Retrying");
                        throw;
                    }
                    await Task.Delay(sleepPeriod);
                }
            }
        }
    }
}
