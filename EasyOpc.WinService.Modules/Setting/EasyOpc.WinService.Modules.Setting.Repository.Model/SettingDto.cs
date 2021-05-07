using EasyOpc.WinService.Core.Repository.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyOpc.WinService.Modules.Setting.Repository.Model
{
    /// <summary>
    /// Setting
    /// </summary>
    [Table("Settings")]
    public class SettingDto : BaseDto
    {
        /// <summary>
        /// Setting name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Setting value
        /// </summary>
        public string Value { get; set; }
    }
}
