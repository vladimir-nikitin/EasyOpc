using EasyOpc.Common.Types;
using System;
using System.Collections.Generic;

namespace EasyOpc.WinService.Modules.Opc.Ua.Services.Models
{
    /// <summary>
    /// OPC.UA server
    /// </summary>
	public class OpcUaServer : IIdentifiable
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
		/// Server name
		/// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
