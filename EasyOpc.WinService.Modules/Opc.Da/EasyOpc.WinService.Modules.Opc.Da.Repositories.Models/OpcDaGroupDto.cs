using EasyOpc.WinService.Core.Repository.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyOpc.WinService.Modules.Opc.Da.Repositories.Models
{
    /// <summary>
    /// OPC.DA group
    /// </summary>
    [Table("OpcDaGroups")]
    public class OpcDaGroupDto : BaseDto
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
        /// OPC.DA server ID
        /// </summary>
        public Guid OpcDaServerId { get; set; }
    }
}
