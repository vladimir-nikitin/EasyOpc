using EasyOpc.Common.Opc;
using EasyOpc.Common.Types;
using System;
using System.Collections.Generic;

namespace EasyOpc.WinService.Modules.Opc.Service.Model
{
    /// <summary>
    /// OPC server
    /// </summary>
	public class OpcServer : IIdentifiable
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
        /// Server type DA/UA
        /// </summary>
        public OpcServerType Type { get; set; }

        /// <summary>
        /// Server settings in json format
        /// </summary>
        public string JsonSettings { get; set; }

        /// <summary>
        /// OPC groups
        /// </summary>
        public IEnumerable<OpcGroup> OpcGroups { get; set; }
    }
}
