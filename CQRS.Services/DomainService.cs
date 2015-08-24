using CQRS.Contracts;
using CQRS.Domain;
using CQRS.Infrastructure.Bus;
using CQRS.Infrastructure.Domain;
using CQRS.Infrastructure.Events;
using CQRS.Infrastructure.Notifications;
using CQRS.Infrastructure.Storage;
using CQRS_Views;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Services
{
    public class DomainService : IService
    {

        private readonly IServiceBus _bus;
        private IEventStore _eventStore;
        private IEventPublisher _eventPublisher;
        private IRepository _repository;
        private ISession _session;
        private INotificationPublisher _notificationPublisher;
        private NetworkDeviceCommandHandler _ndch;

        private NetworkDeviceViewBuilder _ndvb;

        private List<UnsubscribeAction> unsubscribeActions;

        public DomainService(IServiceBus bus)
        {
            _bus = bus;
            unsubscribeActions = new List<UnsubscribeAction>();
        }

        public void Start()
        {

            _eventStore = new SqlEventStore(() => EventStoreDbContext.Create());

            //_eventStore = new EventStore();

            _eventPublisher = new EventPublisher(_bus);
            _notificationPublisher = new NotificationPublisher(_bus);

            _repository = new MyRepository(_eventStore, _eventPublisher);
            _session = new MySession(_repository);

            //_repository = new Repository(_eventStore, _eventPublisher);
            //_session = new Session(_repository);

            _ndch = new NetworkDeviceCommandHandler(_session, _notificationPublisher);
            _ndvb = new NetworkDeviceViewBuilder();

            _bus.SubscribeHandler<CreateNetworkDevice>(_ndch.Handle);
            _bus.SubscribeHandler<ChangeNetworkDeviceHostName>(_ndch.Handle);
            _bus.SubscribeHandler<NetworkDeviceSetStatus>(_ndch.Handle);

            _bus.SubscribeHandler<NetworkDeviceCreated>(_ndvb.Handle);
            _bus.SubscribeHandler<NetworkDeviceHostnameChanged>(_ndvb.Handle);
            _bus.SubscribeHandler<NetworkDeviceOnlineStatusChanged>(_ndvb.Handle);
        }

        public void Stop()
        {
            foreach (var a in unsubscribeActions)
            {
                a.Invoke();
            }
        }
    }
}
