using EasyOpc.WinService.Core.Configuration.Contract;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Da.Repositories
{
    /// <summary>
    /// OPC.DA groups repository
    /// </summary>
    public class OpcDaGroupsRepository : BaseRepository<DataBaseContext, OpcDaGroupDto>, IOpcDaGroupsRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration module</param>
        /// <param name="logger">Logger module</param>
        public OpcDaGroupsRepository(IConfiguration configuration, ILogger logger) : 
            base(new DataBaseContext(configuration[ConfigurationKeys.ConnectionString]), logger)
        {
        }

        ///<inheritdoc cref="IOpcDaGroupsRepository.GetByOpcDaServerIdAsync(Guid)"/>
        public async Task<IEnumerable<OpcDaGroupDto>> GetByOpcDaServerIdAsync(Guid opcDaServerId)
        {
            try
            {
                return await Entities.Where(p => p.OpcDaServerId == opcDaServerId).ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
