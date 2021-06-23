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
    [RoutePrefix("api/opcdaservers")]
    public class OpcDaServersController : ApiController
    {
        private IOpcDaServersService OpcDaServersService { get; }

        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public OpcDaServersController(IOpcDaServersService opcDaServersService, ILogger logger, IMapper mapper)
        {
            OpcDaServersService = opcDaServersService;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IEnumerable<OpcDaServerData>> GetAllAsync()
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcDaServerData>>(await OpcDaServersService.GetAllAsync());
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task AddAsync([FromBody] OpcDaServerData server)
        {
            try
            {
                await OpcDaServersService.AddAsync(Mapper.Map<OpcDaServer>(server));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpDelete]
        [Route("{opcDaServerId}")]
        public async Task RemoveAsync(Guid opcDaServerId)
        {
            try
            {
                await OpcDaServersService.RemoveByIdAsync(opcDaServerId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("import")]
        public async Task ImportAsync([FromBody] ImportOpcDaServersRequest request)
        {
            try
            {
                await OpcDaServersService.ImportOpcDaServersAsync(request.Content);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("browse/{opcDaServerId}")]
        public async Task<IEnumerable<DiscoveryItemData>> BrowseAsync(Guid opcDaServerId, [FromUri] string itemName = null)
        {
            try
            {
                return Mapper.Map<IEnumerable<DiscoveryItemData>>(await OpcDaServersService.BrowseAsync(opcDaServerId, itemName));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("browseAll/{opcDaServerId}")]
        public async Task<IEnumerable<DiscoveryItemData>> BrowseAllAsync(Guid opcDaServerId, [FromUri] string itemName = null)
        {
            try
            {
                return Mapper.Map<IEnumerable<DiscoveryItemData>>(await OpcDaServersService.BrowseAllAsync(opcDaServerId, itemName));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
