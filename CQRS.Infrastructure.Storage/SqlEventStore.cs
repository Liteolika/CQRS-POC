using CQRS.Infrastructure.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Storage
{
    public class SqlEventStore : IEventStore
    {

        private readonly Func<EventStoreDbContext> _dbFactory;
        private readonly JsonSerializerSettings _serializerSettings;

        public SqlEventStore(Func<EventStoreDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
            _serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
        }

        public void Save(IEvent @event)
        {
            using (var db = _dbFactory())
            {

                if (!db.Aggregates.Any(x => x.AggregateId == @event.Id))
                    db.Aggregates.Add(new AggregateItemDescriptor()
                    {
                        AggregateId = @event.Id
                    });

                db.Events.Add(new EventDescriptor()
                {
                    AggregateId = @event.Id,
                    EventData = SerializeEvent(@event),
                    Version = @event.Version,
                    EventId = Guid.NewGuid()
                });

                db.SaveChanges();

            }
        }

        public IEnumerable<IEvent> Get(Guid aggregateId, int fromVersion)
        {
            using (var db = _dbFactory())
            {

                List<IEvent> events = new List<IEvent>();

                var storedEvents = db.Events
                    .Where(x => x.AggregateId == aggregateId && x.Version > fromVersion)
                    .OrderBy(x => x.Version);

                foreach (var @event in storedEvents)
                {
                    events.Add(DeserializeEvent(@event.EventData));
                }

                return events;

            }
        }

        private string SerializeEvent(IEvent @event)
        {
            var evt = JsonConvert.SerializeObject(@event, _serializerSettings);
            return evt;
        }

        private IEvent DeserializeEvent(string eventData)
        {
            var evt = JsonConvert.DeserializeObject(eventData, _serializerSettings);
            return (IEvent)evt;
        }

    }
}
