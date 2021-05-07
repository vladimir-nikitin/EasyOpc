using EasyOpc.Common.Constant;
using EasyOpc.Common.Opc;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Worker.Base;
using EasyOpc.WinService.Modules.Opc.Connectors;
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
using System.Threading.Tasks;
using OpcItem = EasyOpc.WinService.Modules.Opc.Connectors.OpcItem;
using WorkType = EasyOpc.WinService.Core.Worker.Model.Work;

namespace EasyOpc.WinService.Modules.Opc.Workers.Export
{
    /// <summary>
    /// Export to csv worker
    /// </summary>
    public class Worker : BaseWorker<ExportWorkSetting>
    {
        /// <summary>
        /// Logs prefix
        /// </summary>
        private string LoggerPrefix { get; set; }

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
        /// Timer
        /// </summary>
        private System.Timers.Timer Timer { get; set; }

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

            if (Settings.Timespan.TotalMilliseconds == 0)
                return;
            
            var opcGroupData = await OpcGroupService.GetByIdAsync(work.ExternalId.Value);
            var opcServerData = await OpcServerService.GetByIdAsync(opcGroupData.OpcServerId);
            var opcItemDatas = await OpcItemService.GetByOpcGroupIdAsync(opcGroupData.Id);

            LoggerPrefix = $"[Host: {opcServerData.Host}][OPC server: {opcServerData.Name}][OPC group: {opcGroupData.Name}]";
            Logger.Info($"{LoggerPrefix} Export to .CSV is started");

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

            Timer = new System.Timers.Timer(Settings.Timespan.TotalMilliseconds);
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

            Logger.Info($"{LoggerPrefix} Export to .CSV is stopped");

            return Task.CompletedTask;
        }

        private void OpcItemsChanged(IEnumerable<IOpcItem> items)
        {
        }

        /// <summary>
        /// Timer elapsed
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e) => Export(OpcServer, OpcGroup);

        /// <summary>
        /// Export to csv
        /// </summary>
        private void Export(IOpcServer opcServer, IOpcGroup opcGroup)
        {
            //_loggerService.AddInfo($"Export to .CSV. Host: {_opcServer.Host}; Server: {_opcServer.Name}; Group: {_opcGroup.Name};");
            
            var report = new StringBuilder();
            int errors = 0;

            foreach (var item in opcGroup.GetOpcItems().Values)
            {
                try
                {
                    report.Append(string.Join(WellKnownCodes.SystemStringSeparator,
                        GetCsvString(opcGroup.Name),
                        GetCsvString(item.Name),
                        GetCsvString(string.IsNullOrEmpty(item.AccessPath) ? " " : item.AccessPath),
                        item.CanonicalDataTypeId,
                        item.ReadOnly,
                        "0",
                        "1000",//opcGroup.ReqUpdateRate.ToString(),
                        "0",
                        GetCsvString(item.Value),
                        item.Quality,
                        GetCsvString(item.TimestampUtc),
                        GetCsvString(item.TimestampLocal),
                        item.CanonicalDataType + "\r\n"));
                }
                catch { errors++; }
            }

            string fileName = Settings.FolderPath + "\\" + opcGroup.Name;
            fileName = Settings.IsWriteInOneFile == false ? fileName + "_" + DateTime.Now.ToString("HH.mm.ss_dd.MM.yyyy") : fileName;
            fileName += ".csv";

            Logger.Info($"{LoggerPrefix} Export to .CSV file: {fileName}");

            try
            {
                Directory.CreateDirectory(Settings.FolderPath);
            }
            catch { }

            try
            {
                CreateCSVExportFile(fileName, opcServer.Name, opcGroup.Name);
                File.AppendAllText(fileName, report.ToString(), Encoding.ASCII);
            }
            catch { }
        }

        /// <summary>
        /// Create csv file
        /// </summary>
        private void CreateCSVExportFile(string fileName, string serverName, string groupName)
        {
            try
            {
                var report = new StringBuilder();
                report.Append("# " + serverName + " Group: " + groupName + " - CSV File Export;;;;;;;;;;;\r\n");
                report.Append("# Format as Follows:;;;;;;;;;;;\r\n");
                report.Append("# GroupName;ItemName;ItemPath;DataType;ReadOnly;Reserved;UpdateRate;Reserved;Value;Quality;Timestamp;LocalTimestamp;\r\n");
                report.Append("# e.g.: Group1;FIC101;;3;1;1;1000;0;31;Good Non-Specific;03.11.2008 13:59;03.11.2008 10:59;Long Integer\r\n");
                report.Append("#;;;;;;;;;;;\r\n");

                var header = WellKnownCodes.SystemStringSeparator != ";" ?
                    report.ToString().Replace(";", WellKnownCodes.SystemStringSeparator) : report.ToString();

                File.WriteAllText(fileName, header, Encoding.ASCII);
            }
            catch { }
        }

        /// <summary>
        /// Return csv string
        /// </summary>
        /// <param name="content">Text</param>
        /// <returns>csv string</returns>
        private string GetCsvString(string content) => string.IsNullOrEmpty(content) ? string.Empty : $"\"{content}\"";
    }
}
