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
    public class MySchedulerMultiple : IHostedService
    {
        public IScheduler Scheduler { get; set; }
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobMetadata> _jobMetadatas;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ILogProvider _logProvider;

        public MySchedulerMultiple(IJobFactory jobFactory
            , IEnumerable<JobMetadata> jobMetadatas
            , ISchedulerFactory schedulerFactory
            , ILogProvider logProvider)
        {
            _jobFactory = jobFactory;
            _jobMetadatas = jobMetadatas;
            _schedulerFactory = schedulerFactory;
            _logProvider = logProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            LogProvider.SetCurrentLogProvider(_logProvider);

            //Create Scheduler
            Scheduler = await _schedulerFactory.GetScheduler();
            Scheduler.JobFactory = _jobFactory;
            await BuildExecutionList(_jobMetadatas, cancellationToken);
               //Start Scheduler
               await Scheduler.Start(cancellationToken);
        }

        private async Task BuildExecutionList(IEnumerable<JobMetadata> jobMetadatas,CancellationToken cancellationToken)
        {
            foreach(var jobMetadata in jobMetadatas)
            {
                //Create Job
                IJobDetail jobDetail = CreateJob(jobMetadata);
                //Create Trigger
                ITrigger trigger = CreateTrigger(jobMetadata);
                //Schedule Job
                await Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
            }
            //return Task.CompletedTask;

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
            await Scheduler.Shutdown(cancellationToken);
        }
    }
}
