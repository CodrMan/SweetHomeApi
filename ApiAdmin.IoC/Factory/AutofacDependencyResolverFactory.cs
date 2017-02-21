using System.Reflection;
using Autofac;
using Autofac.Configuration;
using Autofac.Integration.WebApi;

using ApiAdmin.IoC.Resolver;


namespace ApiAdmin.IoC.Factory
{
    /// <summary>
    ///     Ninject resolver for broadcast
    /// </summary>
    public class AutofacDependencyResolverFactory : IDependencyResolverFactory
    {
        private readonly string _diFilePath;
        private readonly Assembly _assembly;
        private AutofacWebApiDependencyResolver _resolver;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public AutofacDependencyResolverFactory(string diFilePath, Assembly ass)
        {
            _diFilePath = diFilePath;
            _assembly = ass;
        }


        /// <summary>
        ///     Configure and create dependency resolver
        /// </summary>
        public void CreateResolver()
        {
            var builder = new ContainerBuilder();
            var configReader = new ConfigurationSettingsReader("autofac-dependencies", _diFilePath);
            builder.RegisterModule(configReader);
            builder.RegisterApiControllers(_assembly);

            var container = builder.Build();
            _resolver = new AutofacWebApiDependencyResolver(container);
        }

        /// <summary>
        ///     Get dependency resolver instance
        /// </summary>
        /// <returns></returns>
        public IDependencyResolver GetResolver()
        {
            return new EntityDependencyResolver(_resolver);
        }

        /// <summary>
        ///     Get dependency resolver instance for ApiController
        /// </summary>
        /// <returns></returns>
        public System.Web.Http.Dependencies.IDependencyResolver GetApiResolver()
        {
            return _resolver;
        }
    }
}
