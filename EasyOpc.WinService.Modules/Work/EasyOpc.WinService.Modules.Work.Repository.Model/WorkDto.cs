using EasyOpc.WinService.Core.Repository.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyOpc.WinService.Modules.Work.Repository.Model
{
    /// <summary>
    /// Work
    /// </summary>
    [Table("Works")]
    public class WorkDto : BaseDto
    {
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
        /// Launch group
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Activity flag
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
