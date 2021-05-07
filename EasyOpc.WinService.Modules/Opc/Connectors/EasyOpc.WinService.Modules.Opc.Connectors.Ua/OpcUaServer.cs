using System.Collections.Generic;
using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using EasyOpc.WinService.Modules.Opc.Connectors.Contract;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.Common.Opc;
using EasyOpc.Common.Constant;
using EasyOpc.Common.Extension;

namespace EasyOpc.WinService.Modules.Opc.Connectors.Ua
{
    public class OpcUaServer : IOpcServer
    {
        private List<IOpcGroup> OpcGroups { get; } = new List<IOpcGroup>();

        private Session Session { get; set; }

        public string User { get; }

        public string Password { get; }

        protected ILogger Logger { get; }

        public Guid Id { get; }

        public string Name { get; }

        public string Host { get; }

        public OpcServerType Type => OpcServerType.UA;

        public bool IsConnected => Session?.Connected ?? false;

        public OpcUaServer(ILogger logger, Guid id, string name, string host, string user = null, string password = null)
        {
            Logger = logger;
            Id = id;
            Name = name;
            Host = host;
            User = user;
            Password = password;
        }

        public async Task ConnectAsync()
        {
            Logger.Debug($"[{nameof(OpcUaServer)}: {Name}][Host: {Host}][Type: {Type}][Method: {nameof(ConnectAsync)}]");

            //Create application configuration and certificate
            var config = new ApplicationConfiguration()
            {
                ApplicationName = "MyHomework",
                ApplicationUri = Utils.Format(@"urn:{0}:MyHomework", Host),
                ApplicationType = ApplicationType.Client,
                SecurityConfiguration = new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\MachineDefault", SubjectName = Utils.Format(@"CN={0}, DC={1}", "MyHomework", System.Net.Dns.GetHostName()) },
                    TrustedIssuerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA Certificate Authorities" },
                    TrustedPeerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA Applications" },
                    RejectedCertificateStore = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\RejectedCertificates" },
                    AutoAcceptUntrustedCertificates = true,
                    AddAppCertToTrustedStore = true
                },
                TransportConfigurations = new TransportConfigurationCollection(),
                TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
                ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 },
                TraceConfiguration = new TraceConfiguration()
            };

            try
            {
                await config.Validate(ApplicationType.Client);
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Validate config error: {ex.Message}");
            }

            if (config.SecurityConfiguration.AutoAcceptUntrustedCertificates)
            {
                config.CertificateValidator.CertificateValidation += (s, e) => { e.Accept = (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted); };
            }

            var application = new ApplicationInstance
            {
                ApplicationName = "MyHomework",
                ApplicationType = ApplicationType.Client,
                ApplicationConfiguration = config
            };
            try
            {
                await application.CheckApplicationInstanceCertificate(false, 2048);
            }
            catch (Exception ex)
            {
               //MessageBox.Show($"CheckApplicationInstanceCertificate error: {ex.Message}");
            }

            var selectedEndpoint = CoreClientUtils.SelectEndpoint(Host, useSecurity: true, discoverTimeout: 15000);

            UserIdentity userIdentity = null;
            try
            {
                if(!string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(Password))
                {
                    userIdentity = new UserIdentity(User, Password);
                }
            }
            catch { }

            try
            {
                var session = await Session.Create(config,
                    new ConfiguredEndpoint(null, selectedEndpoint, EndpointConfiguration.Create(config)),
                    false, $"{Name}({Host})", 60000, userIdentity, null);

                Session = session;
            }
            catch (Exception ex)
            {
                Logger.Error($"[ERROR][ConnectAsync]");
                Logger.Error(ex);
                Session = null;
            }
        }

        public Task DisconnectAsync()
        {
            Logger.Debug($"[{nameof(OpcUaServer)}: {Name}][Host: {Host}][Type: {Type}][Method: {nameof(DisconnectAsync)}]");
            //IsConnected = false;

            try { Session?.CloseSession(null, true); }
            catch { }

            try { Session?.Close(); }
            catch { }

            try { Session?.Dispose(); }
            catch { }

            Session = null;

            return Task.CompletedTask;
        }

        public async Task ReconnectAsync()
        {
            await DisconnectAsync();

            await ConnectAsync();

            if (IsConnected)
            {
                foreach (var opcGroup in OpcGroups.ToList())
                {
                    CreateSubscription(Session, opcGroup);
                }
            }
        }

        public async Task<bool> PingAsync()
        {
            var items = await BrowseAsync(null);
            if(items != null && items.Count() > 0 )
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
                        opcGroup = new OpcGroup(Logger, this, id, groupName, items, p => p.Id.ToString());
                        OpcGroups.Add(opcGroup);
                    }
                    else
                    {
                        return opcGroup;
                    }
                }

                var subscription = CreateSubscription(Session, opcGroup);
                if (subscription == null)
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                Logger.Error($"[ERROR][CreateOpcGroupAsync]");
                Logger.Error(ex);
            }

            return opcGroup;
        }

        private void OnNotification(MonitoredItem item, MonitoredItemNotificationEventArgs e)
        {
            try
            {
                var opcItemData = ((Guid id, IOpcItem item))item.Handle;
                Array arrayValue = null;
                var newItems = item.DequeueValues()
                        .Select(p => (IOpcItem)new OpcItem
                        {
                            Id = opcItemData.item.Id,
                            Name = opcItemData.item.Name,
                            AccessPath = opcItemData.item.AccessPath,
                            CanonicalDataType = opcItemData.item.CanonicalDataType,
                            CanonicalDataTypeId = opcItemData.item.CanonicalDataTypeId,
                            ReadOnly = opcItemData.item.ReadOnly,
                            ReqDataType = opcItemData.item.ReqDataType,
                            Quality = "Good, non-specific",
                            Value = (arrayValue = p.Value as Array) != null ? arrayValue.ConvertToString(";") : p.Value?.ToString(),
                            TimestampLocal = DateTime.Now.ToString(WellKnownCodes.ExportDateTimeFormat),
                            TimestampUtc = DateTime.UtcNow.ToString(WellKnownCodes.ExportDateTimeFormat)
                        });

                var group = OpcGroups?.FirstOrDefault(g => g.Id == opcItemData.id);
                group?.CallOpcItemsChangedEvent(newItems);
            }
            catch (Exception ex)
            {
                Logger.Error($"[ERROR][OnNotification]");
                Logger.Error(ex);
            }
        }

        public async Task RemoveOpcGroupAsync(Guid groupId)
        {
            Logger.Debug($"[{nameof(OpcUaServer)}: {Name}][Host: {Host}][Type: {Type}][Method: {nameof(RemoveOpcGroupAsync)}] remove groupId: '{groupId}'. [CountGroups: {OpcGroups.Count}]");

            try
            {
                var subscription = Session.Subscriptions.FirstOrDefault(x => ((IOpcGroup)x.Handle).Id == groupId);
                if (subscription != null)
                {
                    try
                    {
                        subscription.MonitoredItems.ToList().ForEach(i => i.Notification -= OnNotification);
                    }
                    catch { }

                    try
                    {
                        subscription.DeleteItems();
                    }
                    catch { }

                    try
                    {
                        subscription.Dispose();
                    }
                    catch { }
                    try
                    {
                        Session.RemoveSubscription(subscription);
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
                Logger.Debug($"[{nameof(OpcUaServer)}: {Name}][Host: {Host}][Type: {Type}][Method: {nameof(RemoveOpcGroupAsync)}] Groups.Count <= 0. Disconnect from the server.");
                await DisconnectAsync();
            }
        }

        public async Task<IEnumerable<IDiscoveryItem>> BrowseAsync(string item)
        {
            if (!IsConnected)
                await ConnectAsync();

            if (!IsConnected)
                return new List<IDiscoveryItem>();
            
            ReferenceDescriptionCollection refs = null;
            Byte[] cp;
            var nodeToBrowse = string.IsNullOrEmpty(item) ? 
                ObjectIds.ObjectsFolder : new NodeId(item);

            await Task.Run(async () => {

                try
                {
                    Session.Browse(null, null, nodeToBrowse, 0u, BrowseDirection.Forward, ReferenceTypeIds.HierarchicalReferences, true, 
                        (uint)NodeClass.Variable | (uint)NodeClass.Object | (uint)NodeClass.Method, out cp, out refs);
                }
                catch 
                {
                    //await DisconnectAsync();
                    //await ConnectAsync();
                }
            });

            var items = refs?.Where(p => p.NodeClass == NodeClass.Object || p.NodeClass == NodeClass.Variable)
                        .Select(p => (IDiscoveryItem)new DiscoveryItem
                        {
                            Name = p.DisplayName.Text,
                            Id = Guid.NewGuid().ToString(),
                            AccessPath = p.NodeId.ToString(),
                            DataType = p.TypeDefinition.ToString(),
                            DataTypeId = p.TypeId.IdType.ToString(),
                            HasChildren = true
                        }).ToList()
                        ?? new List<IDiscoveryItem>();

            var result = new Dictionary<string, IDiscoveryItem>();
            items.ForEach(p => 
            { 
                if(!result.ContainsKey(p.Name))
                {
                    result.Add(p.Name, p);
                }
            });

            return result.Values;
        }

        public async Task<IEnumerable<IDiscoveryItem>> BrowseAllAsync(string item)
        {
            var childs = (await BrowseAsync(item)).ToList();
            var tasks = childs.Select(p => Task.Run(async () =>
            {
                if (p.HasChildren)
                {
                    p.Childs = await BrowseAllAsync(p.AccessPath);
                }
            }));

            await Task.WhenAll(tasks);

            await Task.Delay(100);

            /*for (int i = 0; i < childs.Count; i++)
            {
                if (childs[i].HasChildren)
                {
                    childs[i].Childs = await BrowseAllAsync(childs[i]);
                    if (childs[i].Childs == null || !childs[i].Childs.Any())
                    {
                        childs[i].HasChildren = false;
                    }

                    await Task.Delay(100);
                }
            }*/

            return childs ?? new List<IDiscoveryItem>();
        }

        private Subscription CreateSubscription(Session session, IOpcGroup opcGroup)
        {
            try
            {
                var subscription = new Subscription(session.DefaultSubscription)
                {
                    DisplayName = opcGroup.Name,
                    PublishingInterval = 1000,
                    Handle = opcGroup
                };

                var monitoredItems = opcGroup.GetOpcItems().Values
                    .Select(p => new MonitoredItem(subscription.DefaultItem)
                    {
                        DisplayName = $"{p.Name}-{p.Id}",
                        StartNodeId = p.AccessPath,
                        Handle = (opcGroup.Id, p)
                    })
                    .ToList();
                monitoredItems.ForEach(i => i.Notification += OnNotification);

                subscription.AddItems(monitoredItems);
                session.AddSubscription(subscription);
                subscription.Create();

                return subscription;
            }
            catch (Exception ex)
            {
                Logger.Error($"[ERROR][CreateSubscription]");
                Logger.Error(ex);
            }

            return null;
        }
    }
}
