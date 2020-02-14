namespace Homer.Core.Internals.Registries
{
    /// <summary>
    /// Interface for IoC container registries.
    /// </summary>
    public interface IRegistry
    {
        /// <summary>
        /// Attach the registry so that it can register it's members.
        /// </summary>
        void Attach();
    }
}
