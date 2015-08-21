using CQRS.Contracts;
using CQRS.Domain;
using CQRS.Infrastructure.Bus;
using CQRS.Infrastructure.Commands;
using CQRS.Infrastructure.Domain;
using CQRS.Infrastructure.Events;
using CQRS.Infrastructure.Notifications;
using CQRS.Services;
using CQRS_Views;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.App
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Sleeping for 3 seconds..");
            

            IServiceBus serviceBus = ServiceBusFactory.New(cfg =>
            {
                cfg.ReceiveFrom("rabbitmq://140.150.92.206/cqrs-poc");
                cfg.UseRabbitMq(cf =>
                {
                    cf.ConfigureHost(new Uri("rabbitmq://140.150.92.206/cqrs-poc"), hc =>
                    {
                        hc.SetUsername("petcar");
                        hc.SetPassword("?!Krone2009");
                    });
                });
            });

            CommandPublisher commandPublisher = new CommandPublisher(serviceBus);

            NetworkDeviceService service = new NetworkDeviceService(commandPublisher);

            NetworkDeviceViewBuilder ndvb = new NetworkDeviceViewBuilder();

            serviceBus.SubscribeHandler<HandlerNotification>(service.Handle);

            service.ServiceResultRecieved += (sender, result) =>
                {
                    Console.WriteLine(
                        "CommandId: {0}, Result: {1}, Message: {2}, Exception: {3}",
                        result.CommandId != default(Guid) ? result.CommandId : Guid.Empty,
                    result.Success,
                    result.Message, result.ExceptionMessage);
                };

            Thread.Sleep(4000);
            //var id = Guid.NewGuid();
            //service.CreateDevice(id, "SESM-1");
            //Thread.Sleep(3000);
            //service.CreateDevice(id, "SESM-1");

            //for (int i = 0; i < 100; i++)
            //{
            //    Thread.Sleep(500);
            //    var id = Guid.NewGuid();
            //    service.CreateDevice(id, "SESM-" + i.ToString());
            //}
            


            //foreach (var device in ndvb.GetDevices())
            //{
            //    Console.WriteLine("{0}\t{1}\t{2}", device.Id, device.Hostname, device.Version);
            //}

            //Console.ReadKey();

            //var id = Guid.NewGuid();

            //service.CreateDevice(id, "SESM-1");
            //Thread.Sleep(2000);
            ////service.CreateDevice(id, "SESM-1");

            //foreach (var device in ndvb.GetDevices())
            //{
            //    Console.WriteLine("{0}\t{1}\t{2}", device.Id, device.Hostname, device.Version);
            //}

            ////Thread.Sleep(2000);

            //Random rnd = new Random();

            ////for (int i = 0; i < 2000; i++)
            ////{
            ////    Thread.Sleep(rnd.Next(1,20));
            ////    service.SetDeviceHostname(id, "SESM-" + i.ToString());
            ////}

            //for (int i = 0; i < 100; i++)
            //{
            //    Thread.Sleep(rnd.Next(1, 200));
            //    service.SetDeviceHostname(id, "SESM-" + i.ToString());
            //}

            //foreach (var device in ndvb.GetDevices())
            //{
            //    Console.WriteLine("{0}\t{1}\t{2}", device.Id, device.Hostname, device.Version);
            //}
            

            ////service.CreateDevice(id, "SESM-1");

            Console.ReadKey();

        }

        




    }



    

    

}
