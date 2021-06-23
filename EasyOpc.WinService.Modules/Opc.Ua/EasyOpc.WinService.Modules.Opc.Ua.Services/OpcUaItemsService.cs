using AutoMapper;
using EasyOpc.Common.Types;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Contracts;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Ua.Services
{
    /// <summary>
    /// OPC.UA items service
    /// </summary>
    public class OpcUaItemsService : BaseService<OpcUaItem, OpcUaItemDto>, IOpcUaItemsService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpcUaItemsService(IOpcUaItemsRepository repository, IMapper mapper, ILogger logger) 
            : base(repository, mapper, logger)
        {
        }

        public async Task<IEnumerable<OpcUaItem>> GetByOpcUaGroupIdAsync(Guid id)
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcUaItem>>(await (Repository as IOpcUaItemsRepository).GetByOpcUaGroupIdAsync(id));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public async Task<Page<OpcUaItem>> GetPageAsync(Guid opcUaGroupId, int pageNumber, int countInPage)
        {
            try
            {
                var page = await (Repository as IOpcUaItemsRepository).GetPageAsync(opcUaGroupId, pageNumber, countInPage);
                return new Page<OpcUaItem>
                {
                    PageNumber = page.PageNumber,
                    CountInPage = page.CountInPage,
                    TotalCount = page.TotalCount,
                    Items = Mapper.Map<IEnumerable<OpcUaItem>>(page.Items)
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
