// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace CQRS.AppWeb.DependencyResolution {
    using Services;
    using CQRS_Views;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using Infrastructure.Commands;
    using Infrastructure.Bus;
    using MassTransit;
    using StructureMap;
    using System;
    using Controllers;

    public class DefaultRegistry : Registry {
        #region Constructors and Destructors

        public DefaultRegistry() {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
            For<NetworkDeviceViewBuilder>().Use<NetworkDeviceViewBuilder>();
            For<NetworkDeviceService>().Use<NetworkDeviceService>();
            For<ICommandPublisher>().Use<CommandPublisher>();
            For<IServiceBus>().Singleton().Use("Creating servicebus", CreateBus);
            For<MessageNotifier>().Singleton().Use<MessageNotifier>();
         }

        private static IServiceBus CreateBus(IContext context)
        {
            var bus = ServiceBusFactory.New(cfg =>
            {
                cfg.DisablePerformanceCounters();
                cfg.ReceiveFrom("rabbitmq://localhost/cqrs-poc");
                cfg.UseRabbitMq(cf =>
                {
                    cf.ConfigureHost(new Uri("rabbitmq://localhost/cqrs-poc"), hc =>
                    {
                        hc.SetUsername("petcar");
                        hc.SetPassword("?!Krone2009");
                    });
                });
            });
            return bus;
        }

        #endregion
    }
}