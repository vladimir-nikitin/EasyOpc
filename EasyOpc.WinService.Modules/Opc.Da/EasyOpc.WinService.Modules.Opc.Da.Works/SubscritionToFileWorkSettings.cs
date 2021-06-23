using System;

namespace EasyOpc.WinService.Modules.Opc.Da.Works
{
    /// <summary>
    /// Group history setting
    /// </summary>
    public class SubscritionToFileWorkSettings
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
