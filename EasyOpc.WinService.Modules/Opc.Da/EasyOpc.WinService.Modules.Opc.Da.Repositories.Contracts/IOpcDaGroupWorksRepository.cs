using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Da.Repositories.Contracts
{
    /// <summary>
    /// Contract for the OPC.DA group works repository
    /// </summary>
    public interface IOpcDaGroupWorksRepository : IBaseRepository<OpcDaGroupWorkDto>
    {
        /// <summary>
        /// Return work by opcDaGroupId and work type
        /// </summary>
        /// <param name="opcDaGroupId">OPC.DA group id</param>
        /// <param name="workTypes">Work types</param>
        /// <returns>Works</returns>
        Task<IEnumerable<OpcDaGroupWorkDto>> GetByOpcDaGroupIdAndTypeAsync(Guid opcDaGroupId, IEnumerable<string> workTypes);
    }
}
