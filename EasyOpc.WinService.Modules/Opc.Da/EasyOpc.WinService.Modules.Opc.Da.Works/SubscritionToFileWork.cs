using EasyOpc.Common.Constants;
using EasyOpc.Common.Helpers;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.WorksService.Contract;
using EasyOpc.WinService.Modules.Opc.Da.Connector;
using EasyOpc.WinService.Modules.Opc.Da.Connector.Contract;
using EasyOpc.WinService.Modules.Opc.Da.Services.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace EasyOpc.WinService.Modules.Opc.Da.Works
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
        private IOpcDaServersFactory OpcDaServersFactory { get; }

        /// <summary>
        /// OPC.DA servers service
        /// </summary>
        private IOpcDaServersService OpcDaServersService { get; }

        /// <summary>
        /// OPC.DA groups service
        /// </summary>
        private IOpcDaGroupsService OpcDaGroupsService { get; }

        /// <summary>
        /// OPC.DA items service
        /// </summary>
        private IOpcDaItemsService OpcDaItemsService { get; }

        /// <summary>
        /// Settings
        /// </summary>
        private SubscritionToFileWorkSettings Settings { get; }

        /// <summary>
        /// OPC.DA group id
        /// </summary>
        private Guid OpcDaGroupId { get; }

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
        private IOpcDaServer OpcDaServer { get; set; }

        /// <summary>
        /// OPC.DA group
        /// </summary>
        private IOpcDaGroup OpcDaGroup { get; set; }

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

        public SubscritionToFileWork(string name, string launchGroup, Guid opcDaGroupId, string jsonSettings, 
            ILogger logger, IOpcDaServersService opcDaServersService, IOpcDaGroupsService opcDaGroupsService, 
            IOpcDaItemsService opcDaItemsService, IOpcDaServersFactory opcDaServersFactory)
        {
            Name = name;
            LaunchGroup = launchGroup;
            OpcDaGroupId = opcDaGroupId;
            Settings = JsonConvert.DeserializeObject<SubscritionToFileWorkSettings>(jsonSettings);

            Logger = logger;

            OpcDaServersService = opcDaServersService;
            OpcDaGroupsService = opcDaGroupsService;
            OpcDaItemsService = opcDaItemsService;
            OpcDaServersFactory = opcDaServersFactory;
        }

        public async Task StartAsync()
        {
            if (Settings.FileTimespan.TotalMilliseconds == 0)
                return;

            CurrentSaveDateTime = DateTime.Now;
            CurrentEndSaveDateTime = CurrentSaveDateTime + Settings.FileTimespan;

            var opcGroupData = await OpcDaGroupsService.GetByIdAsync(OpcDaGroupId);
            var opcServerData = await OpcDaServersService.GetByIdAsync(opcGroupData.OpcDaServerId);
            var opcItemDatas = await OpcDaItemsService.GetByOpcDaGroupIdAsync(opcGroupData.Id);

            LoggerPrefix = $"[Host: {opcServerData.Host}][OPC.DA server: {opcServerData.Name}][OPC.DA group: {opcGroupData.Name}]";
            Logger.Info($"{LoggerPrefix} Subscription mode is started");

            OpcDaServer = OpcDaServersFactory.Create(opcServerData.Id, opcServerData.Name, opcServerData.Host, opcServerData.Clsid);
            OpcDaGroup = await OpcDaServer.CreateOpcDaGroupAsync(opcGroupData.Id, opcGroupData.Name, opcItemDatas.Select(p => new OpcDaItem
            {
                Id = p.Id,
                Name = p.Name,
                AccessPath = p.AccessPath,
                CanonicalDataType = p.CanonicalDataType,
                CanonicalDataTypeId = p.CanonicalDataTypeId,
                ReqDataType = p.ReqDataType,
                ReadOnly = p.ReadOnly,
            }));

            if (OpcDaGroup == null)
                return;

            OpcDaGroup.OpcDaItemsChanged += OpcItemsChanged;

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
            if (OpcDaGroup != null)
            {
                OpcDaGroup.OpcDaItemsChanged -= OpcItemsChanged;
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

        private void OpcItemsChanged(IEnumerable<IOpcDaItem> items)
        {
            if (items == null || !items.Any())
                return;


            if (CurrentSaveDateTime + Settings.FileTimespan < DateTime.Now)
            {
                CurrentSaveDateTime = DateTime.Now;
                CurrentEndSaveDateTime = CurrentSaveDateTime + Settings.FileTimespan;
            }

            var filePath = $"{Settings.FolderPath}\\{OpcDaGroup.Name}_{CurrentSaveDateTime.ToString($"{WellKnownCodes.TimeFormat}_{WellKnownCodes.DateFormat}")}_{CurrentEndSaveDateTime.ToString($"{WellKnownCodes.TimeFormat}_{WellKnownCodes.DateFormat}")}.csv";
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
        private string GetString(IOpcDaItem item)
        {
            return string.Join(WellKnownCodes.SystemStringSeparator.ToString(),
                  GetCsvString(OpcDaGroup.Name),
                  GetCsvString(item.Name),
                  GetCsvString(string.IsNullOrEmpty(item.AccessPath) ? " " : item.AccessPath),
                  string.IsNullOrEmpty(item.CanonicalDataTypeId) ? "0" : item.CanonicalDataTypeId,
                  item.ReadOnly,
                  "0",
                  1000, //OpcGroup.ReqUpdateRate,
                  "0",
                  GetCsvString(item.Value?.Replace('"', '`')),
                  GetCsvString(item.Quality?.Replace("\"", "").Replace("'", "")),
                  GetCsvString(item.Timestamp?.ToUniversalTime().ToString(WellKnownCodes.ExportDateTimeFormat)),
                  GetCsvString(item.Timestamp?.ToLocalTime().ToString(WellKnownCodes.ExportDateTimeFormat)),
                  string.IsNullOrEmpty(item.CanonicalDataType) ? "Unknown" : item.CanonicalDataType);
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
                    var fileName = file.Name.Replace(".csv", "").Replace($"{OpcDaGroup.Name}_", "").Substring(20);
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
