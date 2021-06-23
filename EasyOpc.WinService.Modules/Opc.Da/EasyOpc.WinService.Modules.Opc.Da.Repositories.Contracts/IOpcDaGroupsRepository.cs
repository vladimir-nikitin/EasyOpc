using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Da.Repositories.Contracts
{
    /// <summary>
    /// Contract for the OPC.DA group repository
    /// </summary>
    public interface IOpcDaGroupsRepository : IBaseRepository<OpcDaGroupDto>
    {
        /// <summary>
        /// Returns a list of groups for a specific server
        /// </summary>
        /// <param name="opcDaServerId">Server ID</param>
        /// <returns>List of groups</returns>
        Task<IEnumerable<OpcDaGroupDto>> GetByOpcDaServerIdAsync(Guid opcDaServerId);
    }
}
