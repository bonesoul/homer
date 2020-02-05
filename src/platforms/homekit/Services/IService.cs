using DaanV2.UUID;

namespace Homer.Platform.HomeKit.Services
{
    /// <summary>
    /// Interface for HomeKit services.
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// UUID of the service.
        /// </summary>
        public UUID Uuid { get; }

        /// <summary>
        /// Display name of the service.
        /// </summary>
        public string DisplayName { get; }
    }
}
