using EasyOpc.Common.Constants;
using EasyOpc.Contracts.Opc.Ua;
using EasyOpc.WinService.Modules.Opc.Ua.Connector;
using EasyOpc.WinService.Modules.Opc.Ua.Connector.Contract;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyOpc.WinService.Hubs
{
    public class OpcUaGroupHubConnection : IDisposable
    {
        private OpcUaGroupHub OpcUaGroupHub { get; }

        private IOpcUaGroup OpcUaGroup { get; }

        private int PingDelay { get; } = 5000;

        private int PingCounter { get; set; }

        private CancellationTokenSource CancellationTokenSource { get; }

        public string ConnectionId { get; }

        public Guid OpcUaGroupId { get; }

        public HashSet<Guid> OpcUaItemIds { get; set; }

        public OpcUaGroupHubConnection(string connectionId, Guid opcUaGroupId, OpcUaGroupHub opcUaGroupHub)
        {
            ConnectionId = connectionId;
            OpcUaGroupId = opcUaGroupId;
            OpcUaGroupHub = opcUaGroupHub;
            OpcUaItemIds = new HashSet<Guid>();

            CancellationTokenSource = new CancellationTokenSource();

            var opcServerService = (IOpcUaServersService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOpcUaServersService));
            var opcGroupService = (IOpcUaGroupsService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOpcUaGroupsService));
            var opcItemService = (IOpcUaItemsService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOpcUaItemsService));

            var groupData = opcGroupService.GetByIdAsync(opcUaGroupId).GetAwaiter().GetResult();
            var serverData = opcServerService.GetByIdAsync(groupData.OpcUaServerId).GetAwaiter().GetResult();
            var itemDatas = opcItemService.GetByOpcUaGroupIdAsync(opcUaGroupId).GetAwaiter().GetResult();

            var opcUaServerFactory = (IOpcUaServersFactory)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOpcUaServersFactory));
            var opcServer = opcUaServerFactory.Create(serverData.Id, serverData.Name, serverData.Host, serverData.UserName, serverData.Password);

            OpcUaGroup = opcServer.CreateOpcUaGroupAsync(groupData.Id, groupData.Name, itemDatas.Select(p => new OpcUaItem
            {
                Id = p.Id,
                Name = p.Name,
                NodeId = p.NodeId,
            })).GetAwaiter().GetResult();

            if (OpcUaGroup != null)
            {
                opcUaGroupHub.OnConnected(connectionId, opcUaGroupId, null);
                //OnOpcItemsChanged(OpcGroup.GetOpcItems().Values);

                OpcUaGroup.OpcUaItemsChanged += OnOpcItemsChanged;
                var token = CancellationTokenSource.Token;
                Task.Run(async () =>
                {
                    while (true)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }

                        if (PingCounter > 5)
                        {
                            OpcUaGroup.OpcUaItemsChanged -= OnOpcItemsChanged;
                            return;
                        }

                        await Task.Delay(PingDelay, token);

                        opcUaGroupHub.Ping(connectionId);
                        PingCounter++;

                    }
                });
            }
            else
            {
                opcUaGroupHub.OnConnected(connectionId, opcUaGroupId, "An error occurred while connecting to the OPC group");
            }
        }

        private void OnOpcItemsChanged(IEnumerable<IOpcUaItem> items)
        {
            if (!OpcUaItemIds.Any())
                return;

            OpcUaGroupHub.OnOpcItemsChanged(ConnectionId, OpcUaGroupId, items.Where(p => OpcUaItemIds.Contains(p.Id)).Select(p => new OpcUaItemData
            {
                Id = p.Id,
                Name = p.Name,
                NodeId = p.NodeId,
                OpcUaGroupId = OpcUaGroupId,
                Value = p.Value,
                TimestampLocal = p.Timestamp?.ToLocalTime().ToString(WellKnownCodes.ExportDateTimeFormat),
                TimestampUtc = p.Timestamp?.ToUniversalTime().ToString(WellKnownCodes.ExportDateTimeFormat)
            }));
        }

        public void Pong()
        {
            PingCounter--;
        }

        public void Subscribe(Guid[] opcUaItemIds)
        {
            OpcUaItemIds = new HashSet<Guid>(opcUaItemIds);
            OnOpcItemsChanged(OpcUaGroup.GetOpcUaItems().Values);
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
                CancellationTokenSource.Cancel();
            }
            catch { }

            OpcUaGroupHub.OnDisconnected(ConnectionId, OpcUaGroupId, null);
        }
    }
}
