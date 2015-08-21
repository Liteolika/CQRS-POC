using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Messages
{
    public interface IHandler<T> where T : IMessage
    {
        void Handle(T message);
    }
}
