using System.Threading.Tasks;

namespace EasyOpc.WinService.Modules.Work.Service.Contract
{
    /// <summary>
    /// Contract for the work execution service
    /// </summary>
    public interface IWorkExecutionService
    {
        /// <summary>
        /// Start execution works
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// Stop execution works
        /// </summary>
        Task StopAsync();
    }
}
