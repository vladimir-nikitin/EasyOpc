using EasyOpc.Common.Constant;
using EasyOpc.Common.Opc;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Worker.Base;
using EasyOpc.WinService.Modules.Opc.Connectors.Contract;
using EasyOpc.WinService.Modules.Opc.Connectors.Da;
using EasyOpc.WinService.Modules.Opc.Connectors.Ua;
using EasyOpc.WinService.Modules.Opc.Service.Contract;
using EasyOpc.WinService.Modules.Opc.Service.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using OpcItem = EasyOpc.WinService.Modules.Opc.Connectors.OpcItem;
using WorkType = EasyOpc.WinService.Core.Worker.Model.Work;

namespace EasyOpc.WinService.Modules.Opc.Workers.History
{
    /// <summary>
    /// Save history worker
    /// </summary>
    public class Worker : BaseWorker<HistoryWorkSetting>
    {
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
        /// Object for lock
        /// </summary>
        private object LockObject { get; } = new object();

        /// <summary>
        /// OPC.DA factory
        /// </summary>
        private OpcDaServerFactory OpcDaServerFactory { get; }

        /// <summary>
        /// OPC.UA factory
        /// </summary>
        private OpcUaServerFactory OpcUaServerFactory { get; }

        /// <summary>
        /// OPC servers service
        /// </summary>
        private IOpcServerService OpcServerService { get; }

        /// <summary>
        /// OPC groups service
        /// </summary>
        private IOpcGroupService OpcGroupService { get; }

        /// <summary>
        /// OPC items service
        /// </summary>
        private IOpcItemService OpcItemService { get; }

        /// <summary>
        /// OPC server
        /// </summary>
        private IOpcServer OpcServer { get; set; }

        /// <summary>
        /// OPC group
        /// </summary>
        private IOpcGroup OpcGroup { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Worker(ILogger logger, IOpcServerService opcServerService, IOpcGroupService opcGroupService, IOpcItemService opcItemService,
            OpcDaServerFactory opcDaServerFactory, OpcUaServerFactory opcUaServerFactory) : base(logger)
        {
            OpcServerService = opcServerService;
            OpcGroupService = opcGroupService;
            OpcItemService = opcItemService;

            OpcDaServerFactory = opcDaServerFactory;
            OpcUaServerFactory = opcUaServerFactory;
        }

        /// <summary>
        /// <see cref="BaseWorker.StartAsync(WorkData)"/>
        /// </summary>
        public override async Task StartAsync(WorkType work)
        {
            if (work == null || !work.IsEnabled || !work.ExternalId.HasValue || string.IsNullOrEmpty(work.JsonSettings))
                return;

            SetSetting(work.JsonSettings);

            if (Settings.FileTimespan.TotalMilliseconds == 0)
                return;

            CurrentSaveDateTime = DateTime.Now;
            CurrentEndSaveDateTime = CurrentSaveDateTime + Settings.FileTimespan;

            var opcGroupData = await OpcGroupService.GetByIdAsync(work.ExternalId.Value);
            var opcServerData = await OpcServerService.GetByIdAsync(opcGroupData.OpcServerId);
            var opcItemDatas = await OpcItemService.GetByOpcGroupIdAsync(opcGroupData.Id);

            LoggerPrefix = $"[Host: {opcServerData.Host}][OPC server: {opcServerData.Name}][OPC group: {opcGroupData.Name}]";
            Logger.Info($"{LoggerPrefix} Subscription mode is started");

            if (opcServerData.Type == OpcServerType.DA)
            {
                OpcServer = OpcDaServerFactory.Create(opcServerData.Id, opcServerData.Name, opcServerData.Host, opcServerData.JsonSettings);
            }
            else
            {
                var settings = string.IsNullOrEmpty(opcServerData.JsonSettings) ? null : JsonConvert.DeserializeObject<OpcUaServerSettings>(opcServerData.JsonSettings);
                OpcServer = OpcUaServerFactory.Create(opcServerData.Id, opcServerData.Name, opcServerData.Host, settings?.UserIdentitySetting?.User, settings?.UserIdentitySetting?.Password);
            }

            OpcGroup = await OpcServer.CreateOpcGroupAsync(opcGroupData.Id, opcGroupData.Name, opcItemDatas.Select(p => new OpcItem
            {
                Id = p.Id,
                Name = p.Name,
                AccessPath = p.AccessPath,
                CanonicalDataType = p.CanonicalDataType,
                CanonicalDataTypeId = p.CanonicalDataTypeId,
                ReqDataType = p.ReqDataType,
                ReadOnly = p.ReadOnly,
            }));

            OpcGroup.OpcItemsChanged += OpcItemsChanged;

            if (Settings.HistoryRetentionTimespan.TotalMilliseconds == 0)
                return;

            Timer = new System.Timers.Timer(Settings.HistoryRetentionTimespan.TotalMilliseconds);
            Timer.Enabled = true;
            Timer.AutoReset = true;
            Timer.Elapsed += OnTimerElapsed;
            Timer.Start();
        }

        /// <summary>
        /// <see cref="BaseWorker.StopAsync"/>
        /// </summary>
        public override Task StopAsync()
        {
            if (OpcGroup != null)
            {
                OpcGroup.OpcItemsChanged -= OpcItemsChanged;
            }

            if (Timer != null)
            {
                Timer.Stop();
                Timer.Elapsed -= OnTimerElapsed;
            }

            Logger.Info($"{LoggerPrefix} Subscription mode is stopped");

            return Task.CompletedTask;
        }

        private void OpcItemsChanged(IEnumerable<IOpcItem> items)
        {
            if (items == null || !items.Any())
                return;

            var records = new StringBuilder();

            items.ToList()?.ForEach(p => records.AppendLine(GetString(p)));

            //newOpcItems?.ToList()?.ForEach(p => records.AppendLine($"\"{p.Name}\";\"{p.CanonicalDataType}\";\"{p.AccessPath}\";\"{p.ReqDataType}\";\"{p.Value?.Replace('"', '\'')}\";\"{p.Quality?.Replace('"', '\'')}\";\"{p.TimestampUtc}\";\"{p.TimestampLocal}\";"));

            lock (LockObject)
            {
                if (CurrentSaveDateTime + Settings.FileTimespan < DateTime.Now)
                {
                    CurrentSaveDateTime = DateTime.Now;
                    CurrentEndSaveDateTime = CurrentSaveDateTime + Settings.FileTimespan;
                }

                var filePath = $"{Settings.FolderPath}\\{OpcGroup.Name}_{CurrentSaveDateTime.ToString($"{WellKnownCodes.TimeFormat}_{WellKnownCodes.DateFormat}")}_{CurrentEndSaveDateTime.ToString($"{WellKnownCodes.TimeFormat}_{WellKnownCodes.DateFormat}")}.csv";

                try
                {
                    File.AppendAllText(filePath, records.ToString());
                }
                catch { }
            }
        }

        /// <summary>
        /// Get CSV string
        /// </summary>
        /// <param name="item">OPC item</param>
        /// <returns>String</returns>
        private string GetString(IOpcItem item)
        {
            return string.Join(WellKnownCodes.SystemStringSeparator.ToString(),
                  GetCsvString(OpcGroup.Name),
                  GetCsvString(item.Name),
                  GetCsvString(item.AccessPath),
                  item.CanonicalDataTypeId,
                  item.ReadOnly,
                  "0",
                  1000, //OpcGroup.ReqUpdateRate,
                  "0",
                  GetCsvString(item.Value?.Replace('"', '`')),
                  GetCsvString(item.Quality?.Replace("\"", "").Replace("'", "")),
                  GetCsvString(item.TimestampUtc),
                  GetCsvString(item.TimestampLocal),
                  item.CanonicalDataType);
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
                    var fileName = file.Name.Replace(".csv", "").Replace($"{OpcGroup.Name}_", "").Substring(20);
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
