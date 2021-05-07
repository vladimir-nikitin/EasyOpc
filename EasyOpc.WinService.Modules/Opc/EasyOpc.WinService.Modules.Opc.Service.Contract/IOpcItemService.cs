using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Service.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Service.Contract
{
    /// <summary>
    /// Contract for the OPC item service
    /// </summary>
    public interface IOpcItemService : IBaseService<OpcItem>
    {
        /// <summary>
        /// Returns a list of items for a specific OPC group
        /// </summary>
        /// <param name="id">OPC group ID</param>
        /// <returns>List of items</returns>
        Task<IEnumerable<OpcItem>> GetByOpcGroupIdAsync(Guid id);
    }
}
