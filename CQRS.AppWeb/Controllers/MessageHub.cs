using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CQRS.Infrastructure.Events;
using CQRS.Infrastructure.Notifications;

namespace CQRS.AppWeb.Controllers
{
    public class MessageHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }

    public class MessageNotifier : INotificationHandler<HandlerNotification>
    {
        public void Handle(HandlerNotification message)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
            hub.Clients.All.messageRecieved(message.Message);
        }
    }

}