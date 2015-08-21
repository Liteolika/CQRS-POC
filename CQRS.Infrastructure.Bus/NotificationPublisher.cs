using CQRS.Infrastructure.Notifications;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Bus
{
    public class NotificationPublisher : INotificationPublisher
    {

        private readonly IServiceBus _bus;

        public NotificationPublisher(IServiceBus bus)
        {
            _bus = bus;
        }

        public void Publish<T>(T notification) where T : INotification
        {
            _bus.Publish(notification, notification.GetType());
        }
    }
}
