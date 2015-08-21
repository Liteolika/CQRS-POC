using CQRS.Contracts;
using CQRS.Infrastructure.Bus;
using CQRS.Infrastructure.Commands;
using CQRS.Services;
using CQRS_Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CQRS.AppWeb.Controllers
{
    [RoutePrefix("api/devices")]
    public class DeviceController : ApiController
    {
        private NetworkDeviceService _service;
        private ICommandPublisher _publisher;
        private NetworkDeviceViewBuilder _deviceViews;

        public DeviceController(NetworkDeviceViewBuilder deviceViews, NetworkDeviceService service, ICommandPublisher publisher)
        {
            _deviceViews = deviceViews;
            _service = service;
            _publisher = publisher;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var views = _deviceViews.GetDevices();
            return Ok(views);
        }

        [HttpPost]
        public IHttpActionResult Post(NetworkDeviceDetails device)
        {
            _service.SetDeviceHostname(device.Id, device.Hostname);
            return Ok("");
        }
        

    }
}
