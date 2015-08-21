using CQRS.Infrastructure.Domain;
using CQRS.Infrastructure.Domain.Factories;
using CQRS.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CQRS.Infrastructure.Helpers;

namespace CQRS.Infrastructure.Snapshots
{
    public class SnapshotRepository : IRepository
    {
        private readonly ISnapshotStore _snapshotStore;
        private readonly ISnapshotStrategy _snapshotStrategy;
        private readonly IRepository _repository;
        private readonly IEventStore _eventStore;

        public SnapshotRepository(ISnapshotStore snapshotStore, ISnapshotStrategy snapshotStrategy, IRepository repository, IEventStore eventStore)
        {
            if (snapshotStore == null)
                throw new ArgumentNullException("snapshotStore");
            if (snapshotStrategy == null)
                throw new ArgumentNullException("snapshotStrategy");
            if (repository == null)
                throw new ArgumentNullException("repository");
            if (eventStore == null)
                throw new ArgumentNullException("eventStore");

            _snapshotStore = snapshotStore;
            _snapshotStrategy = snapshotStrategy;
            _repository = repository;
            _eventStore = eventStore;
        }

        //public void Save<T>(T aggregate, int? exectedVersion = null) where T : AggregateRoot
        public void Save<T>(T aggregate, int? exectedVersion = null, Guid? commandId = null) where T : AggregateRoot
        {
            TryMakeSnapshot(aggregate);
            _repository.Save(aggregate, exectedVersion, commandId);
        }

        public T Get<T>(Guid aggregateId) where T : AggregateRoot
        {
            var aggregate = AggregateFactory.CreateAggregate<T>();
            var snapshotVersion = TryRestoreAggregateFromSnapshot(aggregateId, aggregate);
            if (snapshotVersion == -1)
            {
                return _repository.Get<T>(aggregateId);
            }
            var events = _eventStore.Get(aggregateId, snapshotVersion).Where(desc => desc.Version > snapshotVersion);
            aggregate.LoadFromHistory(events);

            return aggregate;
        }

        private int TryRestoreAggregateFromSnapshot<T>(Guid id, T aggregate)
        {
            var version = -1;
            if (_snapshotStrategy.IsSnapshotable(typeof(T)))
            {
                var snapshot = _snapshotStore.Get(id);
                if (snapshot != null)
                {
                    //aggregate.AsDynamic().Restore(snapshot);
                    ((dynamic)aggregate).Restore(snapshot);
                    version = snapshot.Version;
                }
            }
            return version;
        }

        private void TryMakeSnapshot(AggregateRoot aggregate)
        {
            if (!_snapshotStrategy.ShouldMakeSnapShot(aggregate))
                return;
            //var snapshot = aggregate.AsDynamic().GetSnapshot().RealObject;
            var snapshot = ((dynamic)aggregate).GetSnapshot().RealObject;
            snapshot.Version = aggregate.Version + aggregate.GetUncommittedChanges().Count();
            _snapshotStore.Save(snapshot);
        }


        public bool HasAggregate<T>(Guid aggregateId) where T : AggregateRoot
        {
            return _repository.HasAggregate<T>(aggregateId);
        }
    }
}
