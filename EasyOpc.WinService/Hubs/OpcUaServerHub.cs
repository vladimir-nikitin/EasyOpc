using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyOpc.WinService.Hubs
{
    public class OpcUaServerHub : Hub
    {
        private static List<OpcUaServerHubConnection> Connections { get; } = new List<OpcUaServerHubConnection>();

        public void Connect(Guid opcUaServerId)
        {
            var connectionId = Context.ConnectionId;

            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcUaServerId == opcUaServerId);
                if (connection != null)
                {
                    return;
                }

                Connections.Add(new OpcUaServerHubConnection(connectionId, opcUaServerId, this));
            }
        }

        public void Disconnect(Guid opcUaServerId)
        {
            var connectionId = Context.ConnectionId;
            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcUaServerId == opcUaServerId);
                if (connection != null)
                {
                    Connections.Remove(connection);
                    connection.Dispose();
                }
            }
        }

        public void OnConnected(string connectionId, Guid opcUaServerId, string error)
        {
            Clients.Client(connectionId).OnConnected(opcUaServerId, error);
        }

        public void OnDisconnected(string connectionId, Guid opcUaGroupId, string error)
        {
            Clients.Client(connectionId).OnDisconnected(opcUaGroupId, error);
        }

        public void Ping(string connectionId)
        {
            Clients.Client(connectionId).Ping();
        }

        public void Pong(Guid opcUaServerId)
        {
            var connectionId = Context.ConnectionId;
            
            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcUaServerId == opcUaServerId);
                if (connection != null)
                {
                    connection.Pong();
                }
            }
        }

    }
}
