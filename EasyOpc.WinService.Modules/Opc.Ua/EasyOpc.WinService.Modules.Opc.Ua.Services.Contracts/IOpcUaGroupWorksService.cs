using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Core.WorksService.Contract;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Ua.Services.Contracts
{
    /// <summary>
    /// Contract for the OPC.UA works service
    /// </summary>
    public interface IOpcUaGroupWorksService : IBaseService<OpcUaGroupWork>, IWorksService
    {
        /// <summary>
        /// Return work by opcUaGroupId and work type
        /// </summary>
        /// <param name="opcUaGroupId">OPC.UA group id</param>
        /// <param name="workTypes">Work types</param>
        /// <returns>Works</returns>
        Task<IEnumerable<OpcUaGroupWork>> GetByOpcUaGroupIdAndTypeAsync(Guid opcUaGroupId, IEnumerable<string> workTypes);
    }
}
