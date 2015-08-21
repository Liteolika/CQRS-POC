using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Notifications
{
    public interface INotificationPublisher
    {
        void Publish<T>(T notification) where T : INotification;
    }
}
