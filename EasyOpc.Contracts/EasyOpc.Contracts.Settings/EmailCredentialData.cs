namespace EasyOpc.Contract.Setting
{
    /// <summary>
    /// Email сredential
    /// </summary>
    public class EmailCredentialData
    {
        /// <summary>
        /// Smtp server host
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Smtp server port
        /// </summary>
        public int Port { get; set; }
    }
}
