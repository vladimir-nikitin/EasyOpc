using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Da.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Da.Services.Contracts
{
    /// <summary>
    /// Contract for the OPC server service
    /// </summary>
    public interface IOpcDaServersService : IBaseService<OpcDaServer>
    {
        /// <summary>
        /// Import OPC.DA servers from Matrikon config file
        /// </summary>
        /// <param name="data">Config file from Matrikon</param>
        /// <returns></returns>
        Task ImportOpcDaServersAsync(string data);

        /// <summary>
        /// Browse OPC item
        /// </summary>
        Task<IEnumerable<DiscoveryItem>> BrowseAsync(Guid opcServerId, string itemName);

        /// <summary>
        /// Get all OPC item childs
        /// </summary>
        Task<IEnumerable<DiscoveryItem>> BrowseAllAsync(Guid opcServerId, string itemName);
    }
}
