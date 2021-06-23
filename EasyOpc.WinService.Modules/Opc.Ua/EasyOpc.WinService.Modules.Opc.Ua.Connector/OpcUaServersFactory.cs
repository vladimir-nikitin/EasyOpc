using EasyOpc.WinService.Modules.Opc.Ua.Connector.Contract;
using System;

namespace EasyOpc.WinService.Modules.Opc.Ua.Connector
{
    public class OpcUaServersFactory : IOpcUaServersFactory
    {
        public IOpcUaServer Create(Guid id, string name, string host, string user, string password)
        {
            throw new NotImplementedException();
        }
    }
}
