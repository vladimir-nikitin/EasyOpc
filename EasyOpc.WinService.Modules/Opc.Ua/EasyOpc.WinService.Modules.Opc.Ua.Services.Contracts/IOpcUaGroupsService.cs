using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Ua.Services.Contracts
{
    /// <summary>
    /// Contract for the OPC.UA groups service
    /// </summary>
    public interface IOpcUaGroupsService : IBaseService<OpcUaGroup>
    {
        /// <summary>
        /// Returns a list of groups for a specific server
        /// </summary>
        /// <param name="id">Server ID</param>
        /// <returns>List of groups</returns>
        Task<IEnumerable<OpcUaGroup>> GetByOpcUaServerIdAsync(Guid id);
    }
}
