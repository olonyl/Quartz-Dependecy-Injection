using Kraken.Quartz.Library.Models;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Logging;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kraken.Quartz.Library.Scheduler
{
    public class MySchedulerSingle : IHostedService
    {
        public IScheduler Scheduler { get; set; }
        private readonly IJobFactory _jobFactory;
        private readonly JobMetadata _jobMetadata;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ILogProvider _logProvider;

        public MySchedulerSingle(IJobFactory jobFactory
            , JobMetadata jobMetadata
            , ISchedulerFactory schedulerFactory
            , ILogProvider logProvider)
        {
            _jobFactory = jobFactory;
            _jobMetadata = jobMetadata;
            _schedulerFactory = schedulerFactory;
            _logProvider = logProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            LogProvider.SetCurrentLogProvider(_logProvider);
            //Create Scheduler
            Scheduler = await _schedulerFactory.GetScheduler();
            Scheduler.JobFactory = _jobFactory;
            //Create Job
            IJobDetail jobDetail = CreateJob(_jobMetadata);
            //Create Trigger
            ITrigger trigger = CreateTrigger(_jobMetadata);
            //Schedule Job
            await Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
            //Start Scheduler
            await Scheduler.Start(cancellationToken);
        }

        private ITrigger CreateTrigger(JobMetadata jobMetadata)
        {
            return TriggerBuilder.Create()
                .WithIdentity(jobMetadata.JobId.ToString())
                .WithCronSchedule(jobMetadata.CronExpression)
                .WithDescription(jobMetadata.JobName)
                .Build();
        }

        private IJobDetail CreateJob(JobMetadata jobMetadata)
        {
            return JobBuilder.Create(jobMetadata.JobType)
                  .WithIdentity(jobMetadata.JobId.ToString())
                  .WithDescription(jobMetadata.JobName)
                  .Build();
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler.Shutdown();
        }
    }
}
