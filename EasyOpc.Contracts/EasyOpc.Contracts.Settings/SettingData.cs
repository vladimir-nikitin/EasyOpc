using EasyOpc.Common.Types;
using System;

namespace EasyOpc.Contract.Setting
{
    /// <summary>
    /// Setting
    /// </summary>
    public class SettingData : IIdentifiable
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
