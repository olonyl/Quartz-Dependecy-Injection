using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Quartz.Library.Jobs
{
    public class UserActivityJob : IJob
    {
        private readonly ILogger<UserActivityJob> _logger;

        public UserActivityJob(ILogger<UserActivityJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"{context.JobDetail.JobType} - User Activity at: {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
