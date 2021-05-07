using EasyOpc.Common.Opc;
using EasyOpc.Contracts.Opc;
using EasyOpc.WinService.Modules.Opc.Connectors.Contract;
using EasyOpc.WinService.Modules.Opc.Connectors.Da;
using EasyOpc.WinService.Modules.Opc.Connectors.Ua;
using EasyOpc.WinService.Modules.Opc.Service.Contract;
using EasyOpc.WinService.Modules.Opc.Service.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using OpcItem = EasyOpc.WinService.Modules.Opc.Connectors.OpcItem;

namespace EasyOpc.WinService.Hubs
{
    public class OpcGroupHubConnection : IDisposable
    {
        private OpcGroupHub OpcGroupHub { get; }

        private IOpcGroup OpcGroup { get; }

        private int PingDelay { get; } = 5000;

        private int PingCounter { get; set; }

        private CancellationTokenSource CancellationTokenSource { get; }

        public string ConnectionId { get; }

        public Guid OpcGroupId { get; }

        public OpcGroupHubConnection(string connectionId, Guid opcGroupId, OpcGroupHub opcGroupHub)
        {
            ConnectionId = connectionId;
            OpcGroupId = opcGroupId;
            OpcGroupHub = opcGroupHub;

            CancellationTokenSource = new CancellationTokenSource();

            var opcServerService = (IOpcServerService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOpcServerService));
            var opcGroupService = (IOpcGroupService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOpcGroupService));
            var opcItemService = (IOpcItemService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOpcItemService));

            var groupData = opcGroupService.GetByIdAsync(opcGroupId).GetAwaiter().GetResult();
            var serverData = opcServerService.GetByIdAsync(groupData.OpcServerId).GetAwaiter().GetResult();
            var itemDatas = opcItemService.GetByOpcGroupIdAsync(opcGroupId).GetAwaiter().GetResult();

            IOpcServer opcServer;
            if (serverData.Type == OpcServerType.DA)
            {
                var opcDaServerFactory = (OpcDaServerFactory)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(OpcDaServerFactory));
                opcServer = opcDaServerFactory.Create(serverData.Id, serverData.Name, serverData.Host, serverData.JsonSettings);
            }
            else
            {
                var settings = string.IsNullOrEmpty(serverData.JsonSettings) ? null : JsonConvert.DeserializeObject<OpcUaServerSettings>(serverData.JsonSettings);
                var opcUaServerFactory = (OpcUaServerFactory)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(OpcUaServerFactory));
                opcServer = opcUaServerFactory.Create(serverData.Id, serverData.Name, serverData.Host, settings?.UserIdentitySetting?.User, settings?.UserIdentitySetting?.Password);
            }

            OpcGroup = opcServer.CreateOpcGroupAsync(groupData.Id, groupData.Name, itemDatas.Select(p => new OpcItem
            {
                Id = p.Id,
                Name = p.Name,
                AccessPath = p.AccessPath,
                CanonicalDataType = p.CanonicalDataType,
                CanonicalDataTypeId = p.CanonicalDataTypeId,
                ReqDataType = p.ReqDataType,
                ReadOnly = p.ReadOnly,
            })).GetAwaiter().GetResult();

            if (OpcGroup != null)
            {
                opcGroupHub.OnConnected(connectionId, opcGroupId, null);

                OnOpcItemsChanged(OpcGroup.GetOpcItems().Values);

                OpcGroup.OpcItemsChanged += OnOpcItemsChanged;
                var token = CancellationTokenSource.Token;
                Task.Run(async () =>
                {
                    while (true)
                    {
                        if (token.IsCancellationRequested)
                        {
                            Debug.WriteLine($"[{nameof(OpcGroupHub)}][Ping] Stop by IsCancellationRequested");
                            return;
                        }

                        if (PingCounter > 5)
                        {
                            Debug.WriteLine($"[{nameof(OpcGroupHub)}][Ping] Stop by Counter");
                            OpcGroup.OpcItemsChanged -= OnOpcItemsChanged;
                            return;
                        }

                        await Task.Delay(PingDelay, token);

                        Debug.WriteLine($"[{nameof(OpcGroupHub)}][Ping] ConnectionId: {connectionId}");
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

        private void OnOpcItemsChanged(IEnumerable<IOpcItem> items)
        {
            OpcGroupHub.OnOpcItemsChanged(ConnectionId, OpcGroupId, items.Select(p => new OpcItemData
            {
                Id = p.Id,
                Name = p.Name,
                AccessPath = p.AccessPath,
                CanonicalDataType = p.CanonicalDataType,
                CanonicalDataTypeId = p.CanonicalDataTypeId,
                ReqDataType = p.ReqDataType,
                ReadOnly = p.ReadOnly,
                OpcGroupId = OpcGroupId,
                Quality = p.Quality,
                Value = p.Value,
                TimestampLocal = p.TimestampLocal,
                TimestampUtc = p.TimestampUtc
            }));
        }

        public void Pong()
        {
            Debug.WriteLine($"[{nameof(OpcGroupHubConnection)}][{nameof(Pong)}] ConnectionId: {ConnectionId}");
            PingCounter--;
        }

        public void Dispose()
        {
            try
            {
                OpcGroup.OpcItemsChanged -= OnOpcItemsChanged;
            }
            catch { }
            try
            {
                CancellationTokenSource.Cancel();
            }
            catch { }

            OpcGroupHub.OnDisconnected(ConnectionId, OpcGroupId, null);
        }
    }
}
