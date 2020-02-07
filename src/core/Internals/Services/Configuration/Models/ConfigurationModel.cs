using Homer.Core.Internals.Services.Configuration.Models.Logs;

namespace Homer.Core.Internals.Services.Configuration.Models
{
    /// <summary>
    /// Base engine config data model.
    /// </summary>
    public class ConfigurationModel : IConfigurationModel
    {
        /// <inheritdoc />
        public LogsModel Logs { get; set; }
    }
}
