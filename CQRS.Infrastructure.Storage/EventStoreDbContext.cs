using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Storage
{
    public class EventStoreDbContext : DbContext
    {

        public EventStoreDbContext()
            : base("EventStore")
        {

        }

        public DbSet<AggregateItemDescriptor> Aggregates { get; set; }
        public DbSet<EventDescriptor> Events { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public static EventStoreDbContext Create()
        {
            return new EventStoreDbContext();
        }

    }
}
