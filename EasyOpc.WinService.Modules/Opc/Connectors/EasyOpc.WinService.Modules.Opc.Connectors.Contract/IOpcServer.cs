using EasyOpc.Common.Opc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Connectors.Contract
{
    public interface IOpcServer
    {
        Guid Id { get; }

        bool IsConnected { get; }

        string Name { get; }

        string Host { get; }

        OpcServerType Type { get; }

        Task<bool> PingAsync();

        Task ConnectAsync();

        Task DisconnectAsync();

        Task ReconnectAsync();

        IEnumerable<IOpcGroup> GetOpcGroups();

        Task<IOpcGroup> CreateOpcGroupAsync(Guid groupId, string groupName, IEnumerable<IOpcItem> items);

        Task RemoveOpcGroupAsync(Guid groupId);

        Task<IEnumerable<IDiscoveryItem>> BrowseAsync(string item);

        Task<IEnumerable<IDiscoveryItem>> BrowseAllAsync(string item);
    }
}
