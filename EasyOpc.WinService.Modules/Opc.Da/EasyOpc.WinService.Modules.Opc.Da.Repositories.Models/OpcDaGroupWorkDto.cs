using EasyOpc.WinService.Core.Repository.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyOpc.WinService.Modules.Opc.Da.Repositories.Models
{
    /// <summary>
    /// OPC.DA group work
    /// </summary>
    [Table("OpcDaGroupWorks")]
    public class OpcDaGroupWorkDto : BaseDto
    {
        /// <summary>
        /// OPC.DA group id
        /// </summary>
        public Guid OpcDaGroupId { get; set; }

        /// <summary>
        /// Work name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Work type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Activity flag
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Launch group
        /// </summary>
        public string LaunchGroup { get; set; }

        /// <summary>
        /// Work settings in json format
        /// </summary>
        public string JsonSettings { get; set; }
    }
}
