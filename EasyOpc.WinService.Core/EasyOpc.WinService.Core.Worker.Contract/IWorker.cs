using System.Threading.Tasks;
using WorkType = EasyOpc.WinService.Core.Worker.Model.Work;

namespace EasyOpc.WinService.Core.Worker.Contract
{
    /// <summary>
    /// Worker contract
    /// </summary>
    public interface IWorker
    {
        /// <summary>
        /// Start worker
        /// </summary>
        /// <param name="work">Work</param>
        /// <returns></returns>
        Task StartAsync(WorkType work);

        /// <summary>
        /// Stop worker
        /// </summary>
        Task StopAsync();
    }
}
