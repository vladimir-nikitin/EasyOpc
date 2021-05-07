using EasyOpc.Common.Opc;
using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Repository.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Repository.Contract
{
    /// <summary>
    /// Contract for the OPC server repository
    /// </summary>
    public interface IOpcServerRepository : IBaseRepository<OpcServerDto>
    {
        /// <summary>
        /// Returns a list of servers of a specific type
        /// </summary>
        /// <param name="type">Server type</param>
        /// <returns>Server list</returns>
        Task<IEnumerable<OpcServerDto>> GetByTypeAsync(OpcServerType type);
    }
}
