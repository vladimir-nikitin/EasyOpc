using EasyOpc.WinService.Core.Repository.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyOpc.WinService.Modules.Opc.Repository.Model
{
    /// <summary>
    /// OPC element
    /// </summary>
    [Table("OpcItems")]
    public class OpcItemDto : BaseDto
    {
        /// <summary>
        /// Group ID
        /// </summary>
        public Guid OpcGroupId { get; set; }

        /// <summary>
        /// OPC group
        /// </summary>
        [ForeignKey("OpcGroupId")]
        public OpcGroupDto OpcGroup { get; set; }

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
        /// Time stamp(UTC)
        /// </summary>
        public string TimestampUtc { get; set; }

        /// <summary>
        /// Time stamp(Local)
        /// </summary>
        public string TimestampLocal { get; set; }
    }
}
