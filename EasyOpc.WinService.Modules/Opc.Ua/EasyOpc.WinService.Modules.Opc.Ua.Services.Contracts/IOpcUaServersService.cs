using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Ua.Services.Contracts
{
    /// <summary>
    /// Contract for the OPC.UA servers service
    /// </summary>
    public interface IOpcUaServersService : IBaseService<OpcUaServer>
    {
        /// <summary>
        /// Browse OPC.UA item
        /// </summary>
        Task<IEnumerable<DiscoveryItem>> BrowseAsync(Guid opcUaServerId, string nodeId);

        /// <summary>
        /// Get all OPC.UA item childs
        /// </summary>
        Task<IEnumerable<DiscoveryItem>> BrowseAllAsync(Guid opcServerId, string nodeId);
    }
}
