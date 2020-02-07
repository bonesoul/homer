using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Extensions.PlatformAbstractions;
using Serilog;

namespace Homer.Core.Internals.Services.Runtime
{
    /// <summary>
    /// Runtime info service
    /// </summary>
    public class RuntimeInfoService : IRuntimeInfoService
    {
        /// <inheritdoc />
        public BuildType BuildType
        {
            get
            {
                if (IsRunningUnitTests) return BuildType.Test; // check for
#if DEBUG
                return BuildType.Development;
#else
                return BuildType.Production;
#endif
            }
        }

        /// <inheritdoc />
        public string DotnetCoreRuntimeVersion
        {
            get
            {
                var assembly = typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly;
                var assemblyPath = assembly.CodeBase.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                var netCoreAppIndex = Array.IndexOf(assemblyPath, "Microsoft.NETCore.App");

                if (netCoreAppIndex > 0 && netCoreAppIndex < assemblyPath.Length - 2)
                    return assemblyPath[netCoreAppIndex + 1];

                return null;
            }
        }

        /// <inheritdoc />
        public bool IsRunningUnitTests => AppDomain.CurrentDomain.GetAssemblies().Any(x => x.FullName.StartsWith("xunit.", true, Culture.Default));

        /// <inheritdoc />
        public OperatingSystem OperatingSystem
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return OperatingSystem.Windows;
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return OperatingSystem.Linux;
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return OperatingSystem.MacOS;
                else return OperatingSystem.Unknown;
            }
        }

        /// <inheritdoc />
        public string OperatingSystemName
        {
            get
            {
                switch (OperatingSystem)
                {
                    case OperatingSystem.Windows:
                        return "win";
                    case OperatingSystem.Linux:
                        return "linux";
                    case OperatingSystem.MacOS:
                        return "macos";
                    case OperatingSystem.Unknown:
                    default:
                        return "unknown";
                }
            }
        }

        /// <inheritdoc />
        public void PrintBanner()
        {
            var project = Assembly.GetEntryAssembly()?.GetName().Name; // get the calling cli project's name.
            var logger = Log.ForContext<IRuntimeInfoService>();

            var version = GitVersionInformation.FullSemVer;
            var platform = RuntimeInformation.ProcessArchitecture.ToString().ToLower(Culture.Default);
            var architecture = RuntimeInformation.ProcessArchitecture.ToString().ToLower(Culture.Default);
            var osDescription = RuntimeInformation.OSDescription.Trim();

            logger.Information("------------------------------------------------------");
            logger.Information("homer, Copyright (c) 2020, Hüseyin Uslu");
            logger.Information("------------------------------------------------------");
            logger.Information("{Project:l} warming-up: {Version:l} [{ProcessArchitecture:l}]", project, version, platform);
            logger.Information("running on {Os:l} [{Description:l}]", $"{OperatingSystemName}-{architecture}", osDescription);
            logger.Information("dotnet core {CoreVersion:l}, runtime: {Runtime:l}", PlatformServices.Default.Application.RuntimeFramework.Version, DotnetCoreRuntimeVersion);
            logger.Information("running over {ProcessorCount} core system", Environment.ProcessorCount);
            logger.Information("------------------------------------------------------");
        }
    }

    /// <summary>
    /// Build types enum.
    /// </summary>
    public enum BuildType
    {
        Development,
        Production,
        Test
    }

    /// <summary>
    /// operating systems enum.
    /// </summary>
    public enum OperatingSystem
    {
        Unknown = 0,
        Windows = 1,
        Linux = 2,
        MacOS = 3
    }
}
