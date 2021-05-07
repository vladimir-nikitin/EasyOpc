using System;
using System.Collections.Generic;

namespace EasyOpc.WinService.Modules.Opc.Connectors.Contract
{
    public delegate void OpcItemsChanged(IEnumerable<IOpcItem> items);

    public interface IOpcGroup
    {
        Guid Id { get; }

        string Name { get; }

        event OpcItemsChanged OpcItemsChanged;

        void CallOpcItemsChangedEvent(IEnumerable<IOpcItem> items);

        IDictionary<string, IOpcItem> GetOpcItems();
    }
}
