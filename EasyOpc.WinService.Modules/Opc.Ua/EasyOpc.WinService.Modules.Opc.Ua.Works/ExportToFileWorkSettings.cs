using System;

namespace EasyOpc.WinService.Modules.Opc.Ua.Works
{
    /// <summary>
    /// Group export setting
    /// </summary>
    public class ExportToFileWorkSettings
    {
        /// <summary>
        /// Записывать все в один файл
        /// </summary>
        public bool IsWriteInOneFile { get; set; }

        /// <summary>
        /// Timespan
        /// </summary>
        public TimeSpan Timespan { get; set; }

        /// <summary>
        /// Путь к папке
        /// </summary>
        public string FolderPath { get; set; }
    }
}
