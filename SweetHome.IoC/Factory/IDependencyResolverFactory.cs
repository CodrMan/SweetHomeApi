using SweetHome.IoC.Resolver;

namespace SweetHome.IoC.Factory
{
    /// <summary>
    ///     Factory for DependencyResolver
    /// </summary>
    public interface IDependencyResolverFactory
    {
        /// <summary>
        ///     Configure and create dependency resolver
        /// </summary>
        void CreateResolver();

        /// <summary>
        ///     Get dependency resolver instance
        /// </summary>
        /// <returns></returns>
        IDependencyResolver GetResolver();

        /// <summary>
        ///     Get dependency resolver instance for ApiController
        /// </summary>
        /// <returns></returns>
        System.Web.Http.Dependencies.IDependencyResolver GetApiResolver();
    }
}
