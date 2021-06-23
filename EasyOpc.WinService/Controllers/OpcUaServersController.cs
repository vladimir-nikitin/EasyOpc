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
    [RoutePrefix("api/opcuaservers")]
    public class OpcUaServersController : ApiController
    {
        private IOpcUaServersService OpcUaServersService { get; }

        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public OpcUaServersController(IOpcUaServersService opcUaServersService, ILogger logger, IMapper mapper)
        {
            OpcUaServersService = opcUaServersService;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IEnumerable<OpcUaServerData>> GetAllAsync()
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcUaServerData>>(await OpcUaServersService.GetAllAsync());
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task AddAsync([FromBody] OpcUaServerData server)
        {
            try
            {
                await OpcUaServersService.AddAsync(Mapper.Map<OpcUaServer>(server));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpDelete]
        [Route("{opcUaServerId}")]
        public async Task RemoveAsync(Guid opcUaServerId)
        {
            try
            {
                await OpcUaServersService.RemoveByIdAsync(opcUaServerId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("browse/{opcUaServerId}")]
        public async Task<IEnumerable<DiscoveryItemData>> BrowseAsync(Guid opcUaServerId, [FromUri] string nodeId = null)
        {
            try
            {
                return Mapper.Map<IEnumerable<DiscoveryItemData>>(await OpcUaServersService.BrowseAsync(opcUaServerId, nodeId));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("browseAll/{opcUaServerId}")]
        public async Task<IEnumerable<DiscoveryItemData>> BrowseAllAsync(Guid opcUaServerId, [FromUri] string nodeId = null)
        {
            try
            {
                return Mapper.Map<IEnumerable<DiscoveryItemData>>(await OpcUaServersService.BrowseAllAsync(opcUaServerId, nodeId));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
