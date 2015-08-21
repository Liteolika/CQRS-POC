using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Storage
{
    public class EventDescriptor
    {
        [Key]
        public Guid EventId { get; set; }
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public string EventData { get; set; }

    }
}
