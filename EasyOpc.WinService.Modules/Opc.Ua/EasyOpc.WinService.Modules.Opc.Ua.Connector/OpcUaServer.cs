using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using EasyOpc.WinService.Modules.Opc.Ua.Connector.Contract;

namespace EasyOpc.WinService.Modules.Opc.Ua.Connector
{
    public class OpcUaServer : IOpcUaServer
    {
        public Guid Id => throw new NotImplementedException();

        public bool IsConnected => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string Host => throw new NotImplementedException();

        public string UserName => throw new NotImplementedException();

        public string Password => throw new NotImplementedException();

        public Task<IEnumerable<IDiscoveryItem>> BrowseAllAsync(string nodeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IDiscoveryItem>> BrowseAsync(string nodeId)
        {
            throw new NotImplementedException();
        }

        public Task ConnectAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IOpcUaGroup> CreateOpcUaGroupAsync(Guid groupId, string groupName, IEnumerable<IOpcUaItem> items)
        {
            throw new NotImplementedException();
        }

        public Task DisconnectAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IOpcUaGroup> GetOpcUaGroups()
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

        public Task RemoveOpcUaGroupAsync(Guid groupId)
        {
            throw new NotImplementedException();
        }
    }
}
