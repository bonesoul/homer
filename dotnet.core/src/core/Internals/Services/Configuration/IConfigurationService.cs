using Homer.Core.Internals.Services.Configuration.Models;

namespace Homer.Core.Internals.Services.Configuration
{
    /// <summary>
    /// Interface for configuration service that loads config files and serves the data.
    /// </summary>
    public interface IConfigurationService : IService
    {
        /// <summary>
        /// The configuration data.
        /// </summary>
        IConfigurationModel Configuration { get; }

        /// <summary>
        /// Loads configuration;
        /// </summary>
        /// <param name="args"></param>
        bool Load(string[] args);

        /// <summary>
        /// Logs the configuration summary.
        /// </summary>
        void LogSummary();
    }
}
