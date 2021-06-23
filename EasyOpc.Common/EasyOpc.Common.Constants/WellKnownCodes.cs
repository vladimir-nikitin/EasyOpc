namespace EasyOpc.Common.Constants
{
    /// <summary>
    /// Common constants
    /// </summary>
    public static class WellKnownCodes
    {
        /// <summary>
        /// Date format
        /// </summary>
        public static string DateFormat => "dd.MM.yyyy";

        /// <summary>
        /// Time format
        /// </summary>
        public static string TimeFormat => "HH.mm.ss";

        /// <summary>
        /// Time format
        /// </summary>
        public static string DateTimeFormat => "dd.MM.yyyy HH:mm:ss";

        /// <summary>
        /// Export time format
        /// </summary>
        public static string ExportDateTimeFormat = "dd.MM.yyyy H:mm:ss.fff";

        /// <summary>
        /// Desktop application name
        /// </summary>
        public static string ClientName => "EasyOPC";

        /// <summary>
        /// Win-service application name
        /// </summary>
        public static string ServiceName => "EasyOpcWinService";

        /// <summary>
        /// Win-service URL
        /// </summary>
        public static string ServiceUrl => "http://localhost:5558";

        /// <summary>
        /// Default delimiter
        /// </summary>
        public static char StringSeparator => ';';

        /// <summary>
        /// System delimiter
        /// </summary>
        public static string SystemStringSeparator => System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        #region SettingNames

        /// <summary>
        /// Setting: path to log file
        /// </summary>
        public static string LogFilePathSettingName => "LogFilePath";

        /// <summary>
        /// Setting: service mode flag
        /// </summary>
        public static string ServiceModeSettingName => "ServiceMode";

        /// <summary>
        /// Setting: telegram token
        /// </summary>
        public static string TelegramTokenSettingName => "TelegramToken";

        /// <summary>
        /// Setting: Email
        /// </summary>
        public static string EmailCredentialSettingName => "EmailCredential";

        #endregion SettingNames
    }
}
