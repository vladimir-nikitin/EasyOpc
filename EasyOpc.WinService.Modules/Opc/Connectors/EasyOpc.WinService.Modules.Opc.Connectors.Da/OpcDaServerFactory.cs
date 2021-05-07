using EasyOpc.Common.Opc;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Modules.Opc.Connectors.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Connectors.Da
{
    public class OpcDaServerFactory
    {
        private ILogger Logger { get; }

        private List<IOpcServer> OpcDaServers { get; set; } = new List<IOpcServer>();

        public OpcDaServerFactory(ILogger logger)
        {
            Logger = logger;
        }

        public IOpcServer Create(Guid id, string name, string host, string clsid = null)
        {
            lock (OpcDaServers)
            {
                var server = OpcDaServers.FirstOrDefault(s => s.Id == id);
                if (server == null)
                {
                    server = new OpcDaServer(Logger, id, name, host, clsid);
                    OpcDaServers.Add(server);
                }

                if (OpcDaServers.Count == 1)
                {
                    Task.Run(async () => {

                        await Task.Delay(15000);

                        for (int i = 0; i < OpcDaServers.Count; i++)
                        {
                            var srv = OpcDaServers[i];
                            var groups = srv.GetOpcGroups();
                            if (groups.Count() < 1)
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
