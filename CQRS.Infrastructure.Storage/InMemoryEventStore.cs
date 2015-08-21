using CQRS.Infrastructure.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Storage
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly ConcurrentDictionary<Guid, List<IEvent>> _inMemoryDB =
            new ConcurrentDictionary<Guid, List<IEvent>>();

        public void Save(IEvent @event)
        {
            List<IEvent> list;
            _inMemoryDB.TryGetValue(@event.Id, out list);
            if (list == null)
            {
                list = new List<IEvent>();
                _inMemoryDB.TryAdd(@event.Id, list);
            }
            list.Add(@event);
        }

        public IEnumerable<IEvent> Get(Guid aggregateId, int fromVersion)
        {

            List<IEvent> events;
            _inMemoryDB.TryGetValue(aggregateId, out events);
            return events != null
                ? events.Where(x => x.Version > fromVersion)
                : new List<IEvent>();
        }

    }
}
