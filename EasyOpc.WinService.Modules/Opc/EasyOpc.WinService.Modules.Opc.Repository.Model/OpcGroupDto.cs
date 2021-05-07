using EasyOpc.WinService.Core.Repository.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyOpc.WinService.Modules.Opc.Repository.Model
{
    /// <summary>
    /// OPC group
    /// </summary>
    [Table("OpcGroups")]
    public class OpcGroupDto : BaseDto
    {
        /// <summary>
        /// Group name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Recommended update period
        /// </summary>
        public int ReqUpdateRate { get; set; }

        /// <summary>
        /// OPC server ID
        /// </summary>
        public Guid OpcServerId { get; set; }
    }
}
