using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Notifications
{
    public class HandlerNotification : INotification
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid CommandId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public string ExceptionMessage { get; set; }

        public HandlerNotification()
        {
            this.TimeStamp = DateTime.Now;
        }

    }
}
