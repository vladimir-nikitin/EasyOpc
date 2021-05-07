using AutoMapper;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Opc.Repository.Contract;
using EasyOpc.WinService.Modules.Opc.Repository.Model;
using EasyOpc.WinService.Modules.Opc.Service.Contract;
using EasyOpc.WinService.Modules.Opc.Service.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Service
{
    /// <summary>
    /// OPC item service
    /// </summary>
    public class OpcItemService : BaseService<OpcItem, OpcItemDto>, IOpcItemService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository">OPC item repository</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="logger">Logger</param>
        public OpcItemService(IOpcItemRepository repository, IMapper mapper, ILogger logger) 
            : base(repository, mapper, logger)
        {
        }

        /// <summary>
        /// <see cref="IOpcItemService.GetByOpcGroupIdAsync(Guid)"/>
        /// </summary>
        public async Task<IEnumerable<OpcItem>> GetByOpcGroupIdAsync(Guid id)
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcItem>>(await (Repository as IOpcItemRepository).GetByOpcGroupIdAsync(id));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
