using AutoMapper;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Core.WorksService.Contract;
using EasyOpc.WinService.Modules.Opc.Ua.Connector.Contract;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Contracts;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Models;
using EasyOpc.WinService.Modules.Opc.Ua.Works;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Opc.Da.Services
{
    /// <summary>
    /// OPC.UA works service
    /// </summary>
    public class OpcUaGroupWorksService : BaseService<OpcUaGroupWork, OpcUaGroupWorkDto>, IOpcUaGroupWorksService
    {
        private IOpcUaServersService OpcUaServersService { get; }

        private IOpcUaGroupsService OpcUaGroupsService { get; }

        private IOpcUaItemsService OpcUaItemsService { get; }

        private IOpcUaServersFactory OpcUaServersFactory { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpcUaGroupWorksService(IOpcUaGroupWorksRepository repository, IMapper mapper, ILogger logger,
            IOpcUaServersService opcUaServersService, IOpcUaGroupsService opcUaGroupsService,
            IOpcUaItemsService opcUaItemsService, IOpcUaServersFactory opcUaServersFactory)
            : base(repository, mapper, logger)
        {
            OpcUaServersService = opcUaServersService;
            OpcUaGroupsService = opcUaGroupsService;
            OpcUaItemsService = opcUaItemsService;
            OpcUaServersFactory = opcUaServersFactory;
        }

        /// <inheritdoc cref="IOpcUaGroupWorksService.GetByOpcUaGroupIdAndTypeAsync(Guid, IEnumerable{string})"/>
        public async Task<IEnumerable<OpcUaGroupWork>> GetByOpcUaGroupIdAndTypeAsync(Guid opcUaGroupId, IEnumerable<string> workTypes)
        {
            try
            {
                return Mapper.Map<IEnumerable<OpcUaGroupWork>>(await(Repository as IOpcUaGroupWorksRepository).GetByOpcUaGroupIdAndTypeAsync(opcUaGroupId, workTypes));
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
                    works.Add(new SubscritionToFileWork(workDto.Name, workDto.LaunchGroup, workDto.OpcUaGroupId, workDto.JsonSettings,
                        Logger, OpcUaServersService, OpcUaGroupsService, OpcUaItemsService, OpcUaServersFactory));
                }
                else if (workDto.Type == "EXPORT_TO_FILE")
                {
                    works.Add(new ExportToFileWork(workDto.Name, workDto.LaunchGroup, workDto.OpcUaGroupId, workDto.JsonSettings,
                        Logger, OpcUaServersService, OpcUaGroupsService, OpcUaItemsService, OpcUaServersFactory));
                }

            }

            return works;
        }
    }
}
