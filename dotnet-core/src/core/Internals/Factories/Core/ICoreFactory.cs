using Homer.Core.Internals.Logging;

namespace Homer.Core.Internals.Factories.Core
{
    /// <summary>
    /// Interface for core object factory.
    /// </summary>
    public interface ICoreFactory
    {
        /// <summary>
        /// Returns log manager.
        /// </summary>
        /// <returns><see cref="ILogManager"/></returns>
        ILogManager GetLogManager();
    }
}
