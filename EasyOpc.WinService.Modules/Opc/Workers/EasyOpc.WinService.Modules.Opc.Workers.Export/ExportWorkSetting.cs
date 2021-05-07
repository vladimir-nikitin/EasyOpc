using System;

namespace EasyOpc.WinService.Modules.Opc.Workers.Export
{
    /// <summary>
    /// Group export setting
    /// </summary>
    public class ExportWorkSetting
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
