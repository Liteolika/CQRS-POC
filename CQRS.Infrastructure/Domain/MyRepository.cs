using CQRS.Infrastructure.Domain.Exceptions;
using CQRS.Infrastructure.Domain.Factories;
using CQRS.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Domain
{
    public class MyRepository : IRepository
    {

        private readonly IEventStore _eventStore;
        private readonly IEventPublisher _publisher;

        public MyRepository(IEventStore eventStore, IEventPublisher publisher)
        {
            if (eventStore == null)
                throw new ArgumentNullException("eventStore");
            if (publisher == null)
                throw new ArgumentNullException("publisher");
            
            _eventStore = eventStore;
            _publisher = publisher;
        }

        public void Save<T>(T aggregate, int? expectedVersion = null, Guid? commandId = null) where T : AggregateRoot
        {
            if (expectedVersion != null && _eventStore.Get(
                    aggregate.Id, expectedVersion.Value).Any())
                throw new ConcurrencyException(aggregate.Id);

            var i = 0;
            foreach (var @event in aggregate.GetUncommittedChanges())
            {
                if (@event.Id == Guid.Empty)
                    @event.Id = aggregate.Id;
                if (@event.Id == Guid.Empty)
                    throw new AggregateOrEventMissingIdException(
                        aggregate.GetType(), @event.GetType());
                i++;

                if (commandId.HasValue)
                    @event.ByCommandId = commandId.Value;

                @event.Version = aggregate.Version + i;
                @event.TimeStamp = DateTimeOffset.UtcNow;
                _eventStore.Save(@event);
                _publisher.Publish(@event);
            }
            aggregate.MarkChangesAsCommitted();
        }

        public T Get<T>(Guid aggregateId) where T : AggregateRoot
        {
            return LoadAggregate<T>(aggregateId);
        }

        public bool HasAggregate<T>(Guid aggregateId) where T : AggregateRoot
        {
            var events = _eventStore.Get(aggregateId, 0);
            return events.Any();
        }

        private T LoadAggregate<T>(Guid id) where T : AggregateRoot
        {
            var aggregate = AggregateFactory.CreateAggregate<T>();
           
            var events = _eventStore.Get(id, -1);
            if (!events.Any())
                throw new AggregateNotFoundException(id);
            aggregate.LoadFromHistory(events);

            return aggregate;
        }

    }
}
