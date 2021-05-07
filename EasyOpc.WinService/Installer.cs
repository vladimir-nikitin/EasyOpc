using EasyOpc.Common.Constant;
using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace EasyOpc.WinService
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;

        public Installer()
        {
            InitializeComponent();

            //C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /user=Nec /password=863368 E:\Upwork\Matrikon\SitOpc\SitOpc.Service\bin\Release\SitOpc.Service.exe
            //sc delete SitOpcWinService

            this.BeforeInstall += Installer_BeforeInstall;
        }

        void Installer_BeforeInstall(object sender, InstallEventArgs e)
        {
            //string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Application.AppName;
            //try { new OpcSettingsDll.File().CreateDirectory(appData); }
            //catch { }

            //Log log = new Log("InstallServiceLog", appData);
            //log.AddRecord("#InstallServiceLog");

            string user = null;
            string pass = null;

            try
            {
                user = base.Context.Parameters["user"].ToString();
                if (!user.Contains("\\"))
                    user = @".\" + user;
                Console.WriteLine("#User=" + user);
                pass = base.Context.Parameters["password"].ToString();
                Console.WriteLine("#Pass=" + pass);
            }
            catch { return; }

            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();

            if (String.IsNullOrEmpty(user) || String.IsNullOrEmpty(pass))
            {
                processInstaller.Account = ServiceAccount.LocalSystem;
            }
            else
            {
                processInstaller.Account = ServiceAccount.User;
                processInstaller.Username = user;
                processInstaller.Password = pass;
            }

            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = WellKnownCodes.ServiceName;
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
