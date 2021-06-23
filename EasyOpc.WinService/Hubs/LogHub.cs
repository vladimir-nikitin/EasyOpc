using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Linq;

namespace EasyOpc.WinService.Hubs
{
    public class LogHub : Hub
    {
        private static List<LogHubConnection> Connections { get; } = new List<LogHubConnection>();

        public void Connect()
        {
            var connectionId = Context.ConnectionId;

            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId);
                if (connection != null)
                {
                    return;
                }

                Connections.Add(new LogHubConnection(connectionId, this));

                Clients.Client(connectionId).Connected();
            }
        }

        public void Disconnect()
        {
            var connectionId = Context.ConnectionId;

            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId);
                if (connection != null)
                {
                    Connections.Remove(connection);
                    connection.Dispose();
                }
            }
        }

        public void Ping()
        {
            Clients.Client(Context.ConnectionId).Ping();
        }

        public void Pong()
        {
            var connectionId = Context.ConnectionId;
            lock (Connections)
            {
                var connection = Connections.FirstOrDefault(p => p.ConnectionId == connectionId);
                if (connection != null)
                {
                    connection.Pong();
                }
            }
        }

        public void HubConnected(IEnumerable<string> records)
        {
            Clients.Client(Context.ConnectionId).HubConnected(records);
        }

        public void RecordsAdded(IEnumerable<string> records)
        {
            Clients.Client(Context.ConnectionId).RecordsAdded(records);
        }
    }
}
