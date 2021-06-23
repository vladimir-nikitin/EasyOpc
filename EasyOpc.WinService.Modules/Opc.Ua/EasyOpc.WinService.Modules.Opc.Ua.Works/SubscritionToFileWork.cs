using EasyOpc.Common.Constants;
using EasyOpc.Common.Helpers;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.WorksService.Contract;
using EasyOpc.WinService.Modules.Opc.Ua.Connector;
using EasyOpc.WinService.Modules.Opc.Ua.Connector.Contract;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace EasyOpc.WinService.Modules.Opc.Ua.Works
{
    /// <summary>
    /// Subscrition work
    /// </summary>
    public class SubscritionToFileWork : IWork
    {
        /// <summary>
        /// Logger
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// OPC.DA servers factory
        /// </summary>
        private IOpcUaServersFactory OpcUaServersFactory { get; }

        /// <summary>
        /// OPC.DA servers service
        /// </summary>
        private IOpcUaServersService OpcUaServersService { get; }

        /// <summary>
        /// OPC.DA groups service
        /// </summary>
        private IOpcUaGroupsService OpcUaGroupsService { get; }

        /// <summary>
        /// OPC.DA items service
        /// </summary>
        private IOpcUaItemsService OpcUaItemsService { get; }

        /// <summary>
        /// Settings
        /// </summary>
        private SubscritionToFileWorkSettings Settings { get; }

        /// <summary>
        /// OPC.DA group id
        /// </summary>
        private Guid OpcUaGroupId { get; }

        /// <summary>
        /// Logs prefix
        /// </summary>
        private string LoggerPrefix { get; set; }

        /// <summary>
        /// Timer
        /// </summary>
        private System.Timers.Timer Timer { get; set; }

        /// <summary>
        /// Save date time
        /// </summary>
        private DateTime CurrentSaveDateTime { get; set; }

        /// <summary>
        /// Save date time
        /// </summary>
        private DateTime CurrentEndSaveDateTime { get; set; }

        /// <summary>
        /// OPC.DA server
        /// </summary>
        private IOpcUaServer OpcUaServer { get; set; }

        /// <summary>
        /// OPC.DA group
        /// </summary>
        private IOpcUaGroup OpcUaGroup { get; set; }

        /// <summary>
        /// Buffer
        /// </summary>
        private BufferFile BufferFile { get; set; }

        /// <summary>
        /// Work name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Launch group
        /// </summary>
        public string LaunchGroup { get; }

        public SubscritionToFileWork(string name, string launchGroup, Guid opcUaGroupId, string jsonSettings, 
            ILogger logger, IOpcUaServersService opcUaServersService, IOpcUaGroupsService opcUaGroupsService, 
            IOpcUaItemsService opcUaItemsService, IOpcUaServersFactory opcUaServersFactory)
        {
            Name = name;
            LaunchGroup = launchGroup;
            OpcUaGroupId = opcUaGroupId;
            Settings = JsonConvert.DeserializeObject<SubscritionToFileWorkSettings>(jsonSettings);

            Logger = logger;

            OpcUaServersService = opcUaServersService;
            OpcUaGroupsService = opcUaGroupsService;
            OpcUaItemsService = opcUaItemsService;
            OpcUaServersFactory = opcUaServersFactory;
        }

        public async Task StartAsync()
        {
            if (Settings.FileTimespan.TotalMilliseconds == 0)
                return;

            CurrentSaveDateTime = DateTime.Now;
            CurrentEndSaveDateTime = CurrentSaveDateTime + Settings.FileTimespan;

            var opcGroupData = await OpcUaGroupsService.GetByIdAsync(OpcUaGroupId);
            var opcServerData = await OpcUaServersService.GetByIdAsync(opcGroupData.OpcUaServerId);
            var opcItemDatas = await OpcUaItemsService.GetByOpcUaGroupIdAsync(opcGroupData.Id);

            LoggerPrefix = $"[Host: {opcServerData.Host}][OPC.DA server: {opcServerData.Name}][OPC.DA group: {opcGroupData.Name}]";
            Logger.Info($"{LoggerPrefix} Subscription mode is started");

            OpcUaServer = OpcUaServersFactory.Create(opcServerData.Id, opcServerData.Name, opcServerData.Host, opcServerData.UserName, opcServerData.Password);
            OpcUaGroup = await OpcUaServer.CreateOpcUaGroupAsync(opcGroupData.Id, opcGroupData.Name, opcItemDatas.Select(p => new OpcUaItem
            {
                Id = p.Id,
                Name = p.Name,
                NodeId = p.NodeId,
            }));

            if (OpcUaGroup == null)
                return;

            OpcUaGroup.OpcUaItemsChanged += OpcItemsChanged;

            if (Settings.HistoryRetentionTimespan.TotalMilliseconds > 0)
            {
                Timer = new Timer(Settings.HistoryRetentionTimespan.TotalMilliseconds);
                Timer.Enabled = true;
                Timer.AutoReset = true;
                Timer.Elapsed += OnTimerElapsed;
                Timer.Start();
            }
        }

        public Task StopAsync()
        {
            if (OpcUaGroup != null)
            {
                OpcUaGroup.OpcUaItemsChanged -= OpcItemsChanged;
            }

            if (Timer != null)
            {
                Timer.Stop();
                Timer.Elapsed -= OnTimerElapsed;
                Timer = null;
            }

            Logger.Info($"{LoggerPrefix} Subscription mode is stopped");

            return Task.CompletedTask;
        }

        private void OpcItemsChanged(IEnumerable<IOpcUaItem> items)
        {
            if (items == null || !items.Any())
                return;


            if (CurrentSaveDateTime + Settings.FileTimespan < DateTime.Now)
            {
                CurrentSaveDateTime = DateTime.Now;
                CurrentEndSaveDateTime = CurrentSaveDateTime + Settings.FileTimespan;
            }

            var filePath = $"{Settings.FolderPath}\\{OpcUaGroup.Name}_{CurrentSaveDateTime.ToString($"{WellKnownCodes.TimeFormat}_{WellKnownCodes.DateFormat}")}_{CurrentEndSaveDateTime.ToString($"{WellKnownCodes.TimeFormat}_{WellKnownCodes.DateFormat}")}.csv";
            if (BufferFile != null)
            {
                if (BufferFile.Path == filePath)
                {
                    lock (BufferFile)
                    {
                        foreach (var item in items)
                        {
                            BufferFile.WriteLine(GetString(item));
                        }
                    }
                }
                else
                {
                    BufferFile.Dispose();
                    BufferFile = new BufferFile(filePath);
                    lock (BufferFile)
                    {
                        foreach (var item in items)
                        {
                            BufferFile.WriteLine(GetString(item));
                        }
                    }
                }
            }
            else
            {
                BufferFile = new BufferFile(filePath);
                lock (BufferFile)
                {
                    foreach (var item in items)
                    {
                        BufferFile.WriteLine(GetString(item));
                    }
                }
            }
        }

        /// <summary>
        /// Get CSV string
        /// </summary>
        /// <param name="item">OPC item</param>
        /// <returns>String</returns>
        private string GetString(IOpcUaItem item)
        {
            return string.Join(WellKnownCodes.SystemStringSeparator.ToString(),
                  GetCsvString(OpcUaGroup.Name),
                  GetCsvString(item.Name),
                  "",
                  "0",
                  "0",
                  "0",
                  1000, //OpcGroup.ReqUpdateRate,
                  "0",
                  GetCsvString(item.Value?.Replace('"', '`')),
                  GetCsvString("Good Non-Specific"),
                  GetCsvString(item.Timestamp?.ToUniversalTime().ToString(WellKnownCodes.ExportDateTimeFormat)),
                  GetCsvString(item.Timestamp?.ToLocalTime().ToString(WellKnownCodes.ExportDateTimeFormat)),
                  "Unknown");
        }

        /// <summary>
        /// Convert string to CSV string
        /// </summary>
        /// <param name="content">String</param>
        /// <returns>CSV string</returns>
        private string GetCsvString(string content) => $"\"{content}\"";

        private void OnTimerElapsed(object sender, ElapsedEventArgs e) => GarbageCollect();

        private void GarbageCollect()
        {
            var directoryInfo = new DirectoryInfo(Settings.FolderPath);
            directoryInfo?.GetFiles()?.ToList()?.ForEach(file =>
            {
                try
                {
                    var fileName = file.Name.Replace(".csv", "").Replace($"{OpcUaGroup.Name}_", "").Substring(20);
                    var dateTime = DateTime.ParseExact(fileName, $"{WellKnownCodes.TimeFormat}_{WellKnownCodes.DateFormat}", null);
                    if (dateTime + Settings.HistoryRetentionTimespan < DateTime.Now)
                    {
                        file.Delete();
                    }
                }
                catch { }
            });
        }
    }
}
