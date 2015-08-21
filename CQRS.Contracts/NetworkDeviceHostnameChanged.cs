using CQRS.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Contracts
{
    public class NetworkDeviceHostnameChanged : EventBase
    {
        public string NewHostname { get; set; }

        public NetworkDeviceHostnameChanged(Guid id, int version, string newHostname)
            : base(id, version)
        {
            NewHostname = newHostname;
        }

    }
}
