using System.Reflection;
using System.Web.Hosting;
using System.Web.Http;
using log4net.Config;
using Microsoft.Owin;
using log4net;
using Newtonsoft.Json;
using Owin;

using ApiAdmin.Api.Handlers;
using ApiAdmin.IoC;
using ApiAdmin.IoC.Factory;
using ApiAdmin.Services;


[assembly: OwinStartup(typeof(ApiAdmin.Api.Startup))]
namespace ApiAdmin.Api
{
    public partial class Startup
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(Startup));

        public void Configuration(IAppBuilder app)
        {
            XmlConfigurator.Configure();
            _log.Info("Application starting...");
            ConfigureAuth(app);
            ConfigureIoC();
            GlobalConfiguration.Configuration.DependencyResolver = DependencyResolver.GetDependencyResolver();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            ConfigureFormatters();
            GlobalConfiguration.Configuration.MessageHandlers.Add(new OptionsHttpMessageHandler());
            AutomapperSetup.Initialize();
            _log.Info("Application has been started.");
        }

        private static void ConfigureIoC()
        {
            string diFilePath = HostingEnvironment.MapPath("~/App_Data/dependencies.xml");
            IDependencyResolverFactory factory = new AutofacDependencyResolverFactory(diFilePath, Assembly.GetExecutingAssembly());
            DependencyResolver.ConfigureResolver(factory);
        }

        private static void ConfigureFormatters()
        {
            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            jsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
        }
    }
}