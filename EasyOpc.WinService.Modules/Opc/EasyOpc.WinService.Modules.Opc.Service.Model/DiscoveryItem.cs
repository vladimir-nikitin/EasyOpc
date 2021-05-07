using System;
using System.Collections.Generic;

namespace EasyOpc.WinService.Modules.Opc.Service.Model
{
    public class DiscoveryItem
    {
        public Guid OpcServerId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string AccessPath { get; set; }

        public string DataType { get; set; }

        public string DataTypeId { get; set; }

        public bool HasChildren { get; set; }

        public IEnumerable<DiscoveryItem> Childs { get; set; }
    }
}
