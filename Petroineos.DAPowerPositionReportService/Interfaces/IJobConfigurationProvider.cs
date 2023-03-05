using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petroineos.DAPowerPositionReportService.Interfaces
{
    public interface IJobConfigurationProvider
    {
        string LogFilePath { get; }
        string JobCronExpression { get; }
        public TimeSpan OffsetTimeSpan { get; }
        public string ReportsFilePath { get; }
    }
}
