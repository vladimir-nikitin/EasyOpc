using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Da.Connector.Contract
{
    public interface IOpcDaServer
    {
        Guid Id { get; }

        bool IsConnected { get; }

        string Name { get; }

        string Host { get; }

        string CLSID { get; }

        Task<bool> PingAsync();

        Task ConnectAsync();

        Task DisconnectAsync();

        Task ReconnectAsync();

        IEnumerable<IOpcDaGroup> GetOpcDaGroups();

        Task<IOpcDaGroup> CreateOpcDaGroupAsync(Guid groupId, string groupName, IEnumerable<IOpcDaItem> items);

        Task RemoveOpcDaGroupAsync(Guid groupId);

        Task<IEnumerable<IDiscoveryItem>> BrowseAsync(string item);

        Task<IEnumerable<IDiscoveryItem>> BrowseAllAsync(string item);
    }
}
