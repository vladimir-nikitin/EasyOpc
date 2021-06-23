using EasyOpc.WinService.Core.WorksService.Contract;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Core.WorksExecutionService.Contract
{
    public interface IWorksExecutionService
    {
        void RegisterSource(IWorksService worksService);

        Task StartAsync();

        Task StopAsync();
    }
}
