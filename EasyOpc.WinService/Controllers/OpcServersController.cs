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
    [RoutePrefix("api/opcservers")]
    public class OpcServersController : ApiController
    {
        private IOpcServerService OpcServerService { get; }

        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public OpcServersController(IOpcServerService opcServerService, ILogger logger, IMapper mapper)
        {
            OpcServerService = opcServerService;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IEnumerable<OpcServerData>> GetAllAsync()
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcServerData>>(await OpcServerService.GetAllAsync());
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task AddAsync([FromBody] OpcServerData server)
        {
            try
            {
                await OpcServerService.AddAsync(Mapper.Map<OpcServer>(server));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpDelete]
        [Route("{opcServerId}")]
        public async Task RemoveAsync(Guid opcServerId)
        {
            try
            {
                await OpcServerService.RemoveByIdAsync(opcServerId);
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
                await OpcServerService.ImportOpcDaServersAsync(request.Content);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("browse/{opcServerId}")]
        public async Task<IEnumerable<DiscoveryItemData>> BrowseAsync(Guid opcServerId, [FromUri] string itemName = null, [FromUri] string accessPath = null)
        {
            try
            {
                return Mapper.Map<IEnumerable<DiscoveryItemData>>(await OpcServerService.BrowseAsync(opcServerId, itemName, accessPath));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("browseAll/{opcServerId}")]
        public async Task<IEnumerable<DiscoveryItemData>> BrowseAllAsync(Guid opcServerId, [FromUri] string itemName = null, [FromUri] string accessPath = null)
        {
            try
            {
                return Mapper.Map<IEnumerable<DiscoveryItemData>>(await OpcServerService.BrowseAllAsync(opcServerId, itemName, accessPath));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("tryDisconnect/{opcServerId}")]
        public async Task TryDisconnectAsync(Guid opcServerId)
        {
            try
            {
                await OpcServerService.TryDisconnectFromOpcServerAsync(opcServerId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
