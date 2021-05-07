using AutoMapper;
using EasyOpc.Common.Extension;
using EasyOpc.Common.Opc;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Connectors;
using EasyOpc.WinService.Modules.Opc.Connectors.Contract;
using EasyOpc.WinService.Modules.Opc.Connectors.Da;
using EasyOpc.WinService.Modules.Opc.Connectors.Ua;
using EasyOpc.WinService.Modules.Opc.Repository.Contract;
using EasyOpc.WinService.Modules.Opc.Repository.Model;
using EasyOpc.WinService.Modules.Opc.Service.Contract;
using EasyOpc.WinService.Modules.Opc.Service.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using DiscoveryItem = EasyOpc.WinService.Modules.Opc.Service.Model.DiscoveryItem;
using OpcGroup = EasyOpc.WinService.Modules.Opc.Service.Model.OpcGroup;
using OpcItem = EasyOpc.WinService.Modules.Opc.Service.Model.OpcItem;

namespace EasyOpc.WinService.Modules.Opc.Service
{
    /// <summary>
    /// OPC server service
    /// </summary>
    public class OpcServerService : BaseService<OpcServer, OpcServerDto>, IOpcServerService
    {
        private OpcDaServerFactory OpcDaServerFactory { get; }

        private OpcUaServerFactory OpcUaServerFactory { get; }

        private IOpcGroupService OpcGroupService { get; }

        private IOpcItemService OpcItemService { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository">OPC server repository</param>
        /// <param name="opcGroupService">OPC groups service</param>
        /// <param name="opcItemService">OPC items service</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="logger">Logger</param>
        public OpcServerService(OpcDaServerFactory opcDaServerFactory, OpcUaServerFactory opcUaServerFactory, 
            IOpcServerRepository repository, IOpcGroupService opcGroupService, 
            IOpcItemService opcItemService, IMapper mapper, ILogger logger) : base(repository, mapper, logger)
        {
            OpcDaServerFactory = opcDaServerFactory;
            OpcUaServerFactory = opcUaServerFactory;
            OpcGroupService = opcGroupService;
            OpcItemService = opcItemService;
        }

        /// <summary>
        /// <see cref="IOpcServerService.GetByTypeAsync(OpcServerType)"/>
        /// </summary>
        public async Task<IEnumerable<OpcServer>> GetByTypeAsync(string type)
        {
            try
            {
                var opcType = type == OpcWellKnownCodes.OPC_DA_TYPE ? OpcServerType.DA : OpcServerType.UA;
                return Mapper.Map<IEnumerable<OpcServer>>(await (Repository as IOpcServerRepository).GetByTypeAsync(opcType));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="IOpcServerService.ImportOpcDaServersAsync(string)"/>
        /// </summary>
        public async Task ImportOpcDaServersAsync(string data)
        {
            var newOpcServers = Parse(data);
            var allOpcServers = await GetAllAsync();

            foreach (var newOpcServer in newOpcServers)
            {
                newOpcServer.Host = newOpcServer.Host.StartsWith("\\") ? "127.0.0.1" : newOpcServer.Host;
                var currentOpcServer = allOpcServers.FirstOrDefault(s => s.Host.ToLower() == newOpcServer.Host.ToLower()
                                                            && s.Name.ToLower() == newOpcServer.Name.ToLower());
                if(currentOpcServer == null)
                {
                    await AddAsync(newOpcServer);
                }
                else
                {
                    newOpcServer.Id = currentOpcServer.Id;
                    foreach (var group in newOpcServer.OpcGroups)
                    {
                        group.OpcServerId = currentOpcServer.Id;
                    }
                }

                await OpcGroupService.AddRangeAsync(newOpcServer.OpcGroups);

                foreach (var group in newOpcServer.OpcGroups)
                {
                    var items = new Dictionary<string, OpcItem>();
                    foreach (var item in group.OpcItems)
                    {
                        if (!items.ContainsKey(item.Name))
                        {
                            items.Add(item.Name, item);
                        }
                    }

                    if (items.Count > 0)
                    {
                        await OpcItemService.AddRangeAsync(items.Values);
                    }
                }

            }
        }

        /// <summary>
        /// <see cref="IBaseService.RemoveByIdAsync(Guid)"/>
        /// </summary>
        public override async Task<OpcServer> RemoveByIdAsync(Guid id)
        {
            var groups = await OpcGroupService.GetByOpcServerIdAsync(id);
            foreach (var group in groups)
            {
                var items = await OpcItemService.GetByOpcGroupIdAsync(group.Id);
                await OpcItemService.RemoveRangeAsync(items);
            }

            await OpcGroupService.RemoveRangeAsync(groups);

            return await base.RemoveByIdAsync(id);
        }

        /// <summary>
        /// <see cref="IOpcServerService.BrowseAsync(DiscoveryItemData)"/>
        /// </summary>
        public async Task<IEnumerable<DiscoveryItem>> BrowseAsync(Guid opcServerId, string itemName, string accessPath)
        {
            var serverDto = await Repository.GetByIdAsync(opcServerId);
            if (serverDto == null)
                return new List<DiscoveryItem>();

            IEnumerable<IDiscoveryItem> items;
            if (serverDto.Type == OpcServerType.DA)
            {
                var server  = OpcDaServerFactory.Create(serverDto.Id, serverDto.Name, serverDto.Host, serverDto.JsonSettings);
                items = await server.BrowseAsync(itemName);
            }
            else
            {
                var settings = string.IsNullOrEmpty(serverDto.JsonSettings) ? null : JsonConvert.DeserializeObject<OpcUaServerSettings>(serverDto.JsonSettings);
                var server  = OpcUaServerFactory.Create(serverDto.Id, serverDto.Name, serverDto.Host, settings?.UserIdentitySetting?.User, settings?.UserIdentitySetting?.Password);
                items = await server.BrowseAsync(accessPath);
            }

            return items.Select(p => GetDiscoveryItemData(opcServerId, p));
        }

        /// <summary>
        /// <see cref="IOpcServerService.BrowseAllAsync(DiscoveryItemData)"/>
        /// </summary>
        public async Task<IEnumerable<DiscoveryItem>> BrowseAllAsync(Guid opcServerId, string itemName, string accessPath)
{
            var serverDto = await Repository.GetByIdAsync(opcServerId);
            if (serverDto == null)
                return new List<DiscoveryItem>();

            IEnumerable<IDiscoveryItem> items;
            if (serverDto.Type == OpcServerType.DA)
            {
                var server = OpcDaServerFactory.Create(serverDto.Id, serverDto.Name, serverDto.Host, serverDto.JsonSettings);
                items = await server.BrowseAllAsync(itemName);
            }
            else
            {
                var settings = string.IsNullOrEmpty(serverDto.JsonSettings) ? null : JsonConvert.DeserializeObject<OpcUaServerSettings>(serverDto.JsonSettings);
                var server = OpcUaServerFactory.Create(serverDto.Id, serverDto.Name, serverDto.Host, settings?.UserIdentitySetting?.User, settings?.UserIdentitySetting?.Password);
                items = await server.BrowseAllAsync(accessPath);
            }

            return items.Select(p => GetDiscoveryItemData(opcServerId, p));
        }

        /// <summary>
        /// <see cref="IOpcServerService.TryDisconnectFromOpcServerAsync(Guid)"/>
        /// </summary>
        public async Task<bool> TryDisconnectFromOpcServerAsync(Guid opcServerId)
        {
            var serverDto = await Repository.GetByIdAsync(opcServerId);
            if (serverDto == null)
                return false;

            IOpcServer server;
            if (serverDto.Type == OpcServerType.DA)
            {
                server = OpcDaServerFactory.Create(serverDto.Id, serverDto.Name, serverDto.Host, serverDto.JsonSettings);
            }
            else
            {
                server = OpcUaServerFactory.Create(serverDto.Id, serverDto.Name, serverDto.Host, null, null);
            }

            if(server != null)
            {
                await server.DisconnectAsync();
                return server.IsConnected;
            }

            return false;
        }

        /// <summary>
        /// Parse xml config file from Matrikon
        /// </summary>
        /// <param name="data">XML cofig file content</param>
        /// <returns>OPC.DA servers</returns>
        private IEnumerable<OpcServer> Parse(string data)
        {
            var servers = new List<OpcServer>();

            if (string.IsNullOrEmpty(data))
               return servers;

            var doc = new XmlDocument();
            doc.LoadXml(data);

            foreach (XmlNode xmlHostname in doc.DocumentElement.ChildNodes)
            {
                var host = xmlHostname.Attributes["RemoteHost"].Value;
                foreach (XmlNode xmlServer in xmlHostname.ChildNodes)
                {
                    var groups = new List<OpcGroup>();
                    var server = new OpcServer()
                    {
                        Id = Guid.NewGuid(),
                        Host = host,
                        Type = OpcServerType.DA,
                        Name = xmlServer.Attributes["Name"].Value,
                        JsonSettings = xmlServer.Attributes["CLSID"]?.Value
                    };

                    foreach (XmlNode xmlGroup in xmlServer.ChildNodes)
                    {
                        var items = new List<OpcItem>();
                        var group = new OpcGroup()
                        {
                            Id = Guid.NewGuid(),
                            Name = xmlGroup.Attributes["Name"].Value,
                            ReqUpdateRate = xmlGroup.Attributes["ReqUpdateRate"].Value.ConvertToInt32(),
                            OpcServerId = server.Id
                        };

                        foreach (XmlNode xmlItem in xmlGroup.ChildNodes)
                        {
                            items.Add(new OpcItem()
                            {
                                Id = Guid.NewGuid(),
                                Name = xmlItem.FirstChild.Value,
                                AccessPath = xmlItem.Attributes["AccessPath"].Value,
                                ReqDataType = xmlItem.Attributes["ReqDataType"].Value,
                                OpcGroupId = group.Id
                            });
                        }
                        group.OpcItems = items;
                        groups.Add(group);
                    }

                    server.OpcGroups = groups;
                    servers.Add(server);
                }
            }

            return servers;
        }

        private DiscoveryItem GetDiscoveryItemData(Guid opcServerId, IDiscoveryItem item) => new DiscoveryItem
        {
            OpcServerId = opcServerId,
            Name = item.Name,
            Id = item.Id,
            AccessPath = item.AccessPath,
            DataType = item.DataType,
            DataTypeId = item.DataTypeId,
            HasChildren = item.HasChildren,
            Childs = item.Childs?.Select(p => GetDiscoveryItemData(opcServerId, p))
        };
    }
}
