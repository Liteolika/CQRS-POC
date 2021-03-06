﻿using CQRS.Infrastructure.Domain.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Domain
{
    public class MySession : ISession
    {
        private readonly IRepository _repository;
        private readonly Dictionary<Guid, AggregateDescriptor> _trackedAggregates;

        public MySession(IRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _repository = repository;
            _trackedAggregates = new Dictionary<Guid, AggregateDescriptor>();
        }

        public bool Any<T>(Guid id) where T : AggregateRoot
        {
            return _repository.HasAggregate<T>(id);
        }

        public void Add<T>(T aggregate) where T : AggregateRoot
        {
            lock (_trackedAggregates)
            {

                if (!IsTracked(aggregate.Id))
                    _trackedAggregates.Add(aggregate.Id,
                        new AggregateDescriptor
                        {
                            Aggregate = aggregate,
                            Version = aggregate.Version
                        });
                else if (_trackedAggregates[aggregate.Id].Aggregate != aggregate)
                    throw new ConcurrencyException(aggregate.Id);
            }
        }

        public T Get<T>(Guid id, int? expectedVersion = null) where T : AggregateRoot
        {
            lock (_trackedAggregates)
            {
                if (IsTracked(id))
                {
                    var trackedAggregate = (T)_trackedAggregates[id].Aggregate;
                    if (expectedVersion != null && trackedAggregate.Version != expectedVersion)
                        throw new ConcurrencyException(trackedAggregate.Id);
                    return trackedAggregate;
                }

                var aggregate = _repository.Get<T>(id);
                if (expectedVersion != null && aggregate.Version != expectedVersion)
                    throw new ConcurrencyException(id);
                Add(aggregate);

                return aggregate;
            }
        }

        private bool IsTracked(Guid id)
        {
            return _trackedAggregates.ContainsKey(id);
        }

        public void Commit(Guid commandId)
        {
            lock (_trackedAggregates)
            {
                foreach (var descriptor in _trackedAggregates.Values)
                {
                    _repository.Save(descriptor.Aggregate, descriptor.Version, commandId);
                }
                _trackedAggregates.Clear();
            }
        }

        private class AggregateDescriptor
        {
            public AggregateRoot Aggregate { get; set; }
            public int Version { get; set; }
        }
    }
}
