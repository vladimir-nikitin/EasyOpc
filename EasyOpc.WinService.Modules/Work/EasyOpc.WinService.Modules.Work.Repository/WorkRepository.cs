using EasyOpc.WinService.Core.Configuration.Contract;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Work.Repository.Contract;
using EasyOpc.WinService.Modules.Work.Repository.Model;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Work.Repository
{
    /// <summary>
    /// Works repository
    /// </summary>
    public class WorkRepository : BaseRepository<DataBaseContext, WorkDto>, IWorkRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration module</param>
        /// <param name="logger">Logger module</param>
        public WorkRepository(IConfiguration configuration, ILogger logger) :
            base(new DataBaseContext(configuration[ConfigurationKeys.ConnectionString]), logger)
        {
        }

        public async Task<WorkDto> GetByTypeAndExternalIdAsync(string type, Guid externalId)
        {
            try
            {
                return await Entities.FirstOrDefaultAsync(i => i.Type == type && i.ExternalId == externalId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
