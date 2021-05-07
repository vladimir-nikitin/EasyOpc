using EasyOpc.Common.Constant;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Threading;

namespace EasyOpc.WinService
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main(string[] args)
        {
            //var appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //File.AppendAllLines("D:\\Log.txt", new List<string> { $"appDataDirectory: {appDataDirectory}" });

            /*var dbFileDirectory = $"{appDataDirectory}\\EasyOPC";
            var dbFilePath = $"{dbFileDirectory}\\LocalDataBase.db";
            if (!File.Exists(dbFilePath))
            {
                Directory.CreateDirectory(dbFileDirectory);
                var appDirectory = (Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                File.Copy($"{appDirectory}\\Data\\LocalDataBase.db", dbFilePath);
            }*/

            var appDirectory = (Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            var dbFilePath = $"{appDirectory}\\DataBase.db";
            if (!File.Exists(dbFilePath))
            {
                File.Copy($"{appDirectory}\\LocalDataBase.db", dbFilePath);
            }
            
            AppDomain.CurrentDomain.SetData("DataDirectory", appDirectory);

            var desktopMode = args != null && args.Any(p => p == "-d");

            var result = "";
            try
            {
                result = $"CoInitializeSecurity: {Com.CoInitializeSecurity()}";
            }
            catch (Exception ex)
            {
                result = ($"CoInitializeSecurityException: {ex.Message}");
            }

            UnityConfig.RegisterComponents();

            if (desktopMode)
            {
                new Service().Start();
            }
            else
            {
                var ServicesToRun = new ServiceBase[]
                {
                    new Service()
                };

                ServiceBase.Run(ServicesToRun);
            }

            Dispatcher.Run();
        }
    }
}
