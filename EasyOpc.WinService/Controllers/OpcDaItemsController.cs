using AutoMapper;
using EasyOpc.Common.Types;
using EasyOpc.Contracts.Opc;
using EasyOpc.Contracts.Opc.Da;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Modules.Opc.Da.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyOpc.WinService.Controllers
{
    [RoutePrefix("api/opcdaitems")]
    public class OpcDaItemsController : ApiController
    {
        private IOpcDaItemsService  OpcDaItemsService { get; }

        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public OpcDaItemsController(IOpcDaItemsService opcDaItemsService, ILogger logger, IMapper mapper)
        {
            OpcDaItemsService = opcDaItemsService;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("getByOpcDaGroupId/{opcDaGroupId}")]
        public async Task<IEnumerable<OpcDaItemData>> GetByOpcGroupIdAsync(Guid opcDaGroupId)
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcDaItemData>>(await OpcDaItemsService.GetByOpcDaGroupIdAsync(opcDaGroupId));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("getPage/opcDaGroup/{opcDaGroupId}/page/{pageNumber}/count/{countInPage}")]
        public async Task<Page<OpcDaItemData>> GetByFilterAsync(Guid opcDaGroupId, int pageNumber, int countInPage)
        {
            try
            {
                var page = await OpcDaItemsService.GetPageAsync(opcDaGroupId, pageNumber, countInPage);
                return new Page<OpcDaItemData>
                {
                    PageNumber = page.PageNumber,
                    CountInPage = page.CountInPage,
                    TotalCount = page.TotalCount,
                    Items = Mapper.Map<IEnumerable<OpcDaItemData>>(page.Items)
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
