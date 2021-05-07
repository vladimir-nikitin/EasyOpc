using AutoMapper;
using EasyOpc.Contracts.Opc;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Modules.Opc.Service.Contract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyOpc.WinService.Controllers
{
    [RoutePrefix("api/opcitems")]
    public class OpcItemsController : ApiController
    {
        private IOpcItemService  OpcItemService { get; }

        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public OpcItemsController(IOpcItemService opcItemService, ILogger logger, IMapper mapper)
        {
            OpcItemService = opcItemService;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("getByOpcGroupId/{opcGroupId}")]
        public async Task<IEnumerable<OpcItemData>> GetByOpcGroupIdAsync(Guid opcGroupId)
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcItemData>>(await OpcItemService.GetByOpcGroupIdAsync(opcGroupId));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
