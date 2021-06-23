using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models;
using System.Data.Entity;

namespace EasyOpc.WinService.Modules.Opc.Ua.Repositories
{
    /// <summary>
    /// Context for working with the database
    /// </summary>
    public class DataBaseContext : DbContext
    {
        /// <summary>
        /// List of OPC.UA servers
        /// </summary>
        public DbSet<OpcUaServerDto> OpcUaServers { get; set; }

        /// <summary>
        /// List of OPC.UA groups
        /// </summary>
        public DbSet<OpcUaGroupDto> OpcUaGroups { get; set; }

        /// <summary>
        /// List of OPC.UA group works
        /// </summary>
        public DbSet<OpcUaGroupWorkDto> OpcUaGroupWorks { get; set; }

        /// <summary>
        /// List of OPC.UA items
        /// </summary>
        public DbSet<OpcUaItemDto> OpcUaItems { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        public DataBaseContext(string connectionString) : base(connectionString)
        {
        }
    }
}
