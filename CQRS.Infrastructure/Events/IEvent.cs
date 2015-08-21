using CQRS.Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Events
{
    public interface IEvent : IMessage
    {
        Guid Id { get; set; }
        Guid ByCommandId { get; set; }
        int Version { get; set; }
        DateTimeOffset TimeStamp { get; set; }

    }
}
