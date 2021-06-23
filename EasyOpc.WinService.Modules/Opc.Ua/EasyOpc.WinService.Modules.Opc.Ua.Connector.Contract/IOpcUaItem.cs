using System;

namespace EasyOpc.WinService.Modules.Opc.Ua.Connector.Contract
{
    public interface IOpcUaItem
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
		/// NodeId
		/// </summary>
		string NodeId { get; set; }

		/// <summary>
		/// Value
		/// </summary>
		string Value { get; set; }

		/// <summary>
		/// Time stamp
		/// </summary>
		DateTime? Timestamp { get; set; }
	}
}
