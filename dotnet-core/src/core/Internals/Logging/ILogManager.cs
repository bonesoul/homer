using Serilog;

namespace Homer.Core.Internals.Logging
{
    /// <summary>
    /// Interface for log manager.
    /// </summary>
    public interface ILogManager
    {
        /// <summary>
        /// Creates a separate task logger.
        /// </summary>
        /// <param name="name">name of the log file</param>
        /// <returns></returns>
        ILogger GetPluginLogger(string name);
    }
}
