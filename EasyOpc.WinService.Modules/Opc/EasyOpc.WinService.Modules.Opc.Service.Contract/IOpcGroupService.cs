using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Service.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Service.Contract
{
    /// <summary>
    /// Contract for the OPC group service
    /// </summary>
    public interface IOpcGroupService : IBaseService<OpcGroup>
    {
        /// <summary>
        /// Returns a list of groups for a specific server
        /// </summary>
        /// <param name="id">Server ID</param>
        /// <returns>List of groups</returns>
        Task<IEnumerable<OpcGroup>> GetByOpcServerIdAsync(Guid id);
    }
}
