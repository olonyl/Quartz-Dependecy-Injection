using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Quartz.Library.Models
{
    public class JobMetadata
    {
        public JobMetadata(Guid jobId, Type jobType, string jobName, string cronExpression)
        {
            JobId = jobId;
            JobType = jobType;
            CronExpression = cronExpression;
            JobName = jobName;
        }

        public Guid JobId { get; set; }
        public Type JobType { get; set; }
        public string CronExpression { get; set; }
        public string JobName { get; set; }
    }
}
