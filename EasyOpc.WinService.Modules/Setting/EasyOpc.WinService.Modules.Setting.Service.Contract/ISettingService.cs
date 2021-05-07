using EasyOpc.WinService.Core.Service.Base;
using System.Threading.Tasks;
using SettingType = EasyOpc.WinService.Modules.Setting.Service.Model.Setting;

namespace EasyOpc.WinService.Modules.Setting.Service.Contract
{
    /// <summary>
    /// Contract for the setting service
    /// </summary>
    public interface ISettingService : IBaseService<SettingType>
    {
        /// <summary>
        /// Get setting by name
        /// </summary>
        /// <param name="name">Setting name</param>
        /// <returns>Setting</returns>
        Task<SettingType> GetByNameAsync(string name);
    }
}
