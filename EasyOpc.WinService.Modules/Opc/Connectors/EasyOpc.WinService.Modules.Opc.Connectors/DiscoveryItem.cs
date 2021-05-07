using EasyOpc.WinService.Modules.Opc.Connectors.Contract;
using System.Collections.Generic;

namespace EasyOpc.WinService.Modules.Opc.Connectors
{
    public class DiscoveryItem : IDiscoveryItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string AccessPath { get; set; }

        public string DataType { get; set; }

        public string DataTypeId { get; set; }

        public bool HasChildren { get; set; }

        public IEnumerable<IDiscoveryItem> Childs { get; set; }
    }
}
