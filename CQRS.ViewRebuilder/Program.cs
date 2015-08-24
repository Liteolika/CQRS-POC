using CQRS.Infrastructure.Events;
using CQRS.Infrastructure.Storage;
using CQRS_Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.ViewRebuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("View Rebuilder");
            Console.WriteLine("Press R to rebuild all views...");

            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.R)
            {
                RunRebuilder();
            }

        }

        static void RunRebuilder()
        {
            Console.WriteLine("Running rebuilder. Please wait...");
            NetworkDeviceViewBuilder viewBuilder = new NetworkDeviceViewBuilder();

            EventStoreDbContext eventContext = new EventStoreDbContext();

            IEventStore eventStore = new SqlEventStore(() => EventStoreDbContext.Create());
            
            foreach (var aggregate in eventContext.Aggregates)
            {
                List<IEvent> events = new List<IEvent>();
                foreach (var evt in eventStore.Get(aggregate.AggregateId, -1))
                {
                    events.Add(evt);
                }

                foreach (var evt in events)
                {
                    
                }

            }

        }
    }
}
