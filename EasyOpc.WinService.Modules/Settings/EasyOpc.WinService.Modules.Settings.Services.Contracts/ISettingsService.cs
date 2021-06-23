using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Settings.Services.Models;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Settings.Services.Contracts
{
    /// <summary>
    /// Contract for the settings service
    /// </summary>
    public interface ISettingsService : IBaseService<Setting>
    {
        /// <summary>
        /// Get setting by name
        /// </summary>
        /// <param name="name">Setting name</param>
        /// <returns>Setting</returns>
        Task<Setting> GetByNameAsync(string name);
    }
}
