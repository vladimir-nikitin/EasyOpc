using EasyOpc.WinService.Core.Configuration.Contract;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Ua.Repositories
{
    /// <summary>
    /// OPC.UA groups repository
    /// </summary>
    public class OpcUaGroupsRepository : BaseRepository<DataBaseContext, OpcUaGroupDto>, IOpcUaGroupsRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpcUaGroupsRepository(IConfiguration configuration, ILogger logger) : 
            base(new DataBaseContext(configuration[ConfigurationKeys.ConnectionString]), logger)
        {
        }

        /// <inheritdoc cref="IOpcUaGroupsRepository.GetByOpcUaServerIdAsync(Guid)"/>
        public async Task<IEnumerable<OpcUaGroupDto>> GetByOpcUaServerIdAsync(Guid opcUaServerId)
        {
            try
            {
                return await Entities.Where(p => p.OpcUaServerId == opcUaServerId).ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
