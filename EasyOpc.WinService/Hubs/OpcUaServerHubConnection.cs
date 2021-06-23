using EasyOpc.WinService.Modules.Opc.Ua.Connector.Contract;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyOpc.WinService.Hubs
{
    public class OpcUaServerHubConnection : IDisposable
    {
        private CancellationTokenSource CancellationTokenSource { get; }

        private IOpcUaGroup OpcUaGroup { get; }

        private OpcUaServerHub OpcUaServerHub { get; }

        private int PingDelay { get; } = 5000;

        private int PingCounter { get; set; }

        public string ConnectionId { get; }

        public Guid OpcUaServerId { get; }

        public OpcUaServerHubConnection(string connectionId, Guid opcUaServerId, OpcUaServerHub opcUaServerHub)
        {
            ConnectionId = connectionId;
            OpcUaServerId = opcUaServerId;
            OpcUaServerHub = opcUaServerHub;

            CancellationTokenSource = new CancellationTokenSource();

            var opcServerService = (IOpcUaServersService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOpcUaServersService));
            var serverData = opcServerService.GetByIdAsync(OpcUaServerId).GetAwaiter().GetResult();

            var opcUaServerFactory = (IOpcUaServersFactory)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOpcUaServersFactory));
            var opcUaServer = opcUaServerFactory.Create(serverData.Id, serverData.Name, serverData.Host, serverData.UserName, serverData.Password);

            var opcUaGroupId = Guid.NewGuid();
            OpcUaGroup = opcUaServer.CreateOpcUaGroupAsync(opcUaGroupId, $"Group-{opcUaGroupId}", new List<IOpcUaItem>()).GetAwaiter().GetResult();

            if (OpcUaGroup != null)
            {
                opcUaServerHub.OnConnected(connectionId, opcUaGroupId, null);

                OpcUaGroup.OpcUaItemsChanged += OnOpcItemsChanged;

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

                        opcUaServerHub.Ping(connectionId);

                        PingCounter++;
                    }

                    try
                    {
                        OpcUaGroup.OpcUaItemsChanged -= OnOpcItemsChanged;
                    }
                    catch { }
                });
            }
            else
            {
                opcUaServerHub.OnConnected(connectionId, opcUaGroupId, "An error occurred while connecting to the OPC.UA server");
            }
        }

        private void OnOpcItemsChanged(IEnumerable<IOpcUaItem> items)
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
                OpcUaGroup.OpcUaItemsChanged -= OnOpcItemsChanged;
            }
            catch { }

            try
            {
                CancellationTokenSource?.Cancel();
            }
            catch { }

            OpcUaServerHub.OnDisconnected(ConnectionId, OpcUaServerId, null);
        }
    }
}
