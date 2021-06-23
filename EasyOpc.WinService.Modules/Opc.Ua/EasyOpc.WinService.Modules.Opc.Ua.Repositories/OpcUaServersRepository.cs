using EasyOpc.WinService.Core.Configuration.Contract;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models;

namespace EasyOpc.WinService.Modules.Opc.Ua.Repositories
{
    /// <summary>
    /// OPC.UA servers repository
    /// </summary>
    public class OpcUaServersRepository : BaseRepository<DataBaseContext, OpcUaServerDto>, IOpcUaServersRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpcUaServersRepository(IConfiguration configuration, ILogger logger) :
            base(new DataBaseContext(configuration[ConfigurationKeys.ConnectionString]), logger)
        {
        }
    }
}
