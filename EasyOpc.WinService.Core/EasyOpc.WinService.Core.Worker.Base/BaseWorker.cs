using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Worker.Contract;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WorkType = EasyOpc.WinService.Core.Worker.Model.Work;

namespace EasyOpc.WinService.Core.Worker.Base
{
    /// <summary>
    /// Base worker
    /// </summary>
    /// <typeparam name="TSettings">Settings type</typeparam>
    public abstract class BaseWorker<TSettings> : IWorker
    {
        /// <summary>
        /// Logger
        /// </summary>
        public virtual ILogger Logger { get; private set; }

        /// <summary>
        /// Worker settings
        /// </summary>
        public virtual TSettings Settings { get; private set; }

        public BaseWorker(ILogger logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// <see cref="IWorker.StartAsync"/>
        /// </summary>
        public abstract Task StartAsync(WorkType work);

        /// <summary>
        /// <see cref="IWorker.StopAsync"/>
        /// </summary>
        public abstract Task StopAsync();

        /// <summary>
        /// Set settings
        /// </summary>
        /// <param name="jsonSettings">Settings in json format</param>
        protected virtual void SetSetting(string jsonSettings) => 
            Settings = JsonConvert.DeserializeObject<TSettings>(jsonSettings);
        
    }
}
