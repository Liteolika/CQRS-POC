using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using MassTransit;
using MassTransit.BusConfigurators;
using StructureMap;
using CQRS.AppWeb.DependencyResolution;
using System.Diagnostics;
using CQRS.Infrastructure.Notifications;
using CQRS.AppWeb.Controllers;
using System.Threading;

[assembly: OwinStartup(typeof(CQRS.AppWeb.Startup))]
namespace CQRS.AppWeb
{

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            IContainer container = IoC.Initialize();
            container.AssertConfigurationIsValid();
            Debug.WriteLine(container.WhatDoIHave());

            //http://localhost:62963/api/device
            //http://dotnetcodr.com/2015/07/16/building-a-web-api-2-project-from-scratch-using-owinkatana-net-part-5-adding-an-ioc/

            HttpConfiguration config = new HttpConfiguration();
            config.DependencyResolver = new StructureMapWebApiDependencyResolver(container);

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(config);
            app.MapSignalR();

            Thread.Sleep(5000);

            var bus = container.GetInstance<IServiceBus>();
            var notifier = container.GetInstance<MessageNotifier>();
            bus.SubscribeHandler<HandlerNotification>(notifier.Handle);
            
        }
     

    }

    public class BusInitializer
    {
        public static IServiceBus CreateBus(string queueName, Action<ServiceBusConfigurator> moreInitialization)
        {
            var bus = ServiceBusFactory.New(x =>
            {
                x.UseRabbitMq(cf =>
                {
                    cf.ConfigureHost(new Uri("rabbitmq://140.150.92.206/cqrs-poc"), hc =>
                    {
                        hc.SetUsername("petcar");
                        hc.SetPassword("?!Krone2009");
                    });
                });
                x.ReceiveFrom("rabbitmq://140.150.92.206/cqrs-poc");
            });

            return bus;
        }
    }



}