using EasyOpc.Common.Constants;
using EasyOpc.Contracts.Opc.Da;
using EasyOpc.WinService.Modules.Opc.Da.Connector;
using EasyOpc.WinService.Modules.Opc.Da.Connector.Contract;
using EasyOpc.WinService.Modules.Opc.Da.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyOpc.WinService.Hubs
{
    public class OpcDaGroupHubConnection : IDisposable
    {
        private OpcDaGroupHub OpcDaGroupHub { get; }

        private IOpcDaGroup OpcDaGroup { get; }

        private int PingDelay { get; } = 5000;

        private int PingCounter { get; set; }

        private CancellationTokenSource CancellationTokenSource { get; }

        public string ConnectionId { get; }

        public Guid OpcDaGroupId { get; }

        public HashSet<Guid> OpcDaItemIds { get; set; }

        public OpcDaGroupHubConnection(string connectionId, Guid opcGroupId, OpcDaGroupHub opcGroupHub)
        {
            ConnectionId = connectionId;
            OpcDaGroupId = opcGroupId;
            OpcDaGroupHub = opcGroupHub;
            OpcDaItemIds = new HashSet<Guid>();

            CancellationTokenSource = new CancellationTokenSource();

            var opcServerService = (IOpcDaServersService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOpcDaServersService));
            var opcGroupService = (IOpcDaGroupsService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOpcDaGroupsService));
            var opcItemService = (IOpcDaItemsService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOpcDaItemsService));

            var groupData = opcGroupService.GetByIdAsync(opcGroupId).GetAwaiter().GetResult();
            var serverData = opcServerService.GetByIdAsync(groupData.OpcDaServerId).GetAwaiter().GetResult();
            var itemDatas = opcItemService.GetByOpcDaGroupIdAsync(opcGroupId).GetAwaiter().GetResult();

            var opcDaServerFactory = (IOpcDaServersFactory)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOpcDaServersFactory));
            var opcDaServer = opcDaServerFactory.Create(serverData.Id, serverData.Name, serverData.Host, serverData.Clsid);

            OpcDaGroup = opcDaServer.CreateOpcDaGroupAsync(groupData.Id, groupData.Name, itemDatas.Select(p => new OpcDaItem
            {
                Id = p.Id,
                Name = p.Name,
                AccessPath = p.AccessPath,
                CanonicalDataType = p.CanonicalDataType,
                CanonicalDataTypeId = p.CanonicalDataTypeId,
                ReqDataType = p.ReqDataType,
                ReadOnly = p.ReadOnly,
            })).GetAwaiter().GetResult();

            if (OpcDaGroup != null)
            {
                opcGroupHub.OnConnected(connectionId, opcGroupId, null);
                //OnOpcItemsChanged(OpcGroup.GetOpcItems().Values);

                OpcDaGroup.OpcDaItemsChanged += OnOpcItemsChanged;
                var token = CancellationTokenSource.Token;
                Task.Run(async () =>
                {
                    while (true)
                    {
                        if (token.IsCancellationRequested)
                        {
                            Debug.WriteLine($"[{nameof(OpcDaGroupHub)}][Ping] Stop by IsCancellationRequested");
                            return;
                        }

                        if (PingCounter > 5)
                        {
                            Debug.WriteLine($"[{nameof(OpcDaGroupHub)}][Ping] Stop by Counter");
                            OpcDaGroup.OpcDaItemsChanged -= OnOpcItemsChanged;
                            return;
                        }

                        await Task.Delay(PingDelay, token);

                        Debug.WriteLine($"[{nameof(OpcDaGroupHub)}][Ping] ConnectionId: {connectionId}");
                        opcGroupHub.Ping(connectionId);
                        PingCounter++;

                    }
                });
            }
            else
            {
                opcGroupHub.OnConnected(connectionId, opcGroupId, "An error occurred while connecting to the OPC group");
            }
        }

        private void OnOpcItemsChanged(IEnumerable<IOpcDaItem> items)
        {
            if (!OpcDaItemIds.Any())
                return;

            OpcDaGroupHub.OnOpcDaItemsChanged(ConnectionId, OpcDaGroupId, items.Where(p => OpcDaItemIds.Contains(p.Id)).Select(p => new OpcDaItemData
            {
                Id = p.Id,
                Name = p.Name,
                AccessPath = p.AccessPath,
                CanonicalDataType = p.CanonicalDataType,
                CanonicalDataTypeId = p.CanonicalDataTypeId,
                ReqDataType = p.ReqDataType,
                ReadOnly = p.ReadOnly,
                OpcDaGroupId = OpcDaGroupId,
                Quality = p.Quality,
                Value = p.Value,
                TimestampLocal = p.Timestamp?.ToLocalTime().ToString(WellKnownCodes.ExportDateTimeFormat),
                TimestampUtc = p.Timestamp?.ToUniversalTime().ToString(WellKnownCodes.ExportDateTimeFormat)
            }));
        }

        public void Pong()
        {
            Debug.WriteLine($"[{nameof(OpcDaGroupHubConnection)}][{nameof(Pong)}] ConnectionId: {ConnectionId}");
            PingCounter--;
        }

        public void Subscribe(Guid[] opcDaItemIds)
        {
            OpcDaItemIds = new HashSet<Guid>(opcDaItemIds);
            OnOpcItemsChanged(OpcDaGroup.GetOpcDaItems().Values);
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
                CancellationTokenSource.Cancel();
            }
            catch { }

            OpcDaGroupHub.OnDisconnected(ConnectionId, OpcDaGroupId, null);
        }
    }
}
