using EasyOpc.Common.Types;
using System;

namespace EasyOpc.Contracts.Opc.Ua
{
	/// <summary>
	/// OPC.UA element
	/// </summary>
	public class OpcUaItemData : IIdentifiable
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

		/// <summary>
		/// Group ID
		/// </summary>
		public Guid OpcUaGroupId { get; set; }

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
		/// Time stamp(UTC)
		/// </summary>
		public string TimestampUtc { get; set; }

		/// <summary>
		/// Time stamp(Local)
		/// </summary>
		public string TimestampLocal { get; set; }
    }
}
