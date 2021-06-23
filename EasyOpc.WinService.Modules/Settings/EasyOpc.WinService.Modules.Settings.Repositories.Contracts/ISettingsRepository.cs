using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Settings.Repositories.Models;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Settings.Repositories.Contracts
{
    /// <summary>
    /// Contract for the settings repository
    /// </summary>
    public interface ISettingsRepository : IBaseRepository<SettingDto>
    {
        /// <summary>
        /// Get setting by name
        /// </summary>
        /// <param name="name">Setting name</param>
        /// <returns>Setting</returns>
        Task<SettingDto> GetByNameAsync(string name);
    }
}
