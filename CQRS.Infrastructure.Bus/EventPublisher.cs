using CQRS.Infrastructure.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Bus
{
    public class EventPublisher : IEventPublisher
    {

        private readonly IServiceBus _bus;

        public EventPublisher(IServiceBus bus)
        {
            _bus = bus;
        }

        public void Publish<T>(T @event) where T : IEvent
        {
            _bus.Publish(@event, @event.GetType());
            Console.WriteLine("Event published. Version: {0}", @event.Version);
        }

    }
}
