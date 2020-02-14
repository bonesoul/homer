using System.Collections.Generic;

namespace Homer.Platform.HomeKit.Accessories.Info
{
    /// <summary>
    /// Internal data for accessory that get persisted to disk.
    /// </summary>
    public interface IAccessoryInfo
    {
        string Username { get; }

        string DisplayName { get; }

        AccessoryCategory Category { get; }

        string PinCode { get; }

        string SecretKey { get; }

        string PublicKey { get; }

        IReadOnlyList<string> PairedClients { get; }

        int ConfigVersion { get; }

        string ConfigHash { get; }

        string SetupId { get; }

        bool RelayEnabled { get; }

        int RelayState { get; }

        string RelayAccessoryId { get; }

        string RelayAdminId { get; }

        IReadOnlyList<string> RelayPairedControllers { get; }

        string BagUrl { get; }
    }
}
