using EasyOpc.WinService.Modules.Opc.Da.Connector.Contract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Da.Connector
{
    public class OpcDaServer : IOpcDaServer
    {
        public Guid Id => throw new NotImplementedException();

        public bool IsConnected => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string Host => throw new NotImplementedException();

        public string CLSID => throw new NotImplementedException();

        public Task<IEnumerable<IDiscoveryItem>> BrowseAllAsync(string item)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IDiscoveryItem>> BrowseAsync(string item)
        {
            throw new NotImplementedException();
        }

        public Task ConnectAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IOpcDaGroup> CreateOpcDaGroupAsync(Guid groupId, string groupName, IEnumerable<IOpcDaItem> items)
        {
            throw new NotImplementedException();
        }

        public Task DisconnectAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IOpcDaGroup> GetOpcDaGroups()
        {
            throw new NotImplementedException();
        }

        public Task<bool> PingAsync()
        {
            throw new NotImplementedException();
        }

        public Task ReconnectAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveOpcDaGroupAsync(Guid groupId)
        {
            throw new NotImplementedException();
        }
    }
}
