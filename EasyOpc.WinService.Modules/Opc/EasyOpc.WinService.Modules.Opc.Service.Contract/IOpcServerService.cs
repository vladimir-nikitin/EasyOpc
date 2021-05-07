using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Service.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Service.Contract
{
    /// <summary>
    /// Contract for the OPC server service
    /// </summary>
    public interface IOpcServerService : IBaseService<OpcServer>
    {
        /// <summary>
        /// Returns a list of servers of a specific type
        /// </summary>
        /// <param name="type">Server type</param>
        /// <returns>Server list</returns>
        Task<IEnumerable<OpcServer>> GetByTypeAsync(string type);

        /// <summary>
        /// Import OPC.DA servers from Matrikon config file
        /// </summary>
        /// <param name="data">Config file from Matrikon</param>
        /// <returns></returns>
        Task ImportOpcDaServersAsync(string data);

        /// <summary>
        /// Browse OPC item
        /// </summary>
        Task<IEnumerable<DiscoveryItem>> BrowseAsync(Guid opcServerId, string itemName, string accessPath);

        /// <summary>
        /// Get all OPC item childs
        /// </summary>
        Task<IEnumerable<DiscoveryItem>> BrowseAllAsync(Guid opcServerId, string itemName, string accessPath);

        /// <summary>
        /// Tries to shut down the OPC server
        /// </summary>
        /// <param name="opcServerId">OPC Server Id</param>
        /// <returns></returns>
        Task<bool> TryDisconnectFromOpcServerAsync(Guid opcServerId);
    }
}
