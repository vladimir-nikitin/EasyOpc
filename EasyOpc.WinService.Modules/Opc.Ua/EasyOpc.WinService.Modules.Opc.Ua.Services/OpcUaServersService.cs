using AutoMapper;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Ua.Connector.Contract;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Contracts;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Ua.Services
{
    /// <summary>
    /// OPC.UA servers service
    /// </summary>
    public class OpcUaServersService : BaseService<OpcUaServer, OpcUaServerDto>, IOpcUaServersService
    {
        private IOpcUaServersFactory OpcUaServersFactory { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpcUaServersService(IOpcUaServersFactory opcUaServersFactory, IOpcUaServersRepository repository,
            IMapper mapper, ILogger logger) : base(repository, mapper, logger)
        {
            OpcUaServersFactory = opcUaServersFactory;
        }

        public async Task<IEnumerable<DiscoveryItem>> BrowseAsync(Guid opcUaServerId, string nodeId)
        {
            var serverDto = await Repository.GetByIdAsync(opcUaServerId);
            if (serverDto == null)
                return new List<DiscoveryItem>();

            var server = OpcUaServersFactory.Create(serverDto.Id, serverDto.Name, serverDto.Host, serverDto.UserName, serverDto.Password);
            var items = await server.BrowseAsync(nodeId);

            return items.Select(p => GetDiscoveryItemData(opcUaServerId, p));
        }

        public async Task<IEnumerable<DiscoveryItem>> BrowseAllAsync(Guid opcUaServerId, string nodeId)
{
            var serverDto = await Repository.GetByIdAsync(opcUaServerId);
            if (serverDto == null)
                return new List<DiscoveryItem>();

            var server = OpcUaServersFactory.Create(serverDto.Id, serverDto.Name, serverDto.Host, serverDto.UserName, serverDto.Password);
            var items = await server.BrowseAllAsync(nodeId);

            return items.Select(p => GetDiscoveryItemData(opcUaServerId, p));
        }

        private DiscoveryItem GetDiscoveryItemData(Guid opcUaServerId, IDiscoveryItem item) => new DiscoveryItem
        {
            OpcUaServerId = opcUaServerId,
            Name = item.Name,
            Id = item.Id,
            NodeId = item.NodeId,
            HasChildren = item.HasChildren,
            HasValue = item.HasValue,
            Childs = item.Childs?.Select(p => GetDiscoveryItemData(opcUaServerId, p))
        };
    }
}
