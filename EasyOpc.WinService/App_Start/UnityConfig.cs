using AutoMapper;
using EasyOpc.WinService.Core.Configuration;
using EasyOpc.WinService.Core.Configuration.Contract;
using EasyOpc.WinService.Core.Logger;
using EasyOpc.WinService.Core.Logger.Contract;
using System.Collections.Generic;
using System.Web.Http;
using Unity;
using Unity.WebApi;
using EasyOpc.WinService.Modules.Opc.Connectors.Da;
using EasyOpc.WinService.Modules.Opc.Connectors.Ua;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR;
using System.Reflection;
using Microsoft.AspNet.SignalR.Infrastructure;
using System;
using EasyOpc.WinService.Modules.Opc.Repository.Contract;
using EasyOpc.WinService.Modules.Opc.Repository;
using EasyOpc.WinService.Modules.Opc.Service;
using EasyOpc.WinService.Modules.Opc.Service.Contract;
using ExportWorker = EasyOpc.WinService.Modules.Opc.Workers.Export.Worker;
using HistoryWorker = EasyOpc.WinService.Modules.Opc.Workers.History.Worker;
using EasyOpc.WinService.Modules.Setting.Repository.Contract;
using EasyOpc.WinService.Modules.Setting.Service.Contract;
using EasyOpc.WinService.Modules.Setting.Repository;
using EasyOpc.WinService.Modules.Setting.Service;
using Unity.Injection;
using EasyOpc.WinService.Modules.Work.Repository.Contract;
using EasyOpc.WinService.Modules.Work.Repository;
using EasyOpc.WinService.Modules.Work.Service.Contract;
using EasyOpc.WinService.Modules.Work.Service;
using EasyOpc.WinService.Modules.Opc.Repository.Model;
using EasyOpc.Contracts.Opc;
using EasyOpc.Common.Opc;
using EasyOpc.WinService.Modules.Setting.Repository.Model;
using EasyOpc.Contract.Setting;
using EasyOpc.WinService.Modules.Work.Repository.Model;
using EasyOpc.Contracts.Works;
using EasyOpc.WinService.Modules.Setting.Service.Model;
using EasyOpc.WinService.Modules.Opc.Service.Model;
using EasyOpc.WinService.Core.Worker.Model;

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

            //OPC
            container.RegisterInstance(container.Resolve<OpcDaServerFactory>());
            container.RegisterInstance(container.Resolve<OpcUaServerFactory>());

            container.RegisterType<IOpcServerRepository, OpcServerRepository>();
            container.RegisterType<IOpcGroupRepository, OpcGroupRepository>();
            container.RegisterType<IOpcItemRepository, OpcItemRepository>();

            container.RegisterFactory<IOpcServerService>((c) =>
                new OpcServerService(container.Resolve<OpcDaServerFactory>(), container.Resolve<OpcUaServerFactory>(),
                container.Resolve<IOpcServerRepository>(), container.Resolve<IOpcGroupService>(),
                container.Resolve<IOpcItemService>(), mapper, logger));

            container.RegisterFactory<IOpcGroupService>((c) => new OpcGroupService(container.Resolve<IOpcItemService>(),
                container.Resolve<IOpcGroupRepository>(), mapper, logger));

            container.RegisterFactory<IOpcItemService>((c) => new OpcItemService(container.Resolve<IOpcItemRepository>(), mapper, logger));

            container.RegisterType<ExportWorker>();
            container.RegisterType<HistoryWorker>();

            //SETTINGS
            container.RegisterType<ISettingRepository, SettingRepository>();
            container.RegisterType<ISettingService, SettingService>(new InjectionConstructor(container.Resolve<ISettingRepository>(), mapper, logger));

            //WORKS
            container.RegisterType<IWorkRepository, WorkRepository>();
            container.RegisterType<IWorkService, WorkService>(new InjectionConstructor(container.Resolve<IWorkRepository>(), mapper, logger));
            container.RegisterInstance<IWorkExecutionService>(container.Resolve<WorkExecutionService>());

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = JsonSerializer.Create(settings);

            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        /// <summary>
        /// <see cref="BaseModule.ConfigureMapper"/>
        /// </summary>
        private static IMapper ConfigureMapper() => new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AllowNullCollections = true;

            cfg.CreateMap<OpcServerDto, OpcServer>().ReverseMap();

            cfg.CreateMap<OpcServer, OpcServerData>()
                .ForMember(p => p.Type, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.Type = src.Type == OpcServerType.DA ? OpcWellKnownCodes.OPC_DA_TYPE : OpcWellKnownCodes.OPC_UA_TYPE);

            cfg.CreateMap<OpcServerData, OpcServer>()
                .ForMember(p => p.Type, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.Type = src.Type == OpcWellKnownCodes.OPC_DA_TYPE ? OpcServerType.DA : OpcServerType.UA);

            cfg.CreateMap<OpcGroupDto, OpcGroup>().ReverseMap();
            cfg.CreateMap<OpcGroup, OpcGroupData>().ReverseMap();

            cfg.CreateMap<DiscoveryItem, DiscoveryItemData>().ReverseMap();

            cfg.CreateMap<OpcItemDto, OpcItem>().ReverseMap();
            cfg.CreateMap<OpcItem, OpcItemData>().ReverseMap();

            cfg.CreateMap<SettingDto, Setting>().ReverseMap();
            cfg.CreateMap<Setting, SettingData>().ReverseMap();

            cfg.CreateMap<WorkDto, Work>().ReverseMap();
            cfg.CreateMap<Work, WorkData>().ReverseMap();
           
        }));
    }
}