using EasyOpc.WinService.Core.Repository.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models
{
    /// <summary>
    /// OPC.UA server
    /// </summary>
    [Table("OpcUaServers")]
    public class OpcUaServerDto : BaseDto
    {
        /// <summary>
        /// Server name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
