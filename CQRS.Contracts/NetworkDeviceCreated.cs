using CQRS.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Contracts
{
    public class NetworkDeviceCreated : EventBase
    {
        public string Hostname { get; set; }

        public NetworkDeviceCreated(Guid id, int version, string hostname) : base(id, version)
        {
            Hostname = hostname;
        }

    }
}
