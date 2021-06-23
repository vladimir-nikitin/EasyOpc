using EasyOpc.Common.Types;
using EasyOpc.WinService.Core.Configuration.Contract;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Da.Repositories
{
    /// <summary>
    /// OPC.DA items repository
    /// </summary>
    public class OpcDaItemsRepository : BaseRepository<DataBaseContext, OpcDaItemDto>, IOpcDaItemsRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpcDaItemsRepository(IConfiguration configuration, ILogger logger) : 
            base(new DataBaseContext(configuration[ConfigurationKeys.ConnectionString]), logger)
        {
        }

        /// <inheritdoc cref="IOpcDaItemsRepository.GetByOpcDaGroupIdAsync(Guid)"/>
        public async Task<IEnumerable<OpcDaItemDto>> GetByOpcDaGroupIdAsync(Guid opcDaGroupId)
        {
            try
            {
                return await Entities.Where(p => p.OpcDaGroupId == opcDaGroupId).ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        /// <inheritdoc cref="IOpcDaItemsRepository.GetPageAsync(Guid, int, int)"/>
        public async Task<Page<OpcDaItemDto>> GetPageAsync(Guid opcDaGroupId, int pageNumber, int countInPage)
        {
            try
            {
                var items = Entities.Where(p => p.OpcDaGroupId == opcDaGroupId);
                return new Page<OpcDaItemDto>
                {
                    PageNumber = pageNumber,
                    CountInPage = countInPage,
                    TotalCount = await items.CountAsync(),
                    Items = await items.OrderBy(p => p.Id).Skip(countInPage * (pageNumber)).Take(countInPage).ToArrayAsync()
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
