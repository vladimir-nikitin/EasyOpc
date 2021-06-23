using EasyOpc.Common.Types;
using System;

namespace EasyOpc.WinService.Modules.Settings.Services.Models
{
    /// <summary>
    /// Setting
    /// </summary>
    public class Setting : IIdentifiable
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

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
