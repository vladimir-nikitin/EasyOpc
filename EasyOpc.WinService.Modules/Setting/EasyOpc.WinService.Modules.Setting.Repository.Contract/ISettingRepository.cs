using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Setting.Repository.Model;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Setting.Repository.Contract
{
    /// <summary>
    /// Contract for the setting repository
    /// </summary>
    public interface ISettingRepository : IBaseRepository<SettingDto>
    {
        /// <summary>
        /// Get setting by name
        /// </summary>
        /// <param name="name">Setting name</param>
        /// <returns>Setting</returns>
        Task<SettingDto> GetByNameAsync(string name);
    }
}
