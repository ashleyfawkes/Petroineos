using Petroineos.DAPowerPositionReportService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petroineos.DAPowerPositionReportService.Services
{
    public class JobConfigurationProvider : IJobConfigurationProvider
    {
        private readonly IConfiguration _configuration;
        public JobConfigurationProvider(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public string LogFilePath => _configuration["LogFilePath"];
        public string JobCronExpression => _configuration["JobCronExpression"];
        public TimeSpan OffsetTimeSpan => TimeSpan.Parse(_configuration["OffsetTimeSpan"]);
        public string ReportsFilePath => _configuration["ReportsFilePath"];
    }
}
