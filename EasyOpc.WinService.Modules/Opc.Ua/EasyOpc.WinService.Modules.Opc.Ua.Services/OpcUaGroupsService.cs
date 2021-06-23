using AutoMapper;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Contracts;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Ua.Services
{
    /// <summary>
    /// OPC.UA group service
    /// </summary>
    public class OpcUaGroupsService : BaseService<OpcUaGroup, OpcUaGroupDto>, IOpcUaGroupsService
    {
        private IOpcUaItemsService OpcUaItemsService { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpcUaGroupsService(IOpcUaItemsService opcUaItemsService, IOpcUaGroupsRepository repository,  
            IMapper mapper, ILogger logger) : base(repository, mapper, logger)
        {
            OpcUaItemsService = opcUaItemsService;
        }

        public async Task<IEnumerable<OpcUaGroup>> GetByOpcUaServerIdAsync(Guid id)
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcUaGroup>>(await (Repository as IOpcUaGroupsRepository).GetByOpcUaServerIdAsync(id));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
