using Stashbox;

namespace Homer.Core.Internals.Bootstrap
{
    /// <summary>
    /// Interface for IoC container kernel bootstrapper
    /// </summary>
    public interface IBootstrapper
    {
        /// <summary>
        /// The IoC container
        /// </summary>
        StashboxContainer Container { get; }
    }
}
