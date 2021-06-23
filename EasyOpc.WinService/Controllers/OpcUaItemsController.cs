using AutoMapper;
using EasyOpc.Common.Types;
using EasyOpc.Contracts.Opc;
using EasyOpc.Contracts.Opc.Ua;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyOpc.WinService.Controllers
{
    [RoutePrefix("api/opcuaitems")]
    public class OpcUaItemsController : ApiController
    {
        private IOpcUaItemsService  OpcUaItemsService { get; }

        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public OpcUaItemsController(IOpcUaItemsService opcUaItemsService, ILogger logger, IMapper mapper)
        {
            OpcUaItemsService = opcUaItemsService;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("getByOpcUaGroupId/{opcUaGroupId}")]
        public async Task<IEnumerable<OpcUaItemData>> GetByOpcGroupIdAsync(Guid opcUaGroupId)
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcUaItemData>>(await OpcUaItemsService.GetByOpcUaGroupIdAsync(opcUaGroupId));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("getPage/opcUaGroup/{opcUaGroupId}/page/{pageNumber}/count/{countInPage}")]
        public async Task<Page<OpcUaItemData>> GetByFilterAsync(Guid opcUaGroupId, int pageNumber, int countInPage)
        {
            try
            {
                var page = await OpcUaItemsService.GetPageAsync(opcUaGroupId, pageNumber, countInPage);
                return new Page<OpcUaItemData>
                {
                    PageNumber = page.PageNumber,
                    CountInPage = page.CountInPage,
                    TotalCount = page.TotalCount,
                    Items = Mapper.Map<IEnumerable<OpcUaItemData>>(page.Items)
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
