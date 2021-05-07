using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Repository.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Repository.Contract
{
    /// <summary>
    /// Contract for the OPC item repository
    /// </summary>
    public interface IOpcItemRepository : IBaseRepository<OpcItemDto>
    {
        /// <summary>
        /// Returns a list of items for a specific OPC group
        /// </summary>
        /// <param name="opcGroupId">OPC group ID</param>
        /// <returns>List of items</returns>
        Task<IEnumerable<OpcItemDto>> GetByOpcGroupIdAsync(Guid opcGroupId);
    }
}
