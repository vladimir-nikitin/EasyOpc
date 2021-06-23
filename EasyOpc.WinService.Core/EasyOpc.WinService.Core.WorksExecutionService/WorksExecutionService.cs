using EasyOpc.WinService.Core.WorksExecutionService.Contract;
using EasyOpc.WinService.Core.WorksService.Contract;
using EasyOpc.WinService.Modules.Settings.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using EasyOpc.Common.Constants;

namespace EasyOpc.WinService.Core.WorksExecutionService
{
    /// <summary>
    /// Works execution service
    /// </summary>
    public class WorksExecutionService : IWorksExecutionService
    {
        /// <summary>
        /// Settings service
        /// </summary>
        private ISettingsService SettingsService { get; }

        /// <summary>
        /// Work services
        /// </summary>
        private List<IWorksService> WorksServices { get; }

        /// <summary>
        /// Active task
        /// </summary>
        private Task ActiveTask { get; set; }

        /// <summary>
        /// Active works
        /// </summary>
        private List<IWork> ActiveWorks { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public WorksExecutionService(ISettingsService settingsService)
        {
            SettingsService = settingsService;
            WorksServices = new List<IWorksService>();
            ActiveWorks = new List<IWork>();
        }

        /// <inheritdoc cref="IWorksExecutionService.RegisterSource(IWorksService)"/>
        public void RegisterSource(IWorksService worksService)
        {
            WorksServices.Add(worksService);
        }

        /// <inheritdoc cref="IWorksExecutionService.StartAsync"/>
        public async Task StartAsync()
        {
            if (ActiveTask != null && !ActiveTask.IsCompleted)
            {
                await ActiveTask;
            }

            await(ActiveTask = GetStartTask());

            ActiveTask = null;
        }

        /// <inheritdoc cref="IWorksExecutionService.StopAsync"/>
        public async Task StopAsync()
        {
            if (ActiveTask != null && !ActiveTask.IsCompleted)
            {
                await ActiveTask;
            }

            await(ActiveTask = GetStopTask());

            ActiveTask = null;
        }

        private Task GetStartTask() => Task.Run(async () =>
        {
            var serviceModeSetting = await SettingsService.GetByNameAsync(WellKnownCodes.ServiceModeSettingName);
            serviceModeSetting.Value = false.ToString();
            var result = await SettingsService.UpdateAsync(serviceModeSetting);

            ActiveWorks.Clear();
            foreach (var worksService in WorksServices)
            {
                ActiveWorks.AddRange(await worksService.GetWorks());
            }

            foreach (var work in ActiveWorks)
            {
                try
                {
                    await work.StartAsync();
                    await Task.Delay(200);
                }
                catch { }
            }
        });

        private Task GetStopTask() => Task.Run(async () =>
        {
            var serviceModeSetting = await SettingsService.GetByNameAsync(WellKnownCodes.ServiceModeSettingName);
            serviceModeSetting.Value = true.ToString();
            var result = await SettingsService.UpdateAsync(serviceModeSetting);

            foreach (var work in ActiveWorks)
            {
                try
                {
                    await work.StopAsync();
                    await Task.Delay(200);
                }
                catch { }
            }
            ActiveWorks.Clear();
        });
    }
}
