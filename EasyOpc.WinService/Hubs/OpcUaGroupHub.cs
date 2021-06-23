using EasyOpc.Contracts.Opc.Ua;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyOpc.WinService.Hubs
{
    public class OpcUaGroupHub : Hub
    {
        private static List<OpcUaGroupHubConnection> Connections { get; } = new List<OpcUaGroupHubConnection>();

        public void Connect(Guid opcUaGroupId)
        {
            var connectionId = Context.ConnectionId;

            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcUaGroupId == opcUaGroupId);
                if (connection != null)
                {
                    return;
                }

                Connections.Add(new OpcUaGroupHubConnection(connectionId, opcUaGroupId, this));
            }
        }

        public void Subscribe(Guid opcUaGroupId, Guid[] opcUaItemIds)
        {
            var connectionId = Context.ConnectionId;
            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcUaGroupId == opcUaGroupId);
                if (connection != null)
                {
                    connection.Subscribe(opcUaItemIds);
                }
            }
        }

        public void OnConnected(string connectionId, Guid opcUaGroupId, string error)
        {
            Clients.Client(connectionId).OnConnected(opcUaGroupId, error);
        }

        public void OnDisconnected(string connectionId, Guid opcUaGroupId, string error)
        {
            Clients.Client(connectionId).OnDisconnected(opcUaGroupId, error);
        }
       
        public void OnOpcItemsChanged(string connectionId, Guid opcUaGroupId, IEnumerable<OpcUaItemData> opcUaItems)
        {
            if(opcUaItems == null || !opcUaItems.Any())
                return;
            
            Clients.Client(connectionId).OnOpcUaItemsChanged(opcUaGroupId, opcUaItems);
        }

        public void Ping(string connectionId)
        {
            Clients.Client(connectionId).Ping();
        }

        public void Pong(Guid opcUaGroupId)
        {
            var connectionId = Context.ConnectionId;
            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcUaGroupId == opcUaGroupId);
                if (connection != null)
                {
                    connection.Pong();
                }
            }
        }

        public void Disconnect(Guid opcUaGroupId)
        {
            var connectionId = Context.ConnectionId;
            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId && p.OpcUaGroupId == opcUaGroupId);
                if (connection != null)
                {
                    Connections.Remove(connection);
                    connection.Dispose();
                }
            }
        }
    }
}
