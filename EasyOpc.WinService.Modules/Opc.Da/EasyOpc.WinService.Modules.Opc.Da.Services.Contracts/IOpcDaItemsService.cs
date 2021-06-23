using EasyOpc.Common.Types;
using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Da.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Da.Services.Contracts
{
    /// <summary>
    /// Contract for the OPC.DA item service
    /// </summary>
    public interface IOpcDaItemsService : IBaseService<OpcDaItem>
    {
        /// <summary>
        /// Returns a list of items for a specific OPC group
        /// </summary>
        /// <param name="id">OPC.DA group ID</param>
        /// <returns>List of items</returns>
        Task<IEnumerable<OpcDaItem>> GetByOpcDaGroupIdAsync(Guid id);

        /// <summary>
        /// Return page
        /// </summary>
        /// <param name="opcGroupId">OPC.DA group ID</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="countInPage">Count items in page</param>
        /// <returns></returns>
        Task<Page<OpcDaItem>> GetPageAsync(Guid opcDaGroupId, int pageNumber, int countInPage);
    }
}
