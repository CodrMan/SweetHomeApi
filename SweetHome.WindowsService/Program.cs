using System.ServiceProcess;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace SweetHome.WindowsService
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SweetHomeService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
