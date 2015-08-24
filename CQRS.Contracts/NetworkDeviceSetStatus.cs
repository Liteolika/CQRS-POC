using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Contracts
{
    public class NetworkDeviceSetStatus : CommandBase
    {
        public bool IsOnline { get; set; }

        public NetworkDeviceSetStatus(Guid id, bool isOnline) 
            : base(id)
            //: base(id, 0)
        {
            IsOnline = isOnline;
        }
    }
}
