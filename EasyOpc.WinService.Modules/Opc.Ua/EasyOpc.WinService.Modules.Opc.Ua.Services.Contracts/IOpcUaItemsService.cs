using EasyOpc.Common.Types;
using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Ua.Services.Contracts
{
    /// <summary>
    /// Contract for the OPC.UA items service
    /// </summary>
    public interface IOpcUaItemsService : IBaseService<OpcUaItem>
    {
        /// <summary>
        /// Returns a list of items for a specific OPC.UA group
        /// </summary>
        /// <param name="id">OPC.UA group ID</param>
        /// <returns>List of items</returns>
        Task<IEnumerable<OpcUaItem>> GetByOpcUaGroupIdAsync(Guid id);

        /// <summary>
        /// Return page
        /// </summary>
        /// <param name="opcUaGroupId">OPC.UA group ID</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="countInPage">Count items in page</param>
        /// <returns></returns>
        Task<Page<OpcUaItem>> GetPageAsync(Guid opcUaGroupId, int pageNumber, int countInPage);
    }
}
