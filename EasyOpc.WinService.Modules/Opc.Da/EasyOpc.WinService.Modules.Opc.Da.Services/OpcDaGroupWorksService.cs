using AutoMapper;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Core.WorksService.Contract;
using EasyOpc.WinService.Modules.Opc.Da.Connector.Contract;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Models;
using EasyOpc.WinService.Modules.Opc.Da.Services.Contracts;
using EasyOpc.WinService.Modules.Opc.Da.Services.Models;
using EasyOpc.WinService.Modules.Opc.Da.Works;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Da.Services
{
    /// <summary>
    /// OPC.DA works service
    /// </summary>
    public class OpcDaGroupWorksService : BaseService<OpcDaGroupWork, OpcDaGroupWorkDto>, IOpcDaGroupWorksService
    {
        private IOpcDaServersService OpcDaServersService { get; }

        private IOpcDaGroupsService OpcDaGroupsService { get; }

        private IOpcDaItemsService OpcDaItemsService { get; }

        private IOpcDaServersFactory OpcDaServersFactory { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpcDaGroupWorksService(IOpcDaGroupWorksRepository repository, IMapper mapper, ILogger logger, 
            IOpcDaServersService opcDaServersService, IOpcDaGroupsService opcDaGroupsService,
            IOpcDaItemsService opcDaItemsService, IOpcDaServersFactory opcDaServersFactory)
            : base(repository, mapper, logger)
        {
            OpcDaServersService = opcDaServersService;
            OpcDaGroupsService = opcDaGroupsService;
            OpcDaItemsService = opcDaItemsService;
            OpcDaServersFactory = opcDaServersFactory;
        }

        /// <inheritdoc cref="IOpcDaGroupWorksService.GetByOpcDaGroupIdAndTypeAsync(Guid, IEnumerable{string})"/>
        public async Task<IEnumerable<OpcDaGroupWork>> GetByOpcDaGroupIdAndTypeAsync(Guid opcDaGroupId, IEnumerable<string> types)
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcDaGroupWork>>(await(Repository as IOpcDaGroupWorksRepository).GetByOpcDaGroupIdAndTypeAsync(opcDaGroupId, types));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        /// <inheritdoc cref="IWorksService.GetWorks"/>
        public async Task<IEnumerable<IWork>> GetWorks()
        {
            var works = new List<IWork>();

            var workDtos = await GetAllAsync();
            foreach (var workDto in workDtos)
            {
                if (!workDto.IsEnabled)
                    continue;
                else if (workDto.Type == "SUBSCRITION_TO_FILE")
                {
                    works.Add(new SubscritionToFileWork(workDto.Name, workDto.LaunchGroup, workDto.OpcDaGroupId, workDto.JsonSettings, 
                        Logger, OpcDaServersService, OpcDaGroupsService, OpcDaItemsService, OpcDaServersFactory));
                }
                else if (workDto.Type == "EXPORT_TO_FILE")
                {
                    works.Add(new ExportToFileWork(workDto.Name, workDto.LaunchGroup, workDto.OpcDaGroupId, workDto.JsonSettings,
                        Logger, OpcDaServersService, OpcDaGroupsService, OpcDaItemsService, OpcDaServersFactory));
                }

            }

            return works;
        }
    }
}
