using EasyOpc.Common.Constant;
using EasyOpc.WinService.Core.Worker.Contract;
using EasyOpc.WinService.Modules.Setting.Service.Contract;
using EasyOpc.WinService.Modules.Work.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace EasyOpc.WinService.Modules.Work.Service
{
    /// <summary>
    /// Work execution service
    /// </summary>
    public class WorkExecutionService : IWorkExecutionService
    {
        /// <summary>
        /// Active task
        /// </summary>
        private Task ActiveTask { get; set; }

        /// <summary>
        /// DI container
        /// </summary>
        private IUnityContainer Container { get; }

        /// <summary>
        /// Work service
        /// </summary>
        private IWorkService WorkService { get; }

        /// <summary>
        /// Settings service
        /// </summary>
        private ISettingService SettingService { get; }

        /// <summary>
        /// Workers list
        /// </summary>
        private List<IWorker> Workers { get; } = new List<IWorker>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">DI container</param>
        /// <param name="workService">Work service</param>
        /// <param name="settingService">Settings service</param>
        public WorkExecutionService(IUnityContainer container, IWorkService workService, ISettingService settingService)
        {
            Container = container;
            WorkService = workService;
            SettingService = settingService;
        }

        /// <summary>
        /// <see cref="IWorkExecutionService.StartAsync"/>
        /// </summary>
        public async Task StartAsync()
        {
           if(ActiveTask != null && !ActiveTask.IsCompleted)
           {
                await ActiveTask;
           }

            await (ActiveTask = GetStartTask());

            ActiveTask = null;
        }

        /// <summary>
        /// <see cref="IWorkExecutionService.StopAsync"/>
        /// </summary>
        public async Task StopAsync()
        {
            if (ActiveTask != null && !ActiveTask.IsCompleted)
            {
                await ActiveTask;
            }

            await (ActiveTask = GetStopTask());

            ActiveTask = null;
        }

        private Task GetStartTask() => Task.Run(async () =>
        {
            var serviceModeSetting = await SettingService.GetByNameAsync(WellKnownCodes.ServiceModeSettingName);
            serviceModeSetting.Value = false.ToString();
            var result = await SettingService.UpdateAsync(serviceModeSetting);

            var works = await WorkService.GetAllAsync();
            foreach (var work in works)
            {
                if (!work.IsEnabled) continue;

                var worker = (IWorker)Container.Resolve(Type.GetType(work.Type));
                if (worker == null) continue;

                Workers.Add(worker);
                await worker.StartAsync(work);
            }

            /*IWorker worker;
            List<Task> startTasks;
            foreach (var workGroup in works.GroupBy(w => w.Order))
            {
                startTasks = new List<Task>();
                foreach (var work in workGroup)
                {
                    if (!work.IsEnabled) continue;

                    worker = (IWorker)Container.Resolve(Type.GetType(work.Type));

                    if (worker == null) continue;

                    Workers.Add(worker);
                    startTasks.Add(worker.StartAsync(work));
                }

                await Task.WhenAll(startTasks);
            }*/
        });

        private Task GetStopTask() => Task.Run(async () =>
        {
            var serviceModeSetting = await SettingService.GetByNameAsync(WellKnownCodes.ServiceModeSettingName);
            serviceModeSetting.Value = true.ToString();
            var result = await SettingService.UpdateAsync(serviceModeSetting);

            foreach (var worker in Workers.ToList())
            {
                await worker.StopAsync();
                await Task.Delay(200);
            }

            Workers.Clear();

            /*var stopTasks = new List<Task>();
            lock (Workers)
            {
                stopTasks = Workers.Select(worker => worker.StopAsync()).ToList();
                Workers.Clear();
            }

            await Task.WhenAll(stopTasks);*/
        });
    }
}
