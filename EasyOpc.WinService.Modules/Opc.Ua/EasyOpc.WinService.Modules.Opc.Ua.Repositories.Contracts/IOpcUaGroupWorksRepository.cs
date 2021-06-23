using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Ua.Repositories.Contracts
{
    /// <summary>
    /// Contract for the OPC.UA group works repository
    /// </summary>
    public interface IOpcUaGroupWorksRepository : IBaseRepository<OpcUaGroupWorkDto>
    {
        /// <summary>
        /// Return work by opcUaGroupId and work type
        /// </summary>
        /// <param name="opcUaGroupId">OPC.UA group id</param>
        /// <param name="workTypes">Work types</param>
        /// <returns>Works</returns>
        Task<IEnumerable<OpcUaGroupWorkDto>> GetByOpcUaGroupIdAndTypeAsync(Guid opcUaGroupId, IEnumerable<string> workTypes);
    }
}
