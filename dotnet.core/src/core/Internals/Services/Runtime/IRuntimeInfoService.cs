namespace Homer.Core.Internals.Services.Runtime
{
    /// <summary>
    /// Runtime info service.
    /// </summary>
    public interface IRuntimeInfoService : IService
    {
        /// <summary>
        /// Returns build type.
        /// </summary>
        BuildType BuildType { get; }

        /// <summary>
        /// The dotnet core version we are running on.
        /// </summary>
        string DotnetCoreRuntimeVersion { get; }

        /// <summary>
        /// Are we running within a unit-test session?
        /// </summary>
        /// <returns></returns>
        bool IsRunningUnitTests { get; }

        /// <summary>
        /// Returns operating system.
        /// </summary>
        OperatingSystem OperatingSystem { get; }

        /// <summary>
        /// Returns the name of the operating system.
        /// </summary>
        string OperatingSystemName { get; }

        /// <summary>
        /// prints information about runtime.
        /// </summary>
        void PrintBanner();
    }
}
