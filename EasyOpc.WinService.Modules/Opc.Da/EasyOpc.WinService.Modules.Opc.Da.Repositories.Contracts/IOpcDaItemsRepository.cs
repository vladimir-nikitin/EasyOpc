using EasyOpc.Common.Types;
using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Da.Repositories.Contracts
{
    /// <summary>
    /// Contract for the OPC.DA item repository
    /// </summary>
    public interface IOpcDaItemsRepository : IBaseRepository<OpcDaItemDto>
    {
        /// <summary>
        /// Returns a list of items for a specific OPC.DA group
        /// </summary>
        /// <param name="opcDaGroupId">OPC.DA group ID</param>
        /// <returns>List of items</returns>
        Task<IEnumerable<OpcDaItemDto>> GetByOpcDaGroupIdAsync(Guid opcDaGroupId);

        /// <summary>
        /// Return page
        /// </summary>
        /// <param name="opcDaGroupId">OPC.DA group ID</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="countInPage">Count items in page</param>
        /// <returns></returns>
        Task<Page<OpcDaItemDto>> GetPageAsync(Guid opcDaGroupId, int pageNumber, int countInPage);
    }
}
