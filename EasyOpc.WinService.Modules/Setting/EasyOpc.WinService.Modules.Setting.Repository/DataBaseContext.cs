using EasyOpc.WinService.Modules.Setting.Repository.Model;
using System.Data.Entity;

namespace EasyOpc.WinService.Modules.Setting.Repository
{
    /// <summary>
    /// Context for working with the database
    /// </summary>
    public class DataBaseContext : DbContext
    {
        /// <summary>
        /// List of OPC items
        /// </summary>
        public DbSet<SettingDto> Settings { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        public DataBaseContext(string connectionString) : base(connectionString)
        {
        }
    }
}
