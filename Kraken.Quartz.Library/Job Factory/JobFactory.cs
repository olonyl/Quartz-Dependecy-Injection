using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Quartz.Library.Job_Factory
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _service;

        public JobFactory(IServiceProvider service)
        {
            _service = service;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            return (IJob)_service.GetService(jobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {

        }
    }
}
