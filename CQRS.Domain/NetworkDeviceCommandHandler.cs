using CQRS.Contracts;
using CQRS.Infrastructure.Commands;
using CQRS.Infrastructure.Domain;
using CQRS.Infrastructure.Events;
using CQRS.Infrastructure.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Domain
{
    public class NetworkDeviceCommandHandler : 
        ICommandHandler<CreateNetworkDevice>,
        ICommandHandler<ChangeNetworkDeviceHostName>
    {

        private readonly ISession _session;
        private readonly INotificationPublisher _publisher;

        public NetworkDeviceCommandHandler(ISession session, INotificationPublisher publisher)
        {
            _session = session;
            _publisher = publisher;
        }

        private void SendNotification(ICommand command, bool success = true, string message = "", Exception exception = null)
        {
            HandlerNotification notification = new HandlerNotification();
            notification.ClientId = command.ClientId;
            notification.CommandId = command.CommandId;
            notification.Success = success;
            notification.Message = message;
            if (exception != null)
                notification.ExceptionMessage = string.Format("{0}: {1}", exception.GetType().Name, exception.Message);
            _publisher.Publish(notification);
        }

        public void Handle(CreateNetworkDevice message)
        {
            try
            {
                if (_session.Any<NetworkDevice>(message.Id))
                    throw new AggregateException("Networkdevice already exists");

                var device = new NetworkDevice(message.Id, message.Hostname);
                _session.Add(device);
                _session.Commit(message.CommandId);

                SendNotification(message, true, "Network device created successfully.");

            }
            catch (Exception ex)
            {
                SendNotification(message, false, "Could not add network device", ex);
            }
        }

        public void Handle(ChangeNetworkDeviceHostName message)
        {
            try
            {
                NetworkDevice device = _session.Get<NetworkDevice>(message.Id);
                device.SetHostname(message.NewHostname);
                _session.Commit(message.CommandId);
                SendNotification(message, true, "Network device updated successfully.");
            }
            catch (Exception ex)
            {
                SendNotification(message, false, "Could not change hostname", ex);
            }
        }

    }
}
