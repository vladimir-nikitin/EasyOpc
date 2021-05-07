using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Repository.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Repository.Contract
{
    /// <summary>
    /// Contract for the OPC group repository
    /// </summary>
    public interface IOpcGroupRepository : IBaseRepository<OpcGroupDto>
    {
        /// <summary>
        /// Returns a list of groups for a specific server
        /// </summary>
        /// <param name="opcServerId">Server ID</param>
        /// <returns>List of groups</returns>
        Task<IEnumerable<OpcGroupDto>> GetByOpcServerIdAsync(Guid opcServerId);
    }
}
