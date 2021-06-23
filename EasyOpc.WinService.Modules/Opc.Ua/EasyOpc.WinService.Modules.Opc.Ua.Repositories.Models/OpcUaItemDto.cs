using EasyOpc.WinService.Core.Repository.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models
{
    /// <summary>
    /// OPC.UA element
    /// </summary>
    [Table("OpcUaItems")]
    public class OpcUaItemDto : BaseDto
    {
        /// <summary>
        /// Group ID
        /// </summary>
        public Guid OpcUaGroupId { get; set; }

        /// <summary>
        /// OPC group
        /// </summary>
        [ForeignKey("OpcUaGroupId")]
        public OpcUaGroupDto OpcGroup { get; set; }

        /// <summary>
		/// Element name
		/// </summary>
        public string Name { get; set; }

        /// <summary>
        /// NodeId
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Time stamp
        /// </summary>
        public DateTime? Timestamp { get; set; }
    }
}
