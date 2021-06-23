using AutoMapper;
using EasyOpc.WinService.Core.Configuration;
using EasyOpc.WinService.Core.Configuration.Contract;
using EasyOpc.WinService.Core.Logger;
using EasyOpc.WinService.Core.Logger.Contract;
using System.Collections.Generic;
using System.Web.Http;
using Unity;
using Unity.WebApi;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR;
using System.Reflection;
using Microsoft.AspNet.SignalR.Infrastructure;
using System;
using EasyOpc.Contract.Setting;
using EasyOpc.Common.Constants;
using EasyOpc.WinService.Modules.Settings.Repositories.Contracts;
using EasyOpc.WinService.Modules.Settings.Repositories;
using EasyOpc.WinService.Modules.Settings.Services.Contracts;
using EasyOpc.WinService.Modules.Settings.Services;
using Unity.Injection;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Models;
using EasyOpc.WinService.Modules.Settings.Repositories.Models;
using EasyOpc.WinService.Modules.Settings.Services.Models;
using EasyOpc.WinService.Modules.Opc.Da.Services.Models;
using EasyOpc.Contracts.Opc.Da;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Models;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Models;
using EasyOpc.Contracts.Opc.Ua;
using EasyOpc.WinService.Modules.Opc.Da.Connector;
using EasyOpc.WinService.Modules.Opc.Da.Connector.Contract;
using EasyOpc.WinService.Modules.Opc.Da.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Da.Repositories;
using EasyOpc.WinService.Modules.Opc.Da.Services.Contracts;
using EasyOpc.WinService.Modules.Opc.Da.Services;
using EasyOpc.WinService.Modules.Opc.Ua.Connector.Contract;
using EasyOpc.WinService.Modules.Opc.Ua.Connector;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories.Contracts;
using EasyOpc.WinService.Modules.Opc.Ua.Services.Contracts;
using EasyOpc.WinService.Modules.Opc.Ua.Repositories;
using EasyOpc.WinService.Modules.Opc.Ua.Services;
using EasyOpc.WinService.Core.WorksExecutionService;
using EasyOpc.WinService.Core.WorksExecutionService.Contract;

namespace EasyOpc.WinService
{
    public class SignalRContractResolver : IContractResolver
    {

        private readonly Assembly assembly;
        private readonly IContractResolver camelCaseContractResolver;
        private readonly IContractResolver defaultContractSerializer;

        public SignalRContractResolver()
        {
            defaultContractSerializer = new DefaultContractResolver();
            camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
            assembly = typeof(Connection).Assembly;
        }

        public JsonContract ResolveContract(Type type)
        {
            if (type.Assembly.Equals(assembly))
            {
                return defaultContractSerializer.ResolveContract(type);

            }

            return camelCaseContractResolver.ResolveContract(type);
        }
    }

    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            IConfiguration configuration = new Configuration(new Dictionary<string, string>
            {
                { ConfigurationKeys.ConnectionString, "DbContext" }
            });

            var mapper = ConfigureMapper();
            container.RegisterInstance(mapper);

            var logger = new Logger();
            container.RegisterInstance<ILogger>(logger);
            container.RegisterInstance(configuration);

            //SETTINGS
            container.RegisterType<ISettingsRepository, SettingsRepository>();
            container.RegisterType<ISettingsService, SettingsService>(new InjectionConstructor(container.Resolve<ISettingsRepository>(), mapper, logger));

            try
            {
                var logFilePathSetting = container.Resolve<ISettingsService>().GetByNameAsync(WellKnownCodes.LogFilePathSettingName).GetAwaiter().GetResult();
                if (logFilePathSetting != null)
                {
                    logger.SetLogFilePath(logFilePathSetting.Value);
                }
            }
            catch { }

            //OPC.DA
            container.RegisterInstance((IOpcDaServersFactory)container.Resolve<OpcDaServersFactory>());

            container.RegisterType<IOpcDaServersRepository, OpcDaServersRepository>();
            container.RegisterType<IOpcDaGroupsRepository, OpcDaGroupsRepository>();
            container.RegisterType<IOpcDaItemsRepository, OpcDaItemsRepository>();
            container.RegisterType<IOpcDaGroupWorksRepository, OpcDaGroupWorksRepository>();

            container.RegisterType<IOpcDaItemsService, OpcDaItemsService>();
            container.RegisterType<IOpcDaGroupsService, OpcDaGroupsService>();
            container.RegisterType<IOpcDaServersService, OpcDaServersService>();
            container.RegisterType<IOpcDaGroupWorksService, OpcDaGroupWorksService>();

            //OPC.UA
            container.RegisterInstance((IOpcUaServersFactory)container.Resolve<OpcUaServersFactory>());

            container.RegisterType<IOpcUaServersRepository, OpcUaServersRepository>();
            container.RegisterType<IOpcUaGroupsRepository, OpcUaGroupsRepository>();
            container.RegisterType<IOpcUaItemsRepository, OpcUaItemsRepository>();
            container.RegisterType<IOpcUaGroupWorksRepository, OpcUaGroupWorksRepository>();

            container.RegisterType<IOpcUaItemsService, OpcUaItemsService>();
            container.RegisterType<IOpcUaGroupsService, OpcUaGroupsService>();
            container.RegisterType<IOpcUaServersService, OpcUaServersService>();
            container.RegisterType<IOpcUaGroupWorksService, OpcUaGroupWorksService>();

            var worksExecutionService = new WorksExecutionService(container.Resolve<ISettingsService>());
            worksExecutionService.RegisterSource(container.Resolve<IOpcDaGroupWorksService>());
            worksExecutionService.RegisterSource(container.Resolve<IOpcUaGroupWorksService>());
            container.RegisterInstance<IWorksExecutionService>(worksExecutionService);

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = JsonSerializer.Create(settings);

            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
         
            try
            {
                var serviceModeSetting = container.Resolve<ISettingsService>().GetByNameAsync(WellKnownCodes.ServiceModeSettingName).GetAwaiter().GetResult();
                if (serviceModeSetting != null && serviceModeSetting.Value?.ToLower() == "false")
                {
                    worksExecutionService.StartAsync();
                }
            }
            catch { }
        }

        /// <summary>
        /// <see cref="BaseModule.ConfigureMapper"/>
        /// </summary>
        private static IMapper ConfigureMapper() => new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AllowNullCollections = true;

            cfg.CreateMap<OpcDaServerDto, Modules.Opc.Da.Services.Models.OpcDaServer>().ReverseMap();
            cfg.CreateMap<Modules.Opc.Da.Services.Models.OpcDaServer, OpcDaServerData>().ReverseMap();

            cfg.CreateMap<OpcDaGroupDto, Modules.Opc.Da.Services.Models.OpcDaGroup>().ReverseMap();
            cfg.CreateMap<Modules.Opc.Da.Services.Models.OpcDaGroup, OpcDaGroupData>().ReverseMap();

            cfg.CreateMap<OpcDaGroupWorkDto, OpcDaGroupWork>().ReverseMap();
            cfg.CreateMap<OpcDaGroupWork, OpcDaGroupWorkData>().ReverseMap();

            cfg.CreateMap<OpcDaItemDto, Modules.Opc.Da.Services.Models.OpcDaItem>().ReverseMap();
            cfg.CreateMap<Modules.Opc.Da.Services.Models.OpcDaItem, OpcDaItemData>().ReverseMap();

            cfg.CreateMap<Modules.Opc.Da.Services.Models.DiscoveryItem, Contracts.Opc.Da.DiscoveryItemData>().ReverseMap();

            cfg.CreateMap<OpcUaServerDto, Modules.Opc.Ua.Services.Models.OpcUaServer>().ReverseMap();
            cfg.CreateMap<Modules.Opc.Ua.Services.Models.OpcUaServer, OpcUaServerData>().ReverseMap();

            cfg.CreateMap<OpcUaGroupDto, Modules.Opc.Ua.Services.Models.OpcUaGroup>().ReverseMap();
            cfg.CreateMap<Modules.Opc.Ua.Services.Models.OpcUaGroup, OpcUaGroupData>().ReverseMap();

            cfg.CreateMap<OpcUaGroupWorkDto, OpcUaGroupWork>().ReverseMap();
            cfg.CreateMap<OpcUaGroupWork, OpcUaGroupWorkData>().ReverseMap();

            cfg.CreateMap<OpcUaItemDto, Modules.Opc.Ua.Services.Models.OpcUaItem>().ReverseMap();
            cfg.CreateMap<Modules.Opc.Ua.Services.Models.OpcUaItem, OpcUaItemData>().ReverseMap();

            cfg.CreateMap<Modules.Opc.Ua.Services.Models.DiscoveryItem, Contracts.Opc.Ua.DiscoveryItemData>().ReverseMap();

            cfg.CreateMap<SettingDto, Setting>().ReverseMap();
            cfg.CreateMap<Setting, SettingData>().ReverseMap();          
        }));
    }
}