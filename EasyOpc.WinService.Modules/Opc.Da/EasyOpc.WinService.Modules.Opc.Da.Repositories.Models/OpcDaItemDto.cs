using EasyOpc.WinService.Core.Repository.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyOpc.WinService.Modules.Opc.Da.Repositories.Models
{
    /// <summary>
    /// OPC.DA element
    /// </summary>
    [Table("OpcDaItems")]
    public class OpcDaItemDto : BaseDto
    {
        /// <summary>
        /// Group ID
        /// </summary>
        public Guid OpcDaGroupId { get; set; }

        /// <summary>
        /// OPC group
        /// </summary>
        [ForeignKey("OpcDaGroupId")]
        public OpcDaGroupDto OpcDaGroup { get; set; }

        /// <summary>
		/// Element name
		/// </summary>
        public string Name { get; set; }

        /// <summary>
		/// Data type
		/// </summary>
		public string CanonicalDataType { get; set; }

        /// <summary>
        /// Element path 
        /// </summary>
        public string AccessPath { get; set; }

        /// <summary>
        /// Required data type
        /// </summary>
        public string ReqDataType { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Quality
        /// </summary>
        public string Quality { get; set; }

        /// <summary>
        /// Time stamp
        /// </summary>
        public DateTime? Timestamp { get; set; }
    }
}
