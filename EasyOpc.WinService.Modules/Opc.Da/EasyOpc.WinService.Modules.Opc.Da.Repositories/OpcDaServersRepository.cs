using EasyOpc.WinService.Core.Configuration.Contract;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Models;

namespace EasyOpc.WinService.Modules.Opc.Da.Repositories
{
    /// <summary>
    /// OPC.DA servers repository
    /// </summary>
    public class OpcDaServersRepository : BaseRepository<DataBaseContext, OpcDaServerDto>, IOpcDaServersRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration module</param>
        /// <param name="logger">Logger module</param>
        public OpcDaServersRepository(IConfiguration configuration, ILogger logger) :
            base(new DataBaseContext(configuration[ConfigurationKeys.ConnectionString]), logger)
        {
        }
    }
}
