using Homer.Core.Internals.Services.Configuration.Models.Logs;
using Homer.Core.Internals.Services.Configuration.Models.Platforms;

namespace Homer.Core.Internals.Services.Configuration.Models
{
    /// <summary>
    /// Base engine config data model.
    /// </summary>
    public class ConfigurationModel : IConfigurationModel
    {
        /// <inheritdoc />
        public LogsModel Logs { get; set; }

        /// <inheritdoc />
        public PlatformsModel Platforms { get; set; }
    }
}
