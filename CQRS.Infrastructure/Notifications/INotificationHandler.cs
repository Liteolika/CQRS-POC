using CQRS.Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Notifications
{

    public interface INotificationHandler<T> : IHandler<T> where T : INotification
    {
    }
}
