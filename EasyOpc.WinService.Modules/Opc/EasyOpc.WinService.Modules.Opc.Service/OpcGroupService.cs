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
    /// OPC group service
    /// </summary>
    public class OpcGroupService : BaseService<OpcGroup, OpcGroupDto>, IOpcGroupService
    {
        private IOpcItemService OpcItemService { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opcItemService">OPC items service</param>
        /// <param name="repository">OPC group repository</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="logger">Logger</param>
        public OpcGroupService(IOpcItemService opcItemService, IOpcGroupRepository repository,  IMapper mapper, ILogger logger) 
            : base(repository, mapper, logger)
        {
            OpcItemService = opcItemService;
        }

        public override async Task<OpcGroup> AddAsync(OpcGroup group)
        {
            var items = await OpcItemService.AddRangeAsync(group.OpcItems);
            var opcGroup = await base.AddAsync(group);
            opcGroup.OpcItems = items;
            return opcGroup;
        }

        /// <summary>
        /// <see cref="IBaseService.RemoveByIdAsync(Guid)"/>
        /// </summary>
        public override async Task<OpcGroup> RemoveByIdAsync(Guid id)
        {
            var items = await OpcItemService.GetByOpcGroupIdAsync(id);
            await OpcItemService.RemoveRangeAsync(items);

            return await base.RemoveByIdAsync(id);
        }

        /// <summary>
        /// <see cref="IOpcGroupService.GetByOpcServerIdAsync(Guid)"/>
        /// </summary>
        public async Task<IEnumerable<OpcGroup>> GetByOpcServerIdAsync(Guid id)
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcGroup>>(await (Repository as IOpcGroupRepository).GetByOpcServerIdAsync(id));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
