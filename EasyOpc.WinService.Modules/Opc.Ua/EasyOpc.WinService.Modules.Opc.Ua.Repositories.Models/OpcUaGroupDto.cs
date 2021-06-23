using EasyOpc.WinService.Core.Repository.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models
{
    /// <summary>
    /// OPC.UA group
    /// </summary>
    [Table("OpcUaGroups")]
    public class OpcUaGroupDto : BaseDto
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
        /// OPC.UA server ID
        /// </summary>
        public Guid OpcUaServerId { get; set; }
    }
}
