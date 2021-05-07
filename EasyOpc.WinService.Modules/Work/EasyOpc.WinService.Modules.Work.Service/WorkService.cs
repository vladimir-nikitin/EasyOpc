using AutoMapper;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Work.Repository.Contract;
using EasyOpc.WinService.Modules.Work.Repository.Model;
using EasyOpc.WinService.Modules.Work.Service.Contract;
using System;
using System.Threading.Tasks;
using WorkType = EasyOpc.WinService.Core.Worker.Model.Work;


namespace EasyOpc.WinService.Modules.Work.Service
{
    /// <summary>
    /// Work service
    /// </summary>
    public class WorkService : BaseService<WorkType, WorkDto>, IWorkService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository">Setting repository</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="logger">Logger</param>
        public WorkService(IWorkRepository repository, IMapper mapper, ILogger logger)
            : base(repository, mapper, logger)
        {
        }

        public async Task<WorkType> GetByTypeAndExternalIdAsync(string type, Guid externalId)
        {
            return Mapper.Map<WorkType>(await (Repository as IWorkRepository).GetByTypeAndExternalIdAsync(type, externalId));
        }
    }
}
