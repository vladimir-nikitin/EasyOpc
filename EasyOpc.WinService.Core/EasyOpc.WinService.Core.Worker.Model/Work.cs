using EasyOpc.Common.Types;
using System;

namespace EasyOpc.WinService.Core.Worker.Model
{
    /// <summary>
    /// Work
    /// </summary>
    public class Work : IIdentifiable
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Work type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Work name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Work settings in json format
        /// </summary>
        public string JsonSettings { get; set; }

        /// <summary>
        /// External object reference
        /// </summary>
        public Guid? ExternalId { get; set; }

        /// <summary>
        /// Work group
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Activity flag
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
