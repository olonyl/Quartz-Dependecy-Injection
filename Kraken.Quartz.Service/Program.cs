using Kraken.Quartz.Library.Job_Factory;
using Kraken.Quartz.Library.Jobs;
using Kraken.Quartz.Library.LoggerProvider;
using Kraken.Quartz.Library.Models;
using Kraken.Quartz.Library.Scheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kraken.Quartz.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                 .ConfigureServices((hostContext, services) =>
                 {
                    // services.AddHostedService<Worker>();
                    services.AddSingleton<IJobFactory, JobFactory>();
                     services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
                     services.AddSingleton<UserActivityJob>();
                     services.AddSingleton<NotificationJob>();
                     services.AddSingleton<ILogProvider, ConsoleLogProvider>();
                     services.AddSingleton(CreateJobsList());

                    //services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(UserActivityJob), "Notify job", "0/10 * * * * ?"));
                    //services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(NotificationJob), "Notify job", "0/5 * * * * ?"));

                    services.AddHostedService<MySchedulerMultiple>();

                 });

        private static IEnumerable<JobMetadata> CreateJobsList()
        {
            return new[] {
                new JobMetadata(Guid.NewGuid(), typeof(UserActivityJob), "Notify job", "0/5 * * * * ?"),
                new JobMetadata(Guid.NewGuid(), typeof(NotificationJob), "Notify job", "0/10 * * * * ?")
                };
        }
    }
}
