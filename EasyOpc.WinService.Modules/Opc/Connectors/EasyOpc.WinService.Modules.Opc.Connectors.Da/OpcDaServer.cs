using EasyOpc.Common.Constant;
using EasyOpc.Common.Extension;
using EasyOpc.Common.Opc;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Modules.Opc.Connectors.Contract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using OPC = Opc;
using OPCDA = Opc.Da;


namespace EasyOpc.WinService.Modules.Opc.Connectors.Da
{
    public class OpcDaServer : IOpcServer
    {
        private object LockObject { get; } = new object();

        protected ILogger Logger { get; }

        private OPCDA.Server Server { get; set; }

        private List<IOpcGroup> OpcGroups { get; } = new List<IOpcGroup>();

        public Guid Id { get; }

        public string Name { get; }

        public string Host { get; }

        private string CLSID { get; }

        public OpcServerType Type => OpcServerType.DA;

        public bool IsConnected => Server?.IsConnected ?? false;

        public OpcDaServer(ILogger logger, Guid id, string name, string host, string clsid)
        {
            Logger = logger;
            Id = id;
            Name = name;
            Host = host;
            CLSID = clsid;
        }

        public async Task ConnectAsync()
        {
            Logger.Debug($"[{nameof(OpcDaServer)}: {Name}][Host: {Host}][Type: {Type}][Method: {nameof(ConnectAsync)}]");

            Server = new OPCDA.Server(new OpcCom.Factory(), OpcDaHelper.CreateURL(Host, Name, CLSID));

            await Task.Run(() => 
            { 
                try 
                { 
                    Server.Connect(); 
                } 
                catch(Exception ex)
                {
                    Logger.Error($"[ERROR][ConnectAsync]");
                    Logger.Error(ex);
                    Server = null; 
                } 
            });

            Logger.Debug($"[{nameof(OpcDaServer)}: {Name}][Host: {Host}][Type: {Type}][Method: {nameof(ConnectAsync)}][IsConnected: {IsConnected}]");
        }

        public async Task DisconnectAsync()
        {
            Logger.Debug($"[{nameof(OpcDaServer)}: {Name}][Host: {Host}][Type: {Type}][Method: {nameof(DisconnectAsync)}]");

            await Task.Run(() => 
            { 
                try 
                { 
                    Server?.Disconnect(); 
                } 
                catch { }

                Server = null;
            });

            Logger.Debug($"[{nameof(OpcDaServer)}: {Name}][Host: {Host}][Type: {Type}][Method: {nameof(DisconnectAsync)}][IsConnected: {IsConnected}]");
        }

        public async Task ReconnectAsync()
        {
            await DisconnectAsync();

            await ConnectAsync();

            if(IsConnected)
            {
                foreach (var opcGroup in OpcGroups.ToList())
                {
                    await CreateSubscriptionAsync(Server, opcGroup);
                }
            }
        }

        public async Task<bool> PingAsync()
        {
            var items = await BrowseAsync(null);
            if (items != null && items.Count() > 0)
            {
                return true;
            }

            return false;
        }

        public IEnumerable<IOpcGroup> GetOpcGroups() => OpcGroups;

        public async Task<IOpcGroup> CreateOpcGroupAsync(Guid id, string groupName, IEnumerable<IOpcItem> items)
        {
            if (!IsConnected)
                await ConnectAsync();

            if (!IsConnected)
                return null;

            IOpcGroup opcGroup = null;

            try
            {
                lock (OpcGroups)
                {
                    opcGroup = OpcGroups.FirstOrDefault(g => g.Id == id);
                    if (opcGroup == null)
                    {
                        opcGroup = new OpcGroup(Logger, this, id, groupName, items, p => p.Name);
                        OpcGroups.Add(opcGroup);
                    }
                    else
                    {
                        return opcGroup;
                    }
                }

                var subscription = await CreateSubscriptionAsync(Server, opcGroup);
                if (subscription == null)
                    return null;
            }
            catch (Exception ex)
            {
                Logger.Error($"[ERROR][CreateOpcGroupAsync]");
                Logger.Error(ex);
            }

            return opcGroup;
        }

        public async Task RemoveOpcGroupAsync(Guid groupId)
        {
            Logger.Debug($"[{nameof(OpcDaServer)}: {Name}][Host: {Host}][Type: {Type}][Method: {nameof(RemoveOpcGroupAsync)}] remove groupId '{groupId}'. [CountGroups: {OpcGroups.Count}]");

            try
            {
                OPCDA.Subscription subscription = null;
                for (int i = 0; i < Server.Subscriptions.Count; i++)
                {
                    if((Guid)Server.Subscriptions[i].ClientHandle == groupId)
                    {
                        subscription = Server.Subscriptions[i];
                        break;
                    }
                }

                if(subscription != null)
                {
                    try
                    {
                        Server.CancelSubscription(subscription);
                    }
                    catch { }

                    try
                    {
                        Server.Subscriptions.Remove(subscription);
                    }
                    catch { }

                    try
                    {
                        //subscription.DataChanged -= OnItemsChanged;
                    }
                    catch { }

                    try
                    {
                        subscription.Dispose();
                    }
                    catch { }
                }
            }
            catch { }

            lock (OpcGroups)
            {
                OpcGroups.RemoveAll(g => g.Id == groupId);
            }

            if (OpcGroups.Count <= 0)
            {
                Logger.Debug($"[{nameof(OpcDaServer)}: {Name}][Host: {Host}][Type: {Type}][Method: {nameof(RemoveOpcGroupAsync)}] Groups.Count <= 0. Disconnect from the server.");
                await DisconnectAsync();
            }
        }

        public Task<IEnumerable<IDiscoveryItem>> BrowseAsync(string item) => Task.Run(async () =>
        {
            if (!IsConnected)
                await ConnectAsync();

            if (!IsConnected)
                return new List<IDiscoveryItem>();

            if (string.IsNullOrEmpty(item))
            {
                var items = Server?.Browse(new OPC.ItemIdentifier(), new OPCDA.BrowseFilters(), out OPCDA.BrowsePosition pos0);
                return items?.Where(p => !p.IsItem)
                    .Select(p => (IDiscoveryItem)new DiscoveryItem
                    {
                        Name = p.ItemName,
                        Id = Guid.NewGuid().ToString(),
                        HasChildren = p.HasChildren,
                        AccessPath = p.ItemPath
                    })?
                    .ToList();
            }

            var childs = Server.Browse(new OPC.ItemIdentifier { ItemName = item }, new OPCDA.BrowseFilters { }, out OPCDA.BrowsePosition pos);
            
            IEnumerable<IDiscoveryItem> result = new List<IDiscoveryItem>();
            result = childs?.Select(p => (IDiscoveryItem)(new DiscoveryItem
            {
                Name = p.ItemName,
                Id = Guid.NewGuid().ToString(),
                HasChildren = p.HasChildren,
                AccessPath = p.ItemPath,
            }))?.ToList();

            return result ?? new List<IDiscoveryItem>();
        });

        public Task<IEnumerable<IDiscoveryItem>> BrowseAllAsync(string item) => Task.Run(async () =>
        {
            var childs = await BrowseAsync(item);
            var tasks = childs.Select(p => Task.Run(async() =>
            {
                if (p.HasChildren)
                {
                    p.Childs = await BrowseAllAsync(p.Name);
                }
            }));

            await Task.WhenAll(tasks);

            await Task.Delay(100);

            /*foreach (var child in childs)
            {
                if(child.HasChildren)
                {
                    child.Childs = await BrowseAllAsync(child);
                }
            }*/

            return childs ?? new List<IDiscoveryItem>();
        });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptionHandle">OPC group</param>
        /// <param name="requestHandle">Null</param>
        /// <param name="values">Items values</param>
        private void OnItemsChanged(object subscriptionHandle, object requestHandle, OPCDA.ItemValueResult[] values)
        {
            try
            {
                var opcGroup = (IOpcGroup)subscriptionHandle;

                if (opcGroup == null) return;

                var group = OpcGroups.FirstOrDefault(g => g.Id == opcGroup.Id);

                if (group == null) return;

                var groupItems = group.GetOpcItems();

                Array arrayValue = null;
                IOpcItem item = null;
                group?.CallOpcItemsChangedEvent(values.Select(v =>
                {
                //item = groupItems.FirstOrDefault(x => x.Name.ToLower() == v.ItemName.ToLower());
                item = groupItems[v.ItemName];
                    return new OpcItem
                    {
                        Id = item.Id,
                        Name = item.Name,
                        AccessPath = item.AccessPath,
                        CanonicalDataType = item.CanonicalDataType,
                        CanonicalDataTypeId = item.CanonicalDataTypeId,
                        ReadOnly = item.ReadOnly,
                        ReqDataType = item.ReqDataType,

                        Value = (arrayValue = v.Value as Array) != null ? arrayValue.ConvertToString(";") : v.Value?.ToString(),
                        Quality = OpcDaHelper.GetQuality((int)v.Quality.QualityBits, (int)v.Quality.LimitBits),
                        TimestampUtc = v.Timestamp.ToUniversalTime().ToString(WellKnownCodes.ExportDateTimeFormat, CultureInfo.InvariantCulture),
                        TimestampLocal = v.Timestamp.ToLocalTime().ToString(WellKnownCodes.ExportDateTimeFormat, CultureInfo.InvariantCulture)
                    };
                }));
            }
            catch(Exception ex)
            {
                Logger.Error($"[ERROR][OnItemsChanged]");
                Logger.Error(ex);
            }
        }

        private async Task<OPCDA.Subscription> CreateSubscriptionAsync(OPCDA.Server server, IOpcGroup opcGroup)
        {
            OPCDA.Subscription subscription = null;
            var subscriptionState = new OPCDA.SubscriptionState
            {
                Name = $"{opcGroup.Name}-{opcGroup.Id}",
                Active = true,
                UpdateRate = 1000,
                ClientHandle = opcGroup
            };

            try
            {
                subscription = (OPCDA.Subscription)server.CreateSubscription(subscriptionState);
                subscription.AddItems(opcGroup.GetOpcItems().Values.Select(p => new OPCDA.Item
                {
                    ItemName = p.Name,
                    ClientHandle = p.Id,
                    ServerHandle = p.Id,
                    ItemPath = p.AccessPath,
                }).ToArray());

                try
                {
                    var result = await Task.Run(() => subscription.Read(subscription.Items));
                    OnItemsChanged(opcGroup, null, result);
                }
                catch { }

                try
                {
                    subscription.DataChanged += OnItemsChanged;
                }
                catch
                {
                    //Отключаем обновления
                    try { subscription.DataChanged -= OnItemsChanged; }
                    catch { }

                    //Запускаем принудительное обновление
                    //StartForceUpdateItemsAsync(group, groupDto.ReqUpdateRate);
                }

            }
            catch (Exception ex) 
            {
                Logger.Error($"[ERROR][CreateSubscriptionAsync]");
                Logger.Error(ex);
            }

            return subscription;
        }
    }
}
