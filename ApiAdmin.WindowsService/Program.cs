using System.ServiceProcess;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace ApiAdmin.WindowsService
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ApiAdminService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
