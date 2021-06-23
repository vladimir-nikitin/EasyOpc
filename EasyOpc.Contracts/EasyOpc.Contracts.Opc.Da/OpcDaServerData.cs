using EasyOpc.Common.Types;
using System;
using System.Collections.Generic;

namespace EasyOpc.Contracts.Opc.Da
{
    /// <summary>
    /// OPC.DA server
    /// </summary>
	public class OpcDaServerData : IIdentifiable
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
        /// CLSID
        /// </summary>
        public string Clsid { get; set; }
    }
}