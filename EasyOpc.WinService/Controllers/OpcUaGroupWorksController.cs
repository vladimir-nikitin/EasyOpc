using AutoMapper;
using EasyOpc.Contracts.Opc.Ua;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Contracts;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyOpc.WinService.Controllers
{
    [RoutePrefix("api/opcuagroupworks")]
    public class OpcUaGroupWorksController : ApiController
    {
        private IOpcUaGroupWorksService OpcUaGroupWorksService { get; }

        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public OpcUaGroupWorksController(IOpcUaGroupWorksService opcUaGroupWorksService, ILogger logger, IMapper mapper)
        {
            OpcUaGroupWorksService = opcUaGroupWorksService;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("getByOpcUaGroupIdAndType/{opcUaGroupId}")]
        public async Task<IEnumerable<OpcUaGroupWorkData>> GetByOpcServerIdAsync(Guid opcUaGroupId, [FromUri] string[] workTypes)
        {
            try
            {
                var result = Mapper.Map<IEnumerable<OpcUaGroupWorkData>>(await OpcUaGroupWorksService.GetByOpcUaGroupIdAndTypeAsync(opcUaGroupId, workTypes));
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
        public async Task AddAsync([FromBody] OpcUaGroupWorkData work)
        {
            try
            {
                await OpcUaGroupWorksService.AddAsync(Mapper.Map<OpcUaGroupWork>(work));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] OpcUaGroupWorkData work)
        {
            try
            {
                await OpcUaGroupWorksService.UpdateAsync(Mapper.Map<OpcUaGroupWork>(work));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpDelete]
        [Route("delete/{opcUaGroupWorkId}")]
        public async Task RemoveByIdAsync(Guid opcUaGroupWorkId)
        {
            try
            {
                await OpcUaGroupWorksService.RemoveByIdAsync(opcUaGroupWorkId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
