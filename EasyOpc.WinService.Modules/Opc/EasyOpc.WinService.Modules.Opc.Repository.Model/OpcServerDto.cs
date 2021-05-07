using EasyOpc.Common.Opc;
using EasyOpc.WinService.Core.Repository.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyOpc.WinService.Modules.Opc.Repository.Model
{
    /// <summary>
    /// OPC server
    /// </summary>
    [Table("OpcServers")]
    public class OpcServerDto : BaseDto
    {
        /// <summary>
        /// Server name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Server type DA/UA
        /// </summary>
        public OpcServerType Type { get; set; }

        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Server settings in json format
        /// </summary>
        public string JsonSettings { get; set; }
    }
}
