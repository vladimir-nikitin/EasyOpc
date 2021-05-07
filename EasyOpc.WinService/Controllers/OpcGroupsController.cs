using AutoMapper;
using EasyOpc.Contracts.Opc;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Modules.Opc.Service.Contract;
using EasyOpc.WinService.Modules.Opc.Service.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyOpc.WinService.Controllers
{
    [RoutePrefix("api/opcgroups")]
    public class OpcGroupsController : ApiController
    {
        private IOpcGroupService OpcGroupService { get; }

        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public OpcGroupsController(IOpcGroupService opcGroupService, ILogger logger, IMapper mapper)
        {
            OpcGroupService = opcGroupService;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("getByOpcServerId/{opcServerId}")]
        public async Task<IEnumerable<OpcGroupData>> GetByOpcServerIdAsync(Guid opcServerId)
        {
            try
            {
                var sss = await OpcGroupService.GetByOpcServerIdAsync(opcServerId);
                var ssa = Mapper.Map<IEnumerable<OpcGroupData>>(sss);
                return Mapper.Map<IEnumerable<OpcGroupData>>(await OpcGroupService.GetByOpcServerIdAsync(opcServerId));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task AddAsync([FromBody] OpcGroupData group)
        {
            try
            {
                await OpcGroupService.AddAsync(Mapper.Map<OpcGroup>(group));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpDelete]
        [Route("{opcGroupId}")]
        public async Task RemoveByIdAsync(Guid opcGroupId)
        {
            try
            {
                await OpcGroupService.RemoveByIdAsync(opcGroupId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
