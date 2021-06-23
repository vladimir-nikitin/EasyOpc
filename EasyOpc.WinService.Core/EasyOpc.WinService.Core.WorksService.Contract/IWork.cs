using System.Threading.Tasks;

namespace EasyOpc.WinService.Core.WorksService.Contract
{
    public interface IWork
    {
        string Name { get; }

        string LaunchGroup { get; }

        Task StartAsync();

        Task StopAsync();
    }
}
