using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Domain.Exceptions
{
    public class AggregateOrEventMissingIdException : System.Exception
    {
        public AggregateOrEventMissingIdException(Type aggregateType, Type eventType)
            : base(string.Format("An event of type {0} was tried to save from {1} but no id where set on either", eventType.FullName, aggregateType.FullName))
        {

        }
    }
}
