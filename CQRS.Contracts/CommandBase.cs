using CQRS.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Contracts
{
    public abstract class CommandBase : ICommand
    {
        public Guid Id { get; set; }
        public Guid CommandId { get; set; }
        public Guid ClientId { get; set; }
        //public int ExpectedVersion { get; set; }

        //public CommandBase(Guid id, int expectedVersion)
        public CommandBase(Guid id)
        {
            CommandId = Guid.NewGuid();
            Id = id;
            //ExpectedVersion = expectedVersion;

        }

    }
}
