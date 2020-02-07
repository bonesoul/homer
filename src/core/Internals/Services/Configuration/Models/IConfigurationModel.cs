using Homer.Core.Internals.Services.Configuration.Models.Logs;

namespace Homer.Core.Internals.Services.Configuration.Models
{
    /// <summary>
    /// Interface for config data.
    /// </summary>
    public interface IConfigurationModel
    {
        /// <summary>
        /// Logs configuration.
        /// </summary>
        LogsModel Logs { get; set; }
    }
}
