using EasyOpc.Contracts.Opc;
using EasyOpc.WinService.Modules.Opc.Connectors.Contract;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EasyOpc.WinService.Hubs
{
    public class OpcGroupHub : Hub
    {
        private static List<OpcGroupHubConnection> Connections { get; } = new List<OpcGroupHubConnection>();

        public void Connect(Guid opcGroupId)
        {
            var connectionId = Context.ConnectionId;

            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcGroupId == opcGroupId);
                if (connection != null)
                {
                    return;
                }

                Connections.Add(new OpcGroupHubConnection(connectionId, opcGroupId, this));
            }
        }

        public void OnConnected(string connectionId, Guid opcGroupId, string error)
        {
            Clients.Client(connectionId).OnConnected(opcGroupId, error);
        }

        public void OnDisconnected(string connectionId, Guid opcGroupId, string error)
        {
            Clients.Client(connectionId).OnDisconnected(opcGroupId, error);
        }
       
        public void OnOpcItemsChanged(string connectionId, Guid opcGroupId, IEnumerable<OpcItemData> opcItems)
        {
            Debug.WriteLine($"[{nameof(OpcGroupHub)}][{nameof(OpcItemsChanged)}] ConnectionId: {connectionId};");
            Clients.Client(connectionId).OnOpcItemsChanged(opcGroupId, opcItems);
        }

        public void Ping(string connectionId)
        {
            Debug.WriteLine($"[{nameof(OpcGroupHub)}][{nameof(Ping)}] ConnectionId: {connectionId};");
            Clients.Client(connectionId).Ping();
        }

        public void Pong(Guid opcGroupId)
        {
            var connectionId = Context.ConnectionId;
            Debug.WriteLine($"[{nameof(OpcGroupHub)}][{nameof(Pong)}] ConnectionId: {connectionId}; opcGroupId: {opcGroupId}");

            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcGroupId == opcGroupId);
                if (connection != null)
                {
                    connection.Pong();
                }
            }
        }

        public void Disconnect(Guid opcGroupId)
        {
            var connectionId = Context.ConnectionId;
            Debug.WriteLine($"[{nameof(OpcGroupHub)}][{nameof(Disconnect)}] ConnectionId: {connectionId}; opcGroupId: {opcGroupId}");

            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcGroupId == opcGroupId);
                if (connection != null)
                {
                    Connections.Remove(connection);
                    connection.Dispose();
                }
            }
        }
    }
}
