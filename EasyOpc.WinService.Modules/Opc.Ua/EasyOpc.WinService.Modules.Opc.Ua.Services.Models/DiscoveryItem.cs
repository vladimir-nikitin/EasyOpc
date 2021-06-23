using System;
using System.Collections.Generic;

namespace EasyOpc.WinService.Modules.Opc.Ua.Services.Models
{
    public class DiscoveryItem
    {
        public Guid OpcUaServerId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string NodeId { get; set; }

        public bool HasChildren { get; set; }

        public bool HasValue { get; set; }

        public IEnumerable<DiscoveryItem> Childs { get; set; }
    }
}
