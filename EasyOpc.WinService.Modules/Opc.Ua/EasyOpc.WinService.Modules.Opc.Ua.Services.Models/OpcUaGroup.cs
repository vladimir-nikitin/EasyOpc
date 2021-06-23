using EasyOpc.Common.Types;
using System;
using System.Collections.Generic;

namespace EasyOpc.WinService.Modules.Opc.Ua.Services.Models
{
    /// <summary>
    /// OPC.UA group
    /// </summary>
    public class OpcUaGroup : IIdentifiable
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// OPC.UA server ID
        /// </summary>
        public Guid OpcUaServerId { get; set; }

        /// <summary>
        /// Group name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Recommended update period
        /// </summary>
        public int ReqUpdateRate { get; set; }
    }
}
