using System;
using System.Collections.Generic;

namespace EasyOpc.Contracts.Opc
{
    public class DiscoveryItemData
    {
        public Guid OpcServerId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string AccessPath { get; set; }

        public string DataType { get; set; }

        public string DataTypeId { get; set; }

        public bool HasChildren { get; set; }

        public IEnumerable<DiscoveryItemData> Childs { get; set; }
    }
}
