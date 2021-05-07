using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Work.Repository.Model;
using System;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Work.Repository.Contract
{
    /// <summary>
    /// Contract for the work repository
    /// </summary>
    public interface IWorkRepository : IBaseRepository<WorkDto>
    {
        Task<WorkDto> GetByTypeAndExternalIdAsync(string type, Guid externalId);
    }
}
