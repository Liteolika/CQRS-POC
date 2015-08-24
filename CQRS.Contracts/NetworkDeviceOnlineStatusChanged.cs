using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Contracts
{
    public class NetworkDeviceOnlineStatusChanged : EventBase
    {

        public bool IsOnline { get; set; }

        public NetworkDeviceOnlineStatusChanged(Guid id, int version, bool isOnline)
            : base(id, version)
        {
            IsOnline = isOnline;
        }

    }
}
