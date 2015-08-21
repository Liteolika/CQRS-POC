using CQRS.Infrastructure.Commands;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Bus
{
    public class CommandPublisher : ICommandPublisher
    {

        private readonly IServiceBus _bus;

        public CommandPublisher(IServiceBus bus)
        {
            _bus = bus;
        }

        public void Publish<T>(T command) where T : ICommand
        {
            _bus.Publish(command, command.GetType());
        }
    }
}
