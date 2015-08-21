using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Views
{
    public class NetworkDeviceDetails
    {
        public Guid Id { get; set; }
        public string Hostname { get; set; }
        public int Version { get; set; }
    }
}
