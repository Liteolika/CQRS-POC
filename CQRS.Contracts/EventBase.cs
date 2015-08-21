using CQRS.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Contracts
{
    public class EventBase : IEvent
    {
        public Guid Id { get; set; }
        public Guid ByCommandId { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        public EventBase(Guid id, int version)
        {
            Id = id;
            Version = version;
        }

    }
}
