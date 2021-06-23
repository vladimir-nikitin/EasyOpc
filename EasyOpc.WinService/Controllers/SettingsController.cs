using AutoMapper;
using EasyOpc.Common.Constants;
using EasyOpc.Contract.Setting;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Modules.Settings.Services.Contracts;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyOpc.WinService.Controllers
{
    [RoutePrefix("api/settings")]
    public class SettingsController : ApiController
    {
        private ISettingsService SettingsService { get; }

        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public SettingsController(ISettingsService settingsService, ILogger logger, IMapper mapper)
        {
            SettingsService = settingsService;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("getByName/{settingName}")]
        public async Task<SettingData> GetByNameAsync(string settingName)
        {
            try
            {
                return Mapper.Map<SettingData>(await SettingsService.GetByNameAsync(settingName));
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
                var setting = await SettingsService.GetByNameAsync(settingName);
                setting.Value = value;
                await SettingsService.UpdateAsync(setting);

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
