using AutoMapper;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Settings.Repositories.Contracts;
using EasyOpc.WinService.Modules.Settings.Repositories.Models;
using EasyOpc.WinService.Modules.Settings.Services.Contracts;
using EasyOpc.WinService.Modules.Settings.Services.Models;
using System;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Settings.Services
{
    /// <summary>
    /// Settings service
    /// </summary>
    public class SettingsService : BaseService<Setting, SettingDto>, ISettingsService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsService(ISettingsRepository repository, IMapper mapper, ILogger logger)
            : base(repository, mapper, logger)
        {
        }

        /// <summary>
        /// <see cref="ISettingService.GetByNameAsync(string)"/>
        /// </summary>
        public async Task<Setting> GetByNameAsync(string name)
        {
            try
            {
                return Mapper.Map<Setting>(await (Repository as ISettingsRepository).GetByNameAsync(name));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
