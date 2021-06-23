using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Da.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Da.Services.Contracts
{
    /// <summary>
    /// Contract for the OPC.DA group service
    /// </summary>
    public interface IOpcDaGroupsService : IBaseService<OpcDaGroup>
    {
        /// <summary>
        /// Returns a list of groups for a specific server
        /// </summary>
        /// <param name="id">Server ID</param>
        /// <returns>List of groups</returns>
        Task<IEnumerable<OpcDaGroup>> GetByOpcDaServerIdAsync(Guid id);
    }
}
