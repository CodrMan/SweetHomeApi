using System;
using log4net;

using ApiAdmin.IoC.Factory;
using ApiAdmin.IoC.Resolver;


namespace ApiAdmin.IoC
{
    /// <summary>
    ///     Provide DI relation
    /// </summary>
    public static class DependencyResolver
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DependencyResolver));
        private static readonly object SetLocker = new object();
        private static IDependencyResolverFactory _dependencyResolverFactory;
        private static volatile IDependencyResolver _resolver;
        private static volatile bool _isInited;

        /// <summary>
        ///     Configure dependency resolver implementation
        /// </summary>
        /// <param name="resolverFactory"> </param>
        /// <exception cref="NullReferenceException">Resolver factory  can't be null</exception>
        /// <exception cref="InvalidOperationException">DependencyResolver all ready configured</exception>
        public static void ConfigureResolver(IDependencyResolverFactory resolverFactory)
        {
            if (resolverFactory == null)
            {
                throw new NullReferenceException("Resolver factory can't be null");
            }

            if (_isInited)
            {
                throw new InvalidOperationException("DependencyResolver allready configured");
            }

            InitResolverFactory(resolverFactory);
        }

        /// <summary>
        ///     Get dependency
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction type</typeparam>
        /// <returns>Binded implementation for abstraction</returns>
        /// <exception cref="System.InvalidOperationException">DependencyResolver if not configured</exception>
        public static TAbstraction Resolve<TAbstraction>() where TAbstraction : class
        {
            if (_isInited == false)
            {
                throw new InvalidOperationException("DependencyResolver is not configured");
            }

            return _resolver.Get<TAbstraction>();
        }

        /// <summary>
        ///     Get dependency for ApiController
        /// </summary>
        public static System.Web.Http.Dependencies.IDependencyResolver GetDependencyResolver()
        {
            return _dependencyResolverFactory.GetApiResolver();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public static void Dispose()
        {
            lock (SetLocker)
            {
                _isInited = false;
                _resolver = null;
                _dependencyResolverFactory = null;
            }
        }

        /// <summary>
        ///     Initialize resolver factory
        /// </summary>
        /// <param name="resolverFactory"></param>
        private static void InitResolverFactory(IDependencyResolverFactory resolverFactory)
        {
            lock (SetLocker)
            {
                _dependencyResolverFactory = resolverFactory;
                _dependencyResolverFactory.CreateResolver();
                _resolver = _dependencyResolverFactory.GetResolver();
                _isInited = true;
            }

            Log.Info(String.Format("Configuration complete. DependencyResolverFactory is: {0}", _resolver));
        }
    }
}
