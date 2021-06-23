using AutoMapper;
using EasyOpc.Common.Types;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Models;
using EasyOpc.WinService.Modules.Opc.Da.Services.Contracts;
using EasyOpc.WinService.Modules.Opc.Da.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Da.Services
{
    /// <summary>
    /// OPC.DA items service
    /// </summary>
    public class OpcDaItemsService : BaseService<OpcDaItem, OpcDaItemDto>, IOpcDaItemsService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpcDaItemsService(IOpcDaItemsRepository repository, IMapper mapper, ILogger logger) 
            : base(repository, mapper, logger)
        {
        }

        public async Task<IEnumerable<OpcDaItem>> GetByOpcDaGroupIdAsync(Guid id)
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcDaItem>>(await (Repository as IOpcDaItemsRepository).GetByOpcDaGroupIdAsync(id));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public async Task<Page<OpcDaItem>> GetPageAsync(Guid opcGroupId, int pageNumber, int countInPage)
        {
            try
            {
                var page = await (Repository as IOpcDaItemsRepository).GetPageAsync(opcGroupId, pageNumber, countInPage);
                return new Page<OpcDaItem>
                {
                    PageNumber = page.PageNumber,
                    CountInPage = page.CountInPage,
                    TotalCount = page.TotalCount,
                    Items = Mapper.Map<IEnumerable<OpcDaItem>>(page.Items)
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
