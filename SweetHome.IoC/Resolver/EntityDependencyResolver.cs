using Autofac.Integration.WebApi;

namespace SweetHome.IoC.Resolver
{
    public class EntityDependencyResolver : IDependencyResolver
    {
        private AutofacWebApiDependencyResolver _resolver;
        /// <summary>
        ///     .ctor
        /// </summary>
        /// <param name="resolver">Autofac kernel instance</param>
        /// <exception cref="System.ArgumentNullException">Kernel cant not be null</exception>
        public EntityDependencyResolver(AutofacWebApiDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        ///     Get implementation for type
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type</typeparam>
        /// <returns></returns>
        public TAbstraction Get<TAbstraction>() where TAbstraction : class
        {
            return (TAbstraction)_resolver.GetService(typeof(TAbstraction));
        }

        /// <summary>
        ///     Dispose object
        /// </summary>
        public void Dispose()
        {
            _resolver = null;
        }
    }
}
