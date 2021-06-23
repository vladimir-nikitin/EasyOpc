using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Ua.Connector.Contract
{
    public interface IOpcUaServer
    {
        Guid Id { get; }

        bool IsConnected { get; }

        string Name { get; }

        string Host { get; }

        string UserName { get; }

        string Password { get; }

        Task<bool> PingAsync();

        Task ConnectAsync();

        Task DisconnectAsync();

        Task ReconnectAsync();

        IEnumerable<IOpcUaGroup> GetOpcUaGroups();

        Task<IOpcUaGroup> CreateOpcUaGroupAsync(Guid groupId, string groupName, IEnumerable<IOpcUaItem> items);

        Task RemoveOpcUaGroupAsync(Guid groupId);

        Task<IEnumerable<IDiscoveryItem>> BrowseAsync(string nodeId);

        Task<IEnumerable<IDiscoveryItem>> BrowseAllAsync(string nodeId);
    }
}
