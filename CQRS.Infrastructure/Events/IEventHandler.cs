using CQRS.Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Events
{
    public interface IEventHandler<T> : IHandler<T> where T : IEvent
    {
    }
}
