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
    /// OPC.UA works repository
    /// </summary>
    public class OpcUaGroupWorksRepository : BaseRepository<DataBaseContext, OpcUaGroupWorkDto>, IOpcUaGroupWorksRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpcUaGroupWorksRepository(IConfiguration configuration, ILogger logger) :
            base(new DataBaseContext(configuration[ConfigurationKeys.ConnectionString]), logger)
        {
        }

        /// <inheritdoc cref="IOpcUaGroupWorksRepository.GetByOpcUaGroupIdAndTypeAsync(Guid, IEnumerable{string})"/>
        public async Task<IEnumerable<OpcUaGroupWorkDto>> GetByOpcUaGroupIdAndTypeAsync(Guid opcUaGroupId, IEnumerable<string> workTypes)
        {
            try
            {
                return await Entities.Where(p => p.OpcUaGroupId == opcUaGroupId && workTypes.Contains(p.Type)).ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
