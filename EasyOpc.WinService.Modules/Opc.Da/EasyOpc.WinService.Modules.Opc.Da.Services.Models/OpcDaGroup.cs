using EasyOpc.Common.Types;
using System;
using System.Collections.Generic;

namespace EasyOpc.WinService.Modules.Opc.Da.Services.Models
{
    /// <summary>
    /// OPC.DA group
    /// </summary>
    public class OpcDaGroup : IIdentifiable
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// OPC.DA server ID
        /// </summary>
        public Guid OpcDaServerId { get; set; }

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
