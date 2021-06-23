using EasyOpc.WinService.Core.Logger.Contract;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyOpc.WinService.Hubs
{

    public class LogHubConnection : IDisposable
    {
        private LogHub LogHub { get; }

        private ILogger Logger { get; }

        public string ConnectionId { get; }

        private int PingDelay { get; } = 5000;

        private int PingCounter { get; set; }

        private CancellationTokenSource CancellationTokenSource { get; }

        public LogHubConnection(string connectionId, LogHub logHub)
        {
            LogHub = logHub;
            ConnectionId = connectionId;
            CancellationTokenSource = new CancellationTokenSource();

            Logger = (ILogger)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILogger));
            LogHub.HubConnected(Logger.GetCacheLogs());
            Logger.RecordsAdded += RecordsAdded;

            var token = CancellationTokenSource.Token;
            Task.Run(async () =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested) return;

                    if (PingCounter > 5)
                    {
                        Logger.RecordsAdded -= RecordsAdded;
                        return;
                    }

                    await Task.Delay(PingDelay, token);

                    if (token.IsCancellationRequested) return;

                    LogHub.Ping();

                    if (token.IsCancellationRequested) return;

                    PingCounter++;
                }
            });
        }

        public void Pong()
        {
            PingCounter--;
        }

        public void Dispose()
        {
            try
            {
                Logger.RecordsAdded -= RecordsAdded;
            }
            catch { }

            try
            {
                CancellationTokenSource?.Cancel();
            }
            catch { }
        }

        private void RecordsAdded(IEnumerable<string> records)
        {
            LogHub.RecordsAdded(records);
        }
    }
}
