using System;

namespace EasyOpc.WinService.Modules.Opc.Connectors.Contract
{
    public interface IOpcItem
    {
		/// <summary>
		/// Id
		/// </summary>
		Guid Id { get; set; }

		/// <summary>
		/// Element name
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Data type
		/// </summary>
		string CanonicalDataType { get; set; }

		/// <summary>
		/// Data type ID
		/// </summary>
		string CanonicalDataTypeId { get; set; }

		/// <summary>
		/// Read-only rights
		/// </summary>
		int ReadOnly { get; set; }

		/// <summary>
		/// Element path 
		/// </summary>
		string AccessPath { get; set; }

		/// <summary>
		/// Required data type
		/// </summary>
		string ReqDataType { get; set; }

		/// <summary>
		/// Value
		/// </summary>
		string Value { get; set; }

		/// <summary>
		/// Quality
		/// </summary>
		string Quality { get; set; }

		/// <summary>
		/// Time stamp(UTC)
		/// </summary>
		string TimestampUtc { get; set; }

		/// <summary>
		/// Time stamp(Local)
		/// </summary>
		string TimestampLocal { get; set; }
	}
}
