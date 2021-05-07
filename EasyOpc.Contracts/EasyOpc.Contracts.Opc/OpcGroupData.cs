using EasyOpc.Common.Types;
using System;
using System.Collections.Generic;

namespace EasyOpc.Contracts.Opc
{
    /// <summary>
    /// OPC group
    /// </summary>
    public class OpcGroupData : IIdentifiable
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// OPC server ID
        /// </summary>
        public Guid OpcServerId { get; set; }

        /// <summary>
        /// Group name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Recommended update period
        /// </summary>
        public int ReqUpdateRate { get; set; }

        /// <summary>
        /// OPC items
        /// </summary>
        public IEnumerable<OpcItemData> OpcItems { get; set; }
    }
}