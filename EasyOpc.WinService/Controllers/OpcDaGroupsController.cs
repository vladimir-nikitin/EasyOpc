using AutoMapper;
using EasyOpc.Contracts.Opc;
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
    [RoutePrefix("api/opcdagroups")]
    public class OpcDaGroupsController : ApiController
    {
        private IOpcDaGroupsService OpcDaGroupsService { get; }

        private IOpcDaItemsService OpcDaItemsService { get; }

        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public OpcDaGroupsController(IOpcDaGroupsService opcDaGroupsService, IOpcDaItemsService opcDaItemsService,
            ILogger logger, IMapper mapper)
        {
            OpcDaGroupsService = opcDaGroupsService;
            OpcDaItemsService = opcDaItemsService;

            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("getByOpcDaServerId/{opcDaServerId}")]
        public async Task<IEnumerable<OpcDaGroupData>> GetByOpcServerIdAsync(Guid opcDaServerId)
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcDaGroupData>>(await OpcDaGroupsService.GetByOpcDaServerIdAsync(opcDaServerId));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task AddAsync([FromBody] AddOpcDaGroupRequest group)
        {
            try
            {
                await OpcDaGroupsService.AddAsync(Mapper.Map<OpcDaGroup>(group));

                var dic = new Dictionary<string, OpcDaItemData>();
                foreach (var item in group.OpcDaItems)
                {
                    if(!dic.ContainsKey(item.Name))
                    {
                        dic.Add(item.Name, item);
                    }
                }

                await OpcDaItemsService.AddRangeAsync(Mapper.Map<IEnumerable<OpcDaItem>>(dic.Values));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpDelete]
        [Route("{opcDaGroupId}")]
        public async Task RemoveByIdAsync(Guid opcDaGroupId)
        {
            try
            {
                await OpcDaGroupsService.RemoveByIdAsync(opcDaGroupId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
