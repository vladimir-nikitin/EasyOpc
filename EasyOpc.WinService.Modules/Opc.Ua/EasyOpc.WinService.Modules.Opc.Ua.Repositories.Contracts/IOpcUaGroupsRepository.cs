using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Ua.Repositories.Contracts
{
    /// <summary>
    /// Contract for the OPC.UA group repository
    /// </summary>
    public interface IOpcUaGroupsRepository : IBaseRepository<OpcUaGroupDto>
    {
        /// <summary>
        /// Returns a list of groups for a specific server
        /// </summary>
        /// <param name="opcUaServerId">Server ID</param>
        /// <returns>List of groups</returns>
        Task<IEnumerable<OpcUaGroupDto>> GetByOpcUaServerIdAsync(Guid opcUaServerId);
    }
}
