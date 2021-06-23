using EasyOpc.WinService.Modules.Opc.Da.Connector.Contract;
using System;

namespace EasyOpc.WinService.Modules.Opc.Da.Connector
{
    public class OpcDaServersFactory : IOpcDaServersFactory
    {
        public IOpcDaServer Create(Guid id, string name, string host, string clsid = null)
        {
            throw new NotImplementedException();
        }
    }
}
