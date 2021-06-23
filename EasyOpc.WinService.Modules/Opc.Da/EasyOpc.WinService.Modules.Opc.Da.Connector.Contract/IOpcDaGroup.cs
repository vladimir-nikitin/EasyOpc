using System;
using System.Collections.Generic;

namespace EasyOpc.WinService.Modules.Opc.Da.Connector.Contract
{
    public delegate void OpcDaItemsChanged(IEnumerable<IOpcDaItem> items);

    public interface IOpcDaGroup
    {
        Guid Id { get; }

        string Name { get; }

        event OpcDaItemsChanged OpcDaItemsChanged;

        void CallOpcItemsChangedEvent(IEnumerable<IOpcDaItem> items);

        IDictionary<string, IOpcDaItem> GetOpcDaItems();
    }
}
