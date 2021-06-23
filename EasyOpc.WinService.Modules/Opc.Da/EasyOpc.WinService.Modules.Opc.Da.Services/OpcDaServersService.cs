using AutoMapper;
using EasyOpc.Common.Extensions;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Da.Connector.Contract;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Models;
using EasyOpc.WinService.Modules.Opc.Da.Services.Contracts;
using EasyOpc.WinService.Modules.Opc.Da.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace EasyOpc.WinService.Modules.Opc.Da.Services
{
    /// <summary>
    /// OPC.DA servers service
    /// </summary>
    public class OpcDaServersService : BaseService<OpcDaServer, OpcDaServerDto>, IOpcDaServersService
    {
        private IOpcDaServersFactory OpcDaServersFactory { get; }

        private IOpcDaGroupsService OpcDaGroupsService { get; }

        private IOpcDaItemsService OpcDaItemsService { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpcDaServersService(IOpcDaServersRepository repository, IOpcDaServersFactory opcDaServersFactory, 
            IOpcDaGroupsService opcDaGroupsService, IOpcDaItemsService opcDaItemsService, 
            IMapper mapper, ILogger logger) : base(repository, mapper, logger)
        {
            OpcDaServersFactory = opcDaServersFactory;
            OpcDaGroupsService = opcDaGroupsService;
            OpcDaItemsService = opcDaItemsService;
        }

        public async Task ImportOpcDaServersAsync(string data)
        {
            var session = Parse(data);
            var allOpcServers = await GetAllAsync();

            foreach (var newOpcServer in session.opcDaServers)
            {
                newOpcServer.Host = newOpcServer.Host.StartsWith("\\") ? "127.0.0.1" : newOpcServer.Host;
                var currentOpcServer = allOpcServers.FirstOrDefault(s => s.Host.ToLower() == newOpcServer.Host.ToLower()
                                                            && s.Name.ToLower() == newOpcServer.Name.ToLower());
                if (currentOpcServer != null)
                {
                    session.opcDaGroups.Where(p => p.OpcDaServerId == newOpcServer.Id).ToList().ForEach(p => p.OpcDaServerId = currentOpcServer.Id);
                    newOpcServer.Id = currentOpcServer.Id;
                }
            }

            await AddRangeAsync(session.opcDaServers);
            await OpcDaGroupsService.AddRangeAsync(session.opcDaGroups);

            var dic = new Dictionary<string, OpcDaItem>();
            foreach (var item in session.opcDaItems)
            {
                if (!dic.ContainsKey(item.OpcDaGroupId + "::" + item.Name))
                {
                    dic.Add(item.OpcDaGroupId + "::" + item.Name, item);
                }
            }
            await OpcDaItemsService.AddRangeAsync(dic.Values);
        }

        public async Task<IEnumerable<DiscoveryItem>> BrowseAsync(Guid opcServerId, string itemName)
        {
            var serverDto = await Repository.GetByIdAsync(opcServerId);
            if (serverDto == null)
                return new List<DiscoveryItem>();

            var server = OpcDaServersFactory.Create(serverDto.Id, serverDto.Name, serverDto.Host, serverDto.Clsid);
            var items = await server.BrowseAsync(itemName);

            return items.Select(p => GetDiscoveryItemData(opcServerId, p));
        }

        public async Task<IEnumerable<DiscoveryItem>> BrowseAllAsync(Guid opcServerId, string itemName)
        {
            var serverDto = await Repository.GetByIdAsync(opcServerId);
            if (serverDto == null)
                return new List<DiscoveryItem>();

            var server = OpcDaServersFactory.Create(serverDto.Id, serverDto.Name, serverDto.Host, serverDto.Clsid);
            var items = await server.BrowseAllAsync(itemName);

            return items.Select(p => GetDiscoveryItemData(opcServerId, p));
        }

        /// <summary>
        /// Parse xml config file from Matrikon
        /// </summary>
        /// <param name="data">XML cofig file content</param>
        /// <returns>OPC.DA servers</returns>
        private (IEnumerable<OpcDaServer> opcDaServers, IEnumerable<OpcDaGroup> opcDaGroups, IEnumerable<OpcDaItem> opcDaItems) Parse(string data)
        {
            var servers = new List<OpcDaServer>();
            var groups = new List<OpcDaGroup>();
            var items = new List<OpcDaItem>();

            if (string.IsNullOrEmpty(data))
               return (servers, groups, items);

            var doc = new XmlDocument();
            doc.LoadXml(data);

            foreach (XmlNode xmlHostname in doc.DocumentElement.ChildNodes)
            {
                var host = xmlHostname.Attributes["RemoteHost"].Value;
                foreach (XmlNode xmlServer in xmlHostname.ChildNodes)
                {
                    //var groups = new List<OpcDaGroup>();
                    var server = new OpcDaServer()
                    {
                        Id = Guid.NewGuid(),
                        Host = host,
                        Name = xmlServer.Attributes["Name"].Value,
                        Clsid = xmlServer.Attributes["CLSID"]?.Value
                    };

                    foreach (XmlNode xmlGroup in xmlServer.ChildNodes)
                    {
                        //var items = new List<OpcDaItem>();
                        var group = new OpcDaGroup()
                        {
                            Id = Guid.NewGuid(),
                            Name = xmlGroup.Attributes["Name"].Value,
                            ReqUpdateRate = xmlGroup.Attributes["ReqUpdateRate"].Value.ConvertToInt32(),
                            OpcDaServerId = server.Id
                        };

                        foreach (XmlNode xmlItem in xmlGroup.ChildNodes)
                        {
                            items.Add(new OpcDaItem()
                            {
                                Id = Guid.NewGuid(),
                                Name = xmlItem.FirstChild.Value,
                                AccessPath = xmlItem.Attributes["AccessPath"].Value,
                                ReqDataType = xmlItem.Attributes["ReqDataType"].Value,
                                OpcDaGroupId = group.Id
                            });
                        }
                        //group.OpcDaItems = items;
                        groups.Add(group);
                    }

                    //server.OpcDaGroups = groups;
                    servers.Add(server);
                }
            }

            return (servers, groups, items);
        }

        private DiscoveryItem GetDiscoveryItemData(Guid opcDaServerId, IDiscoveryItem item) => new DiscoveryItem
        {
            OpcDaServerId = opcDaServerId,
            Name = item.Name,
            Id = item.Id,
            AccessPath = item.AccessPath,
            DataType = item.DataType,
            DataTypeId = item.DataTypeId,
            HasChildren = item.HasChildren,
            HasValue = item.HasValue,
            Childs = item.Childs?.Select(p => GetDiscoveryItemData(opcDaServerId, p))
        };
    }
}
