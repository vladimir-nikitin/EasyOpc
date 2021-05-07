using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Modules.Opc.Connectors.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Connectors.Ua
{
    public class OpcUaServerFactory
    {
        private ILogger Logger { get; }

        private List<IOpcServer> Servers { get; set; } = new List<IOpcServer>();

        public OpcUaServerFactory(ILogger logger)
        {
            Logger = logger;
        }

        public IOpcServer Create(Guid id, string name, string host, string user, string password)
        {
            lock (Servers)
            {
                var server = Servers.FirstOrDefault(s => s.Id == id);
                if (server == null)
                {
                    server = new OpcUaServer(Logger, id, name, host, user, password);
                    Servers.Add(server);
                }

                if (Servers.Count == 1)
                {
                    Task.Run(async () => {

                        await Task.Delay(15000);

                        for (int i = 0; i < Servers.Count; i++)
                        {
                            var srv = Servers[i];
                            var groups = srv.GetOpcGroups();
                            if(groups.Count() < 1)
                            {
                                continue;
                            }

                            var ping = await srv.PingAsync();
                            if (!ping)
                            {
                                await srv.ReconnectAsync();
                            }
                        }

                        await Task.Delay(15000);
                    });
                }

                return server;
            }
        }
    }
}
