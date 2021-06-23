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
    /// OPC.DA works repository
    /// </summary>
    public class OpcDaGroupWorksRepository : BaseRepository<DataBaseContext, OpcDaGroupWorkDto>, IOpcDaGroupWorksRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpcDaGroupWorksRepository(IConfiguration configuration, ILogger logger) :
            base(new DataBaseContext(configuration[ConfigurationKeys.ConnectionString]), logger)
        {
        }

        /// <inheritdoc cref="IOpcDaGroupWorksRepository.GetByOpcDaGroupIdAndTypeAsync(Guid, string)"/>
        public async Task<IEnumerable<OpcDaGroupWorkDto>> GetByOpcDaGroupIdAndTypeAsync(Guid opcDaGroupId, IEnumerable<string> types)
        {
            try
            {
                return await Entities.Where(p => p.OpcDaGroupId == opcDaGroupId && types.Contains(p.Type)).ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
