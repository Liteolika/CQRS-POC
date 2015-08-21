using CQRS.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Contracts
{
    public class CreateNetworkDevice : CommandBase
    {
        public string Hostname { get; set; }

        public CreateNetworkDevice(Guid id, string hostname) 
            : base(id)
            //: base(id, 0)
        {
            Hostname = hostname;
        }

    }
}
