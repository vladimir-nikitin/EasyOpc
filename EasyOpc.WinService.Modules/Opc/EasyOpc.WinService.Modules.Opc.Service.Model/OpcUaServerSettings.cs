using EasyOpc.Common.Types;

namespace EasyOpc.WinService.Modules.Opc.Service.Model
{
    /// <summary>
    /// OPC UA Server settings
    /// </summary>
    public class OpcUaServerSettings
    {
        /// <summary>
        /// User identity settings
        /// </summary>
        public UserIdentitySetting UserIdentitySetting { get; set; }
    }
}
