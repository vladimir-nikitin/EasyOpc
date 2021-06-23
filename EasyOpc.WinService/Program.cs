using System.ServiceProcess;
using System.Windows.Threading;

namespace EasyOpc.WinService
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var ServicesToRun = new ServiceBase[]
            {
                    new Service()
            };

            ServiceBase.Run(ServicesToRun);
            Dispatcher.Run();
        }
    }
}
