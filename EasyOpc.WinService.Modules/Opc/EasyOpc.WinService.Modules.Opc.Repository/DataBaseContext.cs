using EasyOpc.WinService.Modules.Opc.Repository.Model;
using System.Data.Entity;

namespace EasyOpc.WinService.Modules.Opc.Repository
{
    /// <summary>
    /// Context for working with the database
    /// </summary>
    public class DataBaseContext : DbContext
    {
        /// <summary>
        /// List of OPC servers
        /// </summary>
        public DbSet<OpcServerDto> OpcServers { get; set; }

        /// <summary>
        /// List of OPC groups
        /// </summary>
        public DbSet<OpcGroupDto> OpcGroups { get; set; }

        /// <summary>
        /// List of OPC items
        /// </summary>
        public DbSet<OpcItemDto> OpcItems { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        public DataBaseContext(string connectionString) : base(connectionString)
        {
        }
    }
}
