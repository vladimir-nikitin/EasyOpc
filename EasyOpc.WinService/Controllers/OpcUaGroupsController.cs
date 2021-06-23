using AutoMapper;
using EasyOpc.Contracts.Opc;
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
    [RoutePrefix("api/opcuagroups")]
    public class OpcUaGroupsController : ApiController
    {
        private IOpcUaGroupsService OpcUaGroupsService { get; }

        private IOpcUaItemsService OpcUaItemsService { get; }

        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public OpcUaGroupsController(IOpcUaGroupsService opcUaGroupsService, IOpcUaItemsService opcUaItemsService, 
            ILogger logger, IMapper mapper)
        {
            OpcUaGroupsService = opcUaGroupsService;
            OpcUaItemsService = opcUaItemsService;

            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("getByOpcUaServerId/{opcUaServerId}")]
        public async Task<IEnumerable<OpcUaGroupData>> GetByOpcServerIdAsync(Guid opcUaServerId)
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcUaGroupData>>(await OpcUaGroupsService.GetByOpcUaServerIdAsync(opcUaServerId));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task AddAsync([FromBody] AddOpcUaGroupRequest group)
        {
            try
            {
                await OpcUaGroupsService.AddAsync(Mapper.Map<OpcUaGroup>(group));
                await OpcUaItemsService.AddRangeAsync(Mapper.Map<IEnumerable<OpcUaItem>>(group.OpcUaItems));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpDelete]
        [Route("{opcUaGroupId}")]
        public async Task RemoveByIdAsync(Guid opcUaGroupId)
        {
            try
            {
                await OpcUaGroupsService.RemoveByIdAsync(opcUaGroupId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
