using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.WorksExecutionService.Contract;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyOpc.WinService.Controllers
{
    [RoutePrefix("api/works")]
    public class WorksController : ApiController
    {
        private IWorksExecutionService WorksExecutionService { get; }

        private ILogger Logger { get; }

        public WorksController(IWorksExecutionService worksExecutionService, ILogger logger)
        {
            WorksExecutionService = worksExecutionService;
            Logger = logger;
        }


        [HttpPut]
        [Route("start")]
        public async Task StartAsync()
        {
            try
            {
                await WorksExecutionService.StartAsync();
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
                await WorksExecutionService.StopAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
