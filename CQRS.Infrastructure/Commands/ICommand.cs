using CQRS.Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Commands
{
    public interface ICommand : IMessage
    {
        Guid Id { get; set; }
        Guid CommandId { get; set; }
        Guid ClientId { get; set; }
        //int ExpectedVersion { get; set; }
    }
}
