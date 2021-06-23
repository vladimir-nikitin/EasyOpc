using AutoMapper;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Models;
using EasyOpc.WinService.Modules.Opc.Da.Services.Contracts;
using EasyOpc.WinService.Modules.Opc.Da.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Da.Services
{
    /// <summary>
    /// OPC.DA group service
    /// </summary>
    public class OpcDaGroupsService : BaseService<OpcDaGroup, OpcDaGroupDto>, IOpcDaGroupsService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpcDaGroupsService(IOpcDaGroupsRepository repository, IMapper mapper, ILogger logger) 
            : base(repository, mapper, logger)
        {
        }

        ///<inheritdoc cref="IOpcDaGroupsService.GetByOpcDaServerIdAsync(Guid)"/>
        public async Task<IEnumerable<OpcDaGroup>> GetByOpcDaServerIdAsync(Guid id)
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcDaGroup>>(await (Repository as IOpcDaGroupsRepository).GetByOpcDaServerIdAsync(id));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
