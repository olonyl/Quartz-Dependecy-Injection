using Kraken.Quartz.Library.Job_Factory;
using Kraken.Quartz.Library.Jobs;
using Kraken.Quartz.Library.Models;
using Kraken.Quartz.Library.Scheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
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
                    //services.AddHostedService<Worker>();
                    services.AddSingleton<IJobFactory, JobFactory>()
                            .AddSingleton<ISchedulerFactory, StdSchedulerFactory>()
                            .AddSingleton<NotificationJob>();

                    services.AddSingleton(new JobMetadata(Guid.NewGuid(),"User Activity",typeof(NotificationJob), "0/10 * * * * ?"));

                    services.AddHostedService<MyScheduler>();
                });
    }
}
