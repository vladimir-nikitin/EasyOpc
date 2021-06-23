using System;

namespace EasyOpc.WinService.Modules.Opc.Ua.Connector.Contract
{
    public interface IOpcUaServersFactory
    {
        IOpcUaServer Create(Guid id, string name, string host, string user, string password);
    }
}
