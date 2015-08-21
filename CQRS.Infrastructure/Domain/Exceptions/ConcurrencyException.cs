using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Domain.Exceptions
{
    public class ConcurrencyException : System.Exception
    {
        public ConcurrencyException(Guid id)
            : base(string.Format("A different version than expected was found in aggregate {0}", id))
        {
        }
    }
}
