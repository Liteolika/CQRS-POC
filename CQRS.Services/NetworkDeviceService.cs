using CQRS.Contracts;
using CQRS.Infrastructure.Commands;
using CQRS.Infrastructure.Events;
using CQRS.Infrastructure.Messages;
using CQRS.Infrastructure.Notifications;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Services
{
    public class NetworkDeviceService : INotificationHandler<HandlerNotification>
    {

        private readonly ICommandPublisher _publisher;

        public readonly Guid clientId;

        public NetworkDeviceService(ICommandPublisher publisher)
        {
            _publisher = publisher;
            clientId = Guid.NewGuid();
        }

        public event EventHandler<ServiceResult> ServiceResultRecieved;

        public void CreateDevice(Guid id, string hostname)
        {
            var command = new CreateNetworkDevice(id, hostname);
            PublishCommand(command);
        }

        public void SetDeviceHostname(Guid id, string newHostname)
        {
            var command = new ChangeNetworkDeviceHostName(id, newHostname);
            PublishCommand(command);
        }

        private void PublishCommand(ICommand command)
        {
            command.ClientId = clientId;
            _publisher.Publish(command);
        }

        public void Handle(HandlerNotification message)
        {
            if (message.ClientId == clientId)
            {
                Console.WriteLine("Got message for commandid: {0}", message.CommandId);

                var result = new ServiceResult()
                    {
                        Success = message.Success,
                        CommandId = message.ClientId,
                        ExceptionMessage = message.ExceptionMessage,
                        Message = message.Message
                    };

                if (ServiceResultRecieved != null)
                    ServiceResultRecieved(this, result);
            }
            else
            {
                Console.WriteLine("Got message but it was not mine...");
            }

        }
    }

    
    
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public Guid CommandId { get; set; }
    }

}
