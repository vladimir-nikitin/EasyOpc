using AutoMapper;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Service.Base;
using EasyOpc.WinService.Modules.Setting.Repository.Contract;
using EasyOpc.WinService.Modules.Setting.Repository.Model;
using EasyOpc.WinService.Modules.Setting.Service.Contract;
using System;
using System.Threading.Tasks;
using SettingType = EasyOpc.WinService.Modules.Setting.Service.Model.Setting;

namespace EasyOpc.WinService.Modules.Setting.Service
{
    /// <summary>
    /// Setting service
    /// </summary>
    public class SettingService : BaseService<SettingType, SettingDto>, ISettingService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository">Setting repository</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="logger">Logger</param>
        public SettingService(ISettingRepository repository, IMapper mapper, ILogger logger)
            : base(repository, mapper, logger)
        {
        }

        /// <summary>
        /// <see cref="ISettingService.GetByNameAsync(string)"/>
        /// </summary>
        public async Task<SettingType> GetByNameAsync(string name)
        {
            try
            {
                return Mapper.Map<SettingType>(await (Repository as ISettingRepository).GetByNameAsync(name));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
