using System;

namespace EasyOpc.WinService.Modules.Opc.Da.Connector.Contract
{
    public interface IOpcDaServersFactory
    {
        IOpcDaServer Create(Guid id, string name, string host, string clsid = null);
    }
}
