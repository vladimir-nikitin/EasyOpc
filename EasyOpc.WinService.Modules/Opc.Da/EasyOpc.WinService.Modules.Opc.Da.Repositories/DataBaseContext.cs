using EasyOpc.WinService.Modules.Opc.Da.Repositories.Models;
using System.Data.Entity;

namespace EasyOpc.WinService.Modules.Opc.Da.Repositories
{
    /// <summary>
    /// Context for working with the database
    /// </summary>
    public class DataBaseContext : DbContext
    {
        /// <summary>
        /// List of OPC.DA servers
        /// </summary>
        public DbSet<OpcDaServerDto> OpcDaServers { get; set; }

        /// <summary>
        /// List of OPC.DA groups
        /// </summary>
        public DbSet<OpcDaGroupDto> OpcDaGroups { get; set; }

        /// <summary>
        /// List of OPC.DA group works
        /// </summary>
        public DbSet<OpcDaGroupWorkDto> OpcDaGroupWorks { get; set; }

        /// <summary>
        /// List of OPC.DA items
        /// </summary>
        public DbSet<OpcDaItemDto> OpcDaItems { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        public DataBaseContext(string connectionString) : base(connectionString)
        {
        }
    }
}
