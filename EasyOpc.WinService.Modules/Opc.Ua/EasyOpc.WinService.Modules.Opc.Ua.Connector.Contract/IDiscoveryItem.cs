using System.Collections.Generic;

namespace EasyOpc.WinService.Modules.Opc.Ua.Connector.Contract
{
    public interface IDiscoveryItem
    {
        string Id { get; set; }

        string Name { get; set; }

        string NodeId { get; set; }

        bool HasChildren { get; set; }

        bool HasValue { get; set; }

        IEnumerable<IDiscoveryItem> Childs { get; set; }
    }
}
