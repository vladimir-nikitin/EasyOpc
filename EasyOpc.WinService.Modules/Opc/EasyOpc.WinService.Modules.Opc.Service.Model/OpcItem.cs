using EasyOpc.Common.Types;
using System;

namespace EasyOpc.WinService.Modules.Opc.Service.Model
{
	/// <summary>
	/// OPC element
	/// </summary>
	public class OpcItem : IIdentifiable
	{
		/// <summary>
		/// ID
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Group ID
		/// </summary>
		public Guid OpcGroupId { get; set; }

		/// <summary>
		/// Element name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Data type
		/// </summary>
		public string CanonicalDataType { get; set; }

		/// <summary>
		/// Data type ID
		/// </summary>
		public string CanonicalDataTypeId { get; set; }

		/// <summary>
		/// Read-only rights
		/// </summary>
		public int ReadOnly { get; set; }

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
