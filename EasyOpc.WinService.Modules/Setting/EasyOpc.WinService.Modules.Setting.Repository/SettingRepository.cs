using EasyOpc.WinService.Core.Configuration.Contract;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Setting.Repository.Contract;
using EasyOpc.WinService.Modules.Setting.Repository.Model;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Setting.Repository
{
    /// <summary>
    /// Settings repository
    /// </summary>
    public class SettingRepository : BaseRepository<DataBaseContext, SettingDto>, ISettingRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration module</param>
        /// <param name="logger">Logger module</param>
        public SettingRepository(IConfiguration configuration, ILogger logger) :
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
