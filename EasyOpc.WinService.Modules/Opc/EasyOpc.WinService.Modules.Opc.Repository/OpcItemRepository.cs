using EasyOpc.WinService.Core.Configuration.Contract;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Repository.Contract;
using EasyOpc.WinService.Modules.Opc.Repository.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Repository
{
    /// <summary>
    /// OPC items repository
    /// </summary>
    public class OpcItemRepository : BaseRepository<DataBaseContext, OpcItemDto>, IOpcItemRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration module</param>
        /// <param name="logger">Logger module</param>
        public OpcItemRepository(IConfiguration configuration, ILogger logger) : 
            base(new DataBaseContext(configuration[ConfigurationKeys.ConnectionString]), logger)
        {
        }

        /// <summary>
        /// <see cref="IOpcItemRepository.GetByOpcGroupIdAsync(Guid)"/>
        /// </summary>
        public async Task<IEnumerable<OpcItemDto>> GetByOpcGroupIdAsync(Guid opcGroupId)
        {
            try
            {
                return await Entities.Where(p => p.OpcGroupId == opcGroupId).ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
