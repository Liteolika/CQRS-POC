using CQRS.Infrastructure.Commands;
using CQRS.Infrastructure.Notifications;
using CQRS.Services;
using CQRS_Views;
using MassTransit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CQRS.AppWinForms
{
    public partial class Form1 : Form
    {
        public IServiceBus serviceBus;

        public CommandPublisher commandPublisher;

        public NetworkDeviceService service;

        public NetworkDeviceViewBuilder ndvb;

        public Form1()
        {
            //deviceService = new NetworkDeviceService();

            InitializeComponent();

            serviceBus = ServiceBusFactory.New(cfg =>
            {
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

            commandPublisher = new CommandPublisher(serviceBus);
            service = new NetworkDeviceService(commandPublisher);
            ndvb = new NetworkDeviceViewBuilder();

            serviceBus.SubscribeHandler<HandlerNotification>(service.Handle);

            //service.ServiceResultRecieved += (sender, result) =>
            //{
            //    var a = result;
            //};

            service.ServiceResultRecieved += service_ServiceResultRecieved;

            GetDevices();

        }

        void service_ServiceResultRecieved(object sender, ServiceResult result)
        {
            if (InvokeRequired)
                this.Invoke(new Action(() => {

                    if (result.Success == false)
                    {
                        MessageBox.Show(result.Message, "An error occured");
                    }
                    GetDevices();

                }));

            
        }

        private void GetDevices()
        {
            Thread.Sleep(500);
            lstDevices.DataSource = ndvb.GetDevices();
            
        }

        private void GetDeviceFromSelection()
        {
            for (int i = 0; i < lstDevices.Rows.Count; i++)
            {
                if (lstDevices.Rows[i].Selected)
                {
                    var deviceId = Guid.Parse(lstDevices.Rows[i].Cells[0].Value.ToString());
                    GetDevice(deviceId);
                }
            }
        }

        private void GetDevice(Guid id)
        {
            var device = ndvb.GetDevices(id);
            currentDevice = device.Id;
            txtDeviceName.Text = device.Hostname;
        }
        private Guid currentDevice;

        private void lstDevices_SelectionChanged(object sender, EventArgs e)
        {
            GetDeviceFromSelection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            service.SetDeviceHostname(currentDevice, txtDeviceName.Text);
            GetDevices();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            service.CreateDevice(Guid.NewGuid(), txtDeviceName.Text);
            GetDevices();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetDevices();
        }
    }

    public class CommandPublisher : ICommandPublisher
    {

        private readonly IServiceBus _bus;

        public CommandPublisher(IServiceBus bus)
        {
            _bus = bus;
        }

        public void Publish<T>(T command) where T : ICommand
        {
            _bus.Publish(command, command.GetType());
        }
    }

}
