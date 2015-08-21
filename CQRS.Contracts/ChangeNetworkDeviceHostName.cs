using CQRS.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Contracts
{
    public class ChangeNetworkDeviceHostName : CommandBase
    {

        public string NewHostname { get; set; }

        //public ChangeNetworkDeviceHostName(Guid id, int expectedVersion, string newHostname) 
        //    : base(id, expectedVersion)
        public ChangeNetworkDeviceHostName(Guid id, string newHostname)
            : base(id)
        {
            NewHostname = newHostname;
        }

    }
}
