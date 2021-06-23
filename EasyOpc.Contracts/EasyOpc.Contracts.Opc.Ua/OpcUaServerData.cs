using EasyOpc.Common.Types;
using System;

namespace EasyOpc.Contracts.Opc.Ua
{
    /// <summary>
    /// OPC.UA server
    /// </summary>
	public class OpcUaServerData : IIdentifiable
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
        /// User
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}