using System;

namespace EasyOpc.WinService.Modules.Opc.Workers.History
{
    /// <summary>
    /// Group history setting
    /// </summary>
    public class HistoryWorkSetting
    {
        /// <summary>
        /// Путь к папке
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// Периодичность создания файла
        /// </summary>
        public TimeSpan FileTimespan { get; set; }

        /// <summary>
        /// Период хранения истории
        /// </summary>
        public TimeSpan HistoryRetentionTimespan { get; set; }
    }
}
