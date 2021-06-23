using System.Collections.Generic;

namespace EasyOpc.WinService.Modules.Opc.Da.Connector.Contract
{
    public interface IDiscoveryItem
    {
        string Id { get; set; }

        string Name { get; set; }

        string AccessPath { get; set; }

        string DataType { get; set; }

        string DataTypeId { get; set; }

        bool HasChildren { get; set; }

        bool HasValue { get; set; }

        IEnumerable<IDiscoveryItem> Childs { get; set; }
    }
}
