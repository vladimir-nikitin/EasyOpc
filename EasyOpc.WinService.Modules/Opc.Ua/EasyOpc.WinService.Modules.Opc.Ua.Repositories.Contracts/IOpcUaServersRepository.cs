using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models;

namespace EasyOpc.WinService.Modules.Opc.Ua.Repositories.Contracts
{
    /// <summary>
    /// Contract for the OPC.UA server repository
    /// </summary>
    public interface IOpcUaServersRepository : IBaseRepository<OpcUaServerDto>
    {
    }
}
