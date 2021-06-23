using EasyOpc.Common.Types;
using EasyOpc.WinService.Core.Configuration.Contract;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Ua.Repositories
{
    /// <summary>
    /// OPC.UA items repository
    /// </summary>
    public class OpcUaItemsRepository : BaseRepository<DataBaseContext, OpcUaItemDto>, IOpcUaItemsRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpcUaItemsRepository(IConfiguration configuration, ILogger logger) : 
            base(new DataBaseContext(configuration[ConfigurationKeys.ConnectionString]), logger)
        {
        }

        /// <inheritdoc cref="IOpcUaItemsRepository.GetByOpcUaGroupIdAsync(Guid)"/>
        public async Task<IEnumerable<OpcUaItemDto>> GetByOpcUaGroupIdAsync(Guid opcUaGroupId)
        {
            try
            {
                return await Entities.Where(p => p.OpcUaGroupId == opcUaGroupId).ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        /// <inheritdoc cref="IOpcUaItemsRepository.GetPageAsync(Guid, int, int)"/>
        public async Task<Page<OpcUaItemDto>> GetPageAsync(Guid opcUaGroupId, int pageNumber, int countInPage)
        {
            try
            {
                var items = Entities.Where(p => p.OpcUaGroupId == opcUaGroupId);
                return new Page<OpcUaItemDto>
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
