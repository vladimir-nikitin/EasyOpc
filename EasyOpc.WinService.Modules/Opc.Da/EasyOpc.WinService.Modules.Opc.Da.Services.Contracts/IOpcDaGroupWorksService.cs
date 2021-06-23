using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Core.WorksService.Contract;
using EasyOpc.WinService.Modules.Opc.Da.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Da.Services.Contracts
{
    /// <summary>
    /// Contract for the OPC.DA works service
    /// </summary>
    public interface IOpcDaGroupWorksService : IBaseService<OpcDaGroupWork>, IWorksService
    {
        /// <summary>
        /// Return work by opcDaGroupId and work type
        /// </summary>
        /// <param name="opcDaGroupId">OPC.DA group id</param>
        /// <param name="workTypes">Work types</param>
        /// <returns>Works</returns>
        Task<IEnumerable<OpcDaGroupWork>> GetByOpcDaGroupIdAndTypeAsync(Guid opcDaGroupId, IEnumerable<string> workTypes);
    }
}
