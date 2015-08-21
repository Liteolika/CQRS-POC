using CQRS.Contracts;
using CQRS.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Domain
{
    public class NetworkDevice : AggregateRoot
    {

        private string hostname;

        private void Apply(NetworkDeviceCreated e)
        {
            //Version = e.Version;
            hostname = e.Hostname;
        }

        private void Apply(NetworkDeviceHostnameChanged e)
        {
            //Version = e.Version;
            hostname = e.NewHostname;
        }


        private NetworkDevice() 
        {
            RegisterTransition<NetworkDeviceCreated>(Apply);
            RegisterTransition<NetworkDeviceHostnameChanged>(Apply);
        
        }

        public NetworkDevice(Guid id, string hostname)
        {
            Id = id;
            ApplyChange(new NetworkDeviceCreated(id, Version, hostname));
        }

        public void SetHostname(string newHostname)
        {
            if (string.IsNullOrEmpty(newHostname))
                throw new ArgumentException("newHostname");
            ApplyChange(new NetworkDeviceHostnameChanged(Id, Version, newHostname));
        }
    }
}
