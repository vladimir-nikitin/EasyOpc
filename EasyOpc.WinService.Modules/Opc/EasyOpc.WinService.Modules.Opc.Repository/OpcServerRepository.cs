using EasyOpc.Common.Opc;
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
    /// OPC servers repository
    /// </summary>
    public class OpcServerRepository : BaseRepository<DataBaseContext, OpcServerDto>, IOpcServerRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration module</param>
        /// <param name="logger">Logger module</param>
        public OpcServerRepository(IConfiguration configuration, ILogger logger) :
            base(new DataBaseContext(configuration[ConfigurationKeys.ConnectionString]), logger)
        {
        }

        /// <summary>
        /// <see cref="IOpcServerRepository.GetByTypeAsync(OpcServerType)"/>
        /// </summary>
        public async Task<IEnumerable<OpcServerDto>> GetByTypeAsync(OpcServerType type)
        {
            try
            {
                return await Entities.Where(p => p.Type == type).ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
