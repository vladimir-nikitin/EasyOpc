using EasyOpc.WinService.Core.Repository.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyOpc.WinService.Modules.Opc.Da.Repositories.Models
{
    /// <summary>
    /// OPC.DA server
    /// </summary>
    [Table("OpcDaServers")]
    public class OpcDaServerDto : BaseDto
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
        /// Server settings in json format
        /// </summary>
        public string Clsid { get; set; }
    }
}
