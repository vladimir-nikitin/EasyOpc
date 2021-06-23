using System;
using System.Collections.Generic;

namespace EasyOpc.WinService.Modules.Opc.Ua.Connector.Contract
{
    public delegate void OpcUaItemsChanged(IEnumerable<IOpcUaItem> items);

    public interface IOpcUaGroup
    {
        Guid Id { get; }

        string Name { get; }

        event OpcUaItemsChanged OpcUaItemsChanged;

        void CallOpcItemsChangedEvent(IEnumerable<IOpcUaItem> items);

        IDictionary<string, IOpcUaItem> GetOpcUaItems();
    }
}
