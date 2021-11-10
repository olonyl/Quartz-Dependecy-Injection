using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Quartz.Library.Models
{
    public class JobMetadata
    {
        public JobMetadata(Guid id, string name, Type type, string cronValue)
        {
            Id = id;
            Name = name;
            Type = type;
            CronSchedule = cronValue;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public string CronSchedule { get; set; }
    }
}
