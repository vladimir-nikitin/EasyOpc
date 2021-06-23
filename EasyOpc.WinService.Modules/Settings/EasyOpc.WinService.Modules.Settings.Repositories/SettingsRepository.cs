using EasyOpc.WinService.Core.Configuration.Contract;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Settings.Repositories.Contracts;
using EasyOpc.WinService.Modules.Settings.Repositories.Models;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Settings.Repositories
{
    /// <summary>
    /// Settings repository
    /// </summary>
    public class SettingsRepository : BaseRepository<DataBaseContext, SettingDto>, ISettingsRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsRepository(IConfiguration configuration, ILogger logger) :
            base(new DataBaseContext(configuration[ConfigurationKeys.ConnectionString]), logger)
        {
        }

        /// <summary>
        /// <see cref="ISettingRepository.GetByName(string)"/>
        /// </summary>
        public async Task<SettingDto> GetByNameAsync(string name)
        {
            try
            {
                return await Entities.FirstOrDefaultAsync(i => i.Name == name);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
