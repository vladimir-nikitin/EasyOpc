using EasyOpc.WinService.Core.Service.Base;
using System;
using System.Threading.Tasks;
using WorkType = EasyOpc.WinService.Core.Worker.Model.Work;

namespace EasyOpc.WinService.Modules.Work.Service.Contract
{
    /// <summary>
    /// Contract for the work service
    /// </summary>
    public interface IWorkService : IBaseService<WorkType>
    {
        Task<WorkType> GetByTypeAndExternalIdAsync(string type, Guid externalId);
    }
}
