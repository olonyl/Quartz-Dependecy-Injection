using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Kraken.Quartz.Library.Jobs
{
    public class NotificationJob : IJob
    {
        private readonly ILogger<NotificationJob> _logger;

        public NotificationJob(ILogger<NotificationJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"User Activity at : {DateTime.Now.ToString()} for JobType: {context.JobDetail.JobType}");
            return Task.CompletedTask;
        }
    }
}
