using EasyOpc.Common.Types;
using System;
using System.Collections.Generic;

namespace EasyOpc.Contracts.Opc
{
    /// <summary>
    /// OPC server
    /// </summary>
	public class OpcServerData : IIdentifiable
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
        public string Type { get; set; }

        /// <summary>
        /// Server settings in json format
        /// </summary>
        public string JsonSettings { get; set; }

        /// <summary>
        /// OPC groups
        /// </summary>
        public IEnumerable<OpcGroupData> OpcGroups { get; set; }
    }
}