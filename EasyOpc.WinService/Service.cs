using EasyOpc.Common.Constants;
using Microsoft.Owin.Hosting;
using System;
using System.ServiceProcess;

namespace EasyOpc.WinService
{
    partial class Service : ServiceBase
    {
        private IDisposable _webapp;

        public Service()
        {
            InitializeComponent();
        }

        public void Start()
        {
            OnStart(null);
        }

        public void Stop()
        {
            OnStop();
        }

        protected override void OnStart(string[] args)
        {
            _webapp = WebApp.Start<Startup>(WellKnownCodes.ServiceUrl);
        }

        protected override void OnStop()
        {
            _webapp?.Dispose();
        }
    }
}
