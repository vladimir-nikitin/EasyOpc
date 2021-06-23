using EasyOpc.Common.Types;
using System;

namespace EasyOpc.WinService.Modules.Opc.Ua.Services.Models
{
	/// <summary>
	/// OPC.UA element
	/// </summary>
	public class OpcUaItem : IIdentifiable
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
		/// Time stamp
		/// </summary>
		public DateTime? Timestamp { get; set; }
	}
}
