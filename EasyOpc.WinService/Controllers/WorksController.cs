using AutoMapper;
using EasyOpc.Contracts.Works;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Worker.Model;
using EasyOpc.WinService.Modules.Work.Service.Contract;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyOpc.WinService.Controllers
{
    [RoutePrefix("api/works")]
    public class WorksController : ApiController
    {
        private IWorkExecutionService WorkExecutionService { get; }

        private IWorkService WorkService { get; }

        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public WorksController(IWorkService workService, IWorkExecutionService workExecutionService, ILogger logger, IMapper mapper)
        {
            WorkExecutionService = workExecutionService;
            WorkService = workService;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("getByTypeAndExternalId")]
        public async Task<WorkData> GetByTypeAndExternalIdAsync([FromUri]string type, [FromUri] Guid externalId)
        {
            try
            {
                return Mapper.Map<WorkData>(await WorkService.GetByTypeAndExternalIdAsync(type, externalId));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task AddAsync([FromBody] WorkData work)
        {
            try
            {
                await WorkService.AddAsync(Mapper.Map<Work>(work));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] WorkData work)
        {
            try
            {
                await WorkService.UpdateAsync(Mapper.Map<Work>(work));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPut]
        [Route("start")]
        public async Task StartAsync()
        {
            try
            {
                await WorkExecutionService.StartAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPut]
        [Route("stop")]
        public async Task StopAsync()
        {
            try
            {
                await WorkExecutionService.StopAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
