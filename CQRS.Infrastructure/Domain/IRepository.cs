using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Domain
{
    public interface IRepository
    {
        //void Save<T>(T aggregate, int? expectedVersion = null) where T : AggregateRoot;
        void Save<T>(T aggregate, int? expectedVersion = null, Guid? commandId = null) where T : AggregateRoot;
        T Get<T>(Guid aggregateId) where T : AggregateRoot;
        bool HasAggregate<T>(Guid aggregateId) where T : AggregateRoot;
    }
}
