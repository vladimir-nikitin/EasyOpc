using AutoMapper;
using EasyOpc.Common.Constant;
using EasyOpc.Contract.Setting;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Modules.Setting.Service.Contract;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyOpc.WinService.Controllers
{
    [RoutePrefix("api/settings")]
    public class SettingsController : ApiController
    {
        private ISettingService SettingService { get; }

        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public SettingsController(ISettingService settingService, ILogger logger, IMapper mapper)
        {
            SettingService = settingService;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("getByName/{settingName}")]
        public async Task<SettingData> GetByNameAsync(string settingName)
        {
            try
            {
                return Mapper.Map<SettingData>(await SettingService.GetByNameAsync(settingName));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPatch]
        [Route("updateByName/{settingName}")]
        public async Task UpdateByNameAsync(string settingName, [FromUri] string value)
        {
            try
            {
                var setting = await SettingService.GetByNameAsync(settingName);
                setting.Value = value;
                await SettingService.UpdateAsync(setting);

                if(setting.Name == WellKnownCodes.LogFilePathSettingName)
                {
                    Logger.SetLogFilePath(setting.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
