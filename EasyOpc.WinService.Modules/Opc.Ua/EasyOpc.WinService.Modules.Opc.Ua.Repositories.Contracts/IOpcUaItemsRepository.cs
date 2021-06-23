using EasyOpc.Common.Types;
using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Ua.Repositories.Contracts
{
    /// <summary>
    /// Contract for the OPC.UA items repository
    /// </summary>
    public interface IOpcUaItemsRepository : IBaseRepository<OpcUaItemDto>
    {
        /// <summary>
        /// Returns a list of items for a specific OPC.UA group
        /// </summary>
        /// <param name="opcUaGroupId">OPC.UA group ID</param>
        /// <returns>List of items</returns>
        Task<IEnumerable<OpcUaItemDto>> GetByOpcUaGroupIdAsync(Guid opcUaGroupId);

        /// <summary>
        /// Return page
        /// </summary>
        /// <param name="opcUaGroupId">OPC.UA group ID</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="countInPage">Count items in page</param>
        /// <returns></returns>
        Task<Page<OpcUaItemDto>> GetPageAsync(Guid opcUaGroupId, int pageNumber, int countInPage);
    }
}
