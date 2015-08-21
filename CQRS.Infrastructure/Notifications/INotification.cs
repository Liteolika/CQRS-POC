using CQRS.Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Notifications
{
    public interface INotification : IMessage
    {
        Guid Id { get; set; }
        Guid ClientId { get; set; }
        DateTimeOffset TimeStamp { get; set; }
        string Message { get; set; }
        bool Success { get; set; }
        string ExceptionMessage { get; set; }
    }
}
