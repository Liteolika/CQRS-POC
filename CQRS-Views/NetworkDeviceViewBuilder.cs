using CQRS.Contracts;
using CQRS.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Views
{
    public class NetworkDeviceViewBuilder : 
        IEventHandler<NetworkDeviceCreated>,
        IEventHandler<NetworkDeviceHostnameChanged>
    {

        
        public NetworkDeviceViewBuilder()
        {
            
        }


        public void Handle(NetworkDeviceCreated message)
        {
            using(var db = new DataBase())
            {
                db.NetworkDevices.Add(new NetworkDeviceDetails()
                {
                    Id = message.Id,
                    Hostname = message.Hostname,
                    Version = message.Version
                });
                db.SaveChanges();
            }
            

        }

        public void Handle(NetworkDeviceHostnameChanged message)
        {
            using (var db = new DataBase())
            {
                var device = db.NetworkDevices.Where(x => x.Id == message.Id).FirstOrDefault();
                device.Hostname = message.NewHostname;
                device.Version = message.Version;
                db.SaveChanges();
            }
        }

        public IEnumerable<NetworkDeviceDetails> GetDevices()
        {
            using (var db = new DataBase())
            {
                return db.NetworkDevices.ToList();
            }
        }

        public NetworkDeviceDetails GetDevices(Guid id)
        {
            using (var db = new DataBase())
            {
                return db.NetworkDevices.FirstOrDefault(x => x.Id == id);
            }
        }
    }

    public class DataBase : DbContext
    {

        public DataBase() : base("ReadStore")
        {

        }

        public DbSet<NetworkDeviceDetails> NetworkDevices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }

}
