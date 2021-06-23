using EasyOpc.Contracts.Opc.Da;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyOpc.WinService.Hubs
{
    public class OpcDaGroupHub : Hub
    {
        private static List<OpcDaGroupHubConnection> Connections { get; } = new List<OpcDaGroupHubConnection>();

        public void Connect(Guid opcDaGroupId)
        {
            var connectionId = Context.ConnectionId;

            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcDaGroupId == opcDaGroupId);
                if (connection != null)
                {
                    return;
                }

                Connections.Add(new OpcDaGroupHubConnection(connectionId, opcDaGroupId, this));
            }
        }

        public void Subscribe(Guid opcDaGroupId, Guid[] opcDaItemIds)
        {
            var connectionId = Context.ConnectionId;
            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcDaGroupId == opcDaGroupId);
                if (connection != null)
                {
                    connection.Subscribe(opcDaItemIds);
                }
            }
        }

        public void OnConnected(string connectionId, Guid opcDaGroupId, string error)
        {
            Clients.Client(connectionId).OnConnected(opcDaGroupId, error);
        }

        public void OnDisconnected(string connectionId, Guid opcDaGroupId, string error)
        {
            Clients.Client(connectionId).OnDisconnected(opcDaGroupId, error);
        }
       
        public void OnOpcDaItemsChanged(string connectionId, Guid opcDaGroupId, IEnumerable<OpcDaItemData> opcDaItems)
        {
            if(opcDaItems == null || !opcDaItems.Any())
                return;
            
            Clients.Client(connectionId).OnOpcDaItemsChanged(opcDaGroupId, opcDaItems);
        }

        public void Ping(string connectionId)
        {
            Clients.Client(connectionId).Ping();
        }

        public void Pong(Guid opcDaGroupId)
        {
            var connectionId = Context.ConnectionId;
            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcDaGroupId == opcDaGroupId);
                if (connection != null)
                {
                    connection.Pong();
                }
            }
        }

        public void Disconnect(Guid opcDaGroupId)
        {
            var connectionId = Context.ConnectionId;
            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcDaGroupId == opcDaGroupId);
                if (connection != null)
                {
                    Connections.Remove(connection);
                    connection.Dispose();
                }
            }
        }
    }
}
