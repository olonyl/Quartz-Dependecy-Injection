using Kraken.Quartz.Library.Models;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kraken.Quartz.Library.Scheduler
{
    public class MyScheduler : IHostedService
    {
        private IScheduler _scheduler;
        private readonly IJobFactory _jobFactory;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly JobMetadata _metadata;

        public MyScheduler(IJobFactory jobFactory, ISchedulerFactory schedulerFactory, JobMetadata metadata)
        {
            _jobFactory = jobFactory;
            _schedulerFactory = schedulerFactory;
            _metadata = metadata;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //Create Scheduler
            _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            _scheduler.JobFactory = _jobFactory;
            //Create Jobs
            IJobDetail job = BuildJob(_metadata);
            //Create Triggers
            ITrigger trigger = BuildTrigger(_metadata);
            //Start working
            await _scheduler.ScheduleJob(job, trigger, cancellationToken);
            await _scheduler.Start(cancellationToken);
        }

        private ITrigger BuildTrigger(JobMetadata metadata)
        {
            return TriggerBuilder.Create()
                .WithIdentity(metadata.Id.ToString())
                .WithDescription(metadata.Name)
                .WithCronSchedule(metadata.CronSchedule)
                .Build();
        }

        private IJobDetail BuildJob(JobMetadata metadata)
        {
            return JobBuilder.Create(metadata.Type)
                .WithIdentity(metadata.Id.ToString())
                .WithDescription(metadata.Name)
                .Build();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _scheduler.Shutdown();
        }
    }
}
