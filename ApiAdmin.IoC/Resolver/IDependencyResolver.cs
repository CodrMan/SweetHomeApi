namespace ApiAdmin.IoC.Resolver
{
    /// <summary>
    ///     Provide resolve injetion
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        ///     Get implementation
        /// </summary>
        /// <typeparam name="TAbstaction">Abstration type</typeparam>
        /// <returns></returns>
        TAbstaction Get<TAbstaction>() where TAbstaction : class;
    }
}
