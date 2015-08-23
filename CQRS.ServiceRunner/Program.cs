using CQRS.Services;
using MassTransit;
using System;
using Topshelf;

namespace CQRS.ServiceRunner
{
    class Program
    {
        static void Main(string[] args)
        {

            IServiceBus serviceBus = ServiceBusFactory.New(cfg =>
            {
                cfg.ReceiveFrom("rabbitmq://localhost/cqrs-poc");
                cfg.UseRabbitMq(r =>
                {
                    r.ConfigureHost(new Uri("rabbitmq://localhost/cqrs-poc"), hc =>
                    {
                        hc.Validate();
                        hc.SetUsername("petcar");
                        hc.SetPassword("?!Krone2009");
                    });
                });
                
            });

            HostFactory.Run(x =>
            {

                x.Service<IService>(c =>
                {
                    c.ConstructUsing(() => new DomainService(serviceBus));
                    c.WhenStarted(s => s.Start());
                    c.WhenStopped(s => s.Stop());
                });

                x.RunAsLocalSystem();
                x.SetServiceName("ReSTore.Orders");
                x.SetDisplayName("ReSTore Orders service");
                x.SetDescription("Main service for the ReSTore order domain");
            });

        }
    }

    

    //public class DomainService : IService
    //{

    //    private readonly IServiceBus _bus;
    //    private IEventStore _eventStore;
    //    private IEventPublisher _eventPublisher;
    //    private IRepository _repository;
    //    private ISession _session;
    //    private INotificationPublisher _notificationPublisher;
    //    private NetworkDeviceCommandHandler _ndch;

    //    private NetworkDeviceViewBuilder _ndvb;

    //    private List<UnsubscribeAction> unsubscribeActions;

    //    public DomainService(IServiceBus bus)
    //    {
    //        _bus = bus;
    //        unsubscribeActions = new List<UnsubscribeAction>();
    //    }

    //    public void Start()
    //    {

    //        _eventStore = new SqlEventStore(() => EventStoreDbContext.Create());

    //        //_eventStore = new EventStore();

    //        _eventPublisher = new EventPublisher(_bus);
    //        _notificationPublisher = new NotificationPublisher(_bus);

    //        _repository = new MyRepository(_eventStore, _eventPublisher);
    //        _session = new MySession(_repository);

    //        //_repository = new Repository(_eventStore, _eventPublisher);
    //        //_session = new Session(_repository);
            
    //        _ndch = new NetworkDeviceCommandHandler(_session, _notificationPublisher);
    //        _ndvb = new NetworkDeviceViewBuilder();

    //        _bus.SubscribeHandler<CreateNetworkDevice>(_ndch.Handle);
    //        _bus.SubscribeHandler<ChangeNetworkDeviceHostName>(_ndch.Handle);

    //        _bus.SubscribeHandler<NetworkDeviceCreated>(_ndvb.Handle);
    //        _bus.SubscribeHandler<NetworkDeviceHostnameChanged>(_ndvb.Handle);
    //    }

    //    public void Stop()
    //    {
    //        foreach (var a in unsubscribeActions)
    //        {
    //            a.Invoke();
    //        }
    //    }
    //}

    //public interface IService
    //{
    //    void Start();
    //    void Stop();
    //}


}
