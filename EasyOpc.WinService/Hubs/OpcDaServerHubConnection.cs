using EasyOpc.WinService.Modules.Opc.Da.Connector.Contract;
using EasyOpc.WinService.Modules.Opc.Da.Services.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyOpc.WinService.Hubs
{
    public class OpcDaServerHubConnection : IDisposable
    {
        private CancellationTokenSource CancellationTokenSource { get; }

        private IOpcDaGroup OpcDaGroup { get; }

        private OpcDaServerHub OpcDaServerHub { get; }

        private int PingDelay { get; } = 5000;

        private int PingCounter { get; set; }

        public string ConnectionId { get; }

        public Guid OpcDaServerId { get; }

        public OpcDaServerHubConnection(string connectionId, Guid opcDaServerId, OpcDaServerHub opcDaServerHub)
        {
            ConnectionId = connectionId;
            OpcDaServerId = opcDaServerId;
            OpcDaServerHub = opcDaServerHub;

            CancellationTokenSource = new CancellationTokenSource();

            var opcServerService = (IOpcDaServersService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOpcDaServersService));
            var serverData = opcServerService.GetByIdAsync(OpcDaServerId).GetAwaiter().GetResult();

            var opcDaServerFactory = (IOpcDaServersFactory)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOpcDaServersFactory));
            var opcDaServer = opcDaServerFactory.Create(serverData.Id, serverData.Name, serverData.Host, serverData.Clsid);

            var opcGroupId = Guid.NewGuid();
            OpcDaGroup = opcDaServer.CreateOpcDaGroupAsync(opcGroupId, $"Group-{opcGroupId}", new List<IOpcDaItem>()).GetAwaiter().GetResult();

            if (OpcDaGroup != null)
            {
                opcDaServerHub.OnConnected(connectionId, opcGroupId, null);

                OpcDaGroup.OpcDaItemsChanged += OnOpcItemsChanged;

                var token = CancellationTokenSource.Token;
                Task.Run(async () =>
                {
                    while (true)
                    {
                        if (token.IsCancellationRequested || PingCounter > 5)
                        {
                            break;
                        }

                        await Task.Delay(PingDelay, token);

                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        opcDaServerHub.Ping(connectionId);

                        PingCounter++;
                    }

                    try
                    {
                        OpcDaGroup.OpcDaItemsChanged -= OnOpcItemsChanged;
                    }
                    catch { }
                });
            }
            else
            {
                opcDaServerHub.OnConnected(connectionId, opcGroupId, "An error occurred while connecting to the OPC.DA server");
            }
        }

        private void OnOpcItemsChanged(IEnumerable<IOpcDaItem> items)
        {
        }

        public void Pong()
        {
            PingCounter--;
        }

        public void Dispose()
        {
            try
            {
                OpcDaGroup.OpcDaItemsChanged -= OnOpcItemsChanged;
            }
            catch { }

            try
            {
                CancellationTokenSource?.Cancel();
            }
            catch { }

            OpcDaServerHub.OnDisconnected(ConnectionId, OpcDaServerId, null);
        }
    }
}
