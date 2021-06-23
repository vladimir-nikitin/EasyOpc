using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyOpc.WinService.Hubs
{
    public class OpcDaServerHub : Hub
    {
        private static List<OpcDaServerHubConnection> Connections { get; } = new List<OpcDaServerHubConnection>();

        public void Connect(Guid opcDaServerId)
        {
            var connectionId = Context.ConnectionId;

            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcDaServerId == opcDaServerId);
                if (connection != null)
                {
                    return;
                }

                Connections.Add(new OpcDaServerHubConnection(connectionId, opcDaServerId, this));
            }
        }

        public void Disconnect(Guid opcDaServerId)
        {
            var connectionId = Context.ConnectionId;
            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcDaServerId == opcDaServerId);
                if (connection != null)
                {
                    Connections.Remove(connection);
                    connection.Dispose();
                }
            }
        }

        public void OnConnected(string connectionId, Guid opcDaServerId, string error)
        {
            Clients.Client(connectionId).OnConnected(opcDaServerId, error);
        }

        public void OnDisconnected(string connectionId, Guid opcDaGroupId, string error)
        {
            Clients.Client(connectionId).OnDisconnected(opcDaGroupId, error);
        }

        public void Ping(string connectionId)
        {
            Clients.Client(connectionId).Ping();
        }

        public void Pong(Guid opcDaServerId)
        {
            var connectionId = Context.ConnectionId;
            
            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcDaServerId == opcDaServerId);
                if (connection != null)
                {
                    connection.Pong();
                }
            }
        }

    }
}
