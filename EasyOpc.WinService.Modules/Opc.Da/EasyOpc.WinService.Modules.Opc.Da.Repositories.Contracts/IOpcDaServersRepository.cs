using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Models;

namespace EasyOpc.WinService.Modules.Opc.Da.Repositories.Contracts
{
    /// <summary>
    /// Contract for the OPC.DA server repository
    /// </summary>
    public interface IOpcDaServersRepository : IBaseRepository<OpcDaServerDto>
    {
    }
}
