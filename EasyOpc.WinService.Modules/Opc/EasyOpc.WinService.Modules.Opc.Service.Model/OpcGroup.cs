using EasyOpc.Common.Types;
using System;
using System.Collections.Generic;

namespace EasyOpc.WinService.Modules.Opc.Service.Model
{
    /// <summary>
    /// OPC group
    /// </summary>
    public class OpcGroup : IIdentifiable
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
        public IEnumerable<OpcItem> OpcItems { get; set; }
    }
}
