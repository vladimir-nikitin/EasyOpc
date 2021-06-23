using AutoMapper;
using EasyOpc.Contracts.Opc.Da;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Modules.Opc.Da.Services.Contracts;
using EasyOpc.WinService.Modules.Opc.Da.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyOpc.WinService.Controllers
{
    [RoutePrefix("api/opcdagroupworks")]
    public class OpcDaGroupWorksController : ApiController
    {
        private IOpcDaGroupWorksService OpcDaGroupWorksService { get; }

        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public OpcDaGroupWorksController(IOpcDaGroupWorksService opcDaGroupWorksService, ILogger logger, IMapper mapper)
        {
            OpcDaGroupWorksService = opcDaGroupWorksService;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("getByOpcDaGroupIdAndType/{opcDaGroupId}")]
        public async Task<IEnumerable<OpcDaGroupWorkData>> GetByOpcServerIdAsync(Guid opcDaGroupId, [FromUri] string[] workTypes)
        {
            try
            {
                var result = Mapper.Map<IEnumerable<OpcDaGroupWorkData>>(await OpcDaGroupWorksService.GetByOpcDaGroupIdAndTypeAsync(opcDaGroupId, workTypes));
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task AddAsync([FromBody] OpcDaGroupWorkData work)
        {
            try
            {
                await OpcDaGroupWorksService.AddAsync(Mapper.Map<OpcDaGroupWork>(work));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] OpcDaGroupWorkData work)
        {
            try
            {
                await OpcDaGroupWorksService.UpdateAsync(Mapper.Map<OpcDaGroupWork>(work));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpDelete]
        [Route("delete/{opcDaGroupWorkId}")]
        public async Task RemoveByIdAsync(Guid opcDaGroupWorkId)
        {
            try
            {
                await OpcDaGroupWorksService.RemoveByIdAsync(opcDaGroupWorkId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
