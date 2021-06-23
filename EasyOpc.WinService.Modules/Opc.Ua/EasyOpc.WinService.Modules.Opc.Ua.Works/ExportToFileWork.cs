using EasyOpc.Common.Constants;
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
using System.Text;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Ua.Works
{
    public class ExportToFileWork : IWork
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
        private ExportToFileWorkSettings Settings { get; }

        /// <summary>
        /// OPC.DA group id
        /// </summary>
        private Guid OpcUaGroupId { get; }

        /// <summary>
        /// OPC.DA server
        /// </summary>
        private IOpcUaServer OpcUaServer { get; set; }

        /// <summary>
        /// OPC.DA group
        /// </summary>
        private IOpcUaGroup OpcUaGroup { get; set; }

        /// <summary>
        /// Logs prefix
        /// </summary>
        private string LoggerPrefix { get; set; }

        /// <summary>
        /// Timer
        /// </summary>
        private System.Timers.Timer Timer { get; set; }

        /// <summary>
        /// Work name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Launch group
        /// </summary>
        public string LaunchGroup { get; }

        public ExportToFileWork(string name, string launchGroup, Guid opcUaGroupId, string jsonSettings,
            ILogger logger, IOpcUaServersService opcUaServersService, IOpcUaGroupsService opcUaGroupsService,
            IOpcUaItemsService opcUaItemsService, IOpcUaServersFactory opcUaServersFactory)
        {
            Name = name;
            LaunchGroup = launchGroup;
            OpcUaGroupId = opcUaGroupId;
            Settings = JsonConvert.DeserializeObject<ExportToFileWorkSettings>(jsonSettings);

            Logger = logger;

            OpcUaServersService = opcUaServersService;
            OpcUaGroupsService = opcUaGroupsService;
            OpcUaItemsService = opcUaItemsService;
            OpcUaServersFactory = opcUaServersFactory;
        }

        public async Task StartAsync()
        {
            if (Settings.Timespan.TotalMilliseconds == 0)
                return;

            var opcGroupData = await OpcUaGroupsService.GetByIdAsync(OpcUaGroupId);
            var opcServerData = await OpcUaServersService.GetByIdAsync(opcGroupData.OpcUaServerId);
            var opcItemDatas = await OpcUaItemsService.GetByOpcUaGroupIdAsync(opcGroupData.Id);

            LoggerPrefix = $"[Host: {opcServerData.Host}][OPC.DA server: {opcServerData.Name}][OPC.DA group: {opcGroupData.Name}]";
            Logger.Info($"{LoggerPrefix} Export to .CSV is started");

            OpcUaServer = OpcUaServersFactory.Create(opcServerData.Id, opcServerData.Name, opcServerData.Host, opcServerData.UserName, opcServerData.Password);
            OpcUaGroup = await OpcUaServer.CreateOpcUaGroupAsync(opcGroupData.Id, opcGroupData.Name, opcItemDatas.Select(p => new OpcUaItem
            {
                Id = p.Id,
                Name = p.Name,
                NodeId = p.NodeId
            }));

            if (OpcUaGroup == null)
                return;

            OpcUaGroup.OpcUaItemsChanged += OpcItemsChanged;

            Timer = new System.Timers.Timer(Settings.Timespan.TotalMilliseconds);
            Timer.Enabled = true;
            Timer.AutoReset = true;
            Timer.Elapsed += OnTimerElapsed;
            Timer.Start();
            Export(OpcUaServer, OpcUaGroup);
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
            }

            Logger.Info($"{LoggerPrefix} Export to .CSV is stopped");

            return Task.CompletedTask;
        }


        private void OpcItemsChanged(IEnumerable<IOpcUaItem> items)
        {
        }

        /// <summary>
        /// Timer elapsed
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e) => Export(OpcUaServer, OpcUaGroup);

        /// <summary>
        /// Export to csv
        /// </summary>
        private void Export(IOpcUaServer opcServer, IOpcUaGroup opcGroup)
        {
            //_loggerService.AddInfo($"Export to .CSV. Host: {_opcServer.Host}; Server: {_opcServer.Name}; Group: {_opcGroup.Name};");

            var report = new StringBuilder();
            int errors = 0;

            foreach (var item in opcGroup.GetOpcUaItems().Values)
            {
                try
                {
                    report.AppendLine(string.Join(WellKnownCodes.SystemStringSeparator,
                        GetCsvString(opcGroup.Name),
                        GetCsvString(item.Name),
                        "",
                        "0",
                        "0",
                        "0",
                        "1000",//opcGroup.ReqUpdateRate.ToString(),
                        "0",
                        GetCsvString(item.Value?.Replace('"', '`')),
                        GetCsvString("Good Non-Specific"),
                        GetCsvString(item.Timestamp?.ToUniversalTime().ToString(WellKnownCodes.ExportDateTimeFormat)),
                        GetCsvString(item.Timestamp?.ToLocalTime().ToString(WellKnownCodes.ExportDateTimeFormat)),
                        "Unknown"));
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
