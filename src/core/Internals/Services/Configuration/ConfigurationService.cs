using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Homer.Core.Internals.Services.Configuration.Models;
using Homer.Core.Internals.Services.Runtime;
using Homer.Core.Utils.IO;
using Microsoft.Extensions.Configuration;
using NetEscapades.Configuration.Yaml;
using Serilog;

namespace Homer.Core.Internals.Services.Configuration
{
    /// <summary>
    /// Configuration service that loads config files and serves the data.
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        /// <inheritdoc />
        public IConfigurationModel Configuration { get; private set; }

        private IConfigurationRoot _builder;

        private readonly IRuntimeInfoService _runtimeInfoService;

        public ConfigurationService(IRuntimeInfoService runtimeInfoService)
        {
            _runtimeInfoService = runtimeInfoService ?? throw new ArgumentNullException(nameof(runtimeInfoService));
            Configuration = new ConfigurationModel();
        }

        /// <inheritdoc />
        public bool Load(string[] args)
        {
            try
            {
                var basePath = Path.Combine(FileHelper.AssemblyRoot, "config"); // base path for logs.

                _builder = new ConfigurationBuilder()
                    .SetBasePath(basePath) // set the base path.
                    .LoadCoreYamlConfiguration(_runtimeInfoService) // load core yaml configuration.
                    //.LoadPluginYamlConfiguration(_runtimeInfoService) // load services yaml configuration.
                    .AddEnvironmentVariables() // add environment variables.
                    .AddCommandLine(args) // add command line arguments.
                    .Build(); // build the configuration root.

                _builder.Bind(Configuration); // bind the configuration.

                return true;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"can not parse configuration: {e.Message} - {(e.InnerException != null ? e.InnerException.Message : "")}"); // we can't use the logging service yet.
                return false;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"cannot load configuration file: {e.FileName}"); // we can't use the logging service yet.
                return false;
            }
        }

        /// <inheritdoc />
        public void LogSummary()
        {
            var logger = Log.ForContext<ConfigurationService>(); // get the master logger reference.
            var files = _builder.Providers.OfType<YamlConfigurationProvider>().Select(yamlProvider => yamlProvider.Source.Path).ToList();
            logger.Information("loaded yaml configs: {Files:l}", files);
        }
    }

    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Loads yaml core yaml configuration.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="runtimeInfoService"></param>
        /// <returns></returns>
        public static IConfigurationBuilder LoadCoreYamlConfiguration(this IConfigurationBuilder builder, IRuntimeInfoService runtimeInfoService)
        {
            if (runtimeInfoService == null) throw new ArgumentNullException(nameof(runtimeInfoService));
            return builder.LoadYamlFiles(runtimeInfoService);
        }

        public static IConfigurationBuilder LoadPluginYamlConfiguration(this IConfigurationBuilder builder, IRuntimeInfoService runtimeInfoService)
        {
            if (runtimeInfoService == null) throw new ArgumentNullException(nameof(runtimeInfoService));

            // get services config path;
            var servicePath = $"{Assembly.GetEntryAssembly()?.GetName().Name}/";
            servicePath = servicePath?.Substring(servicePath.IndexOf('.', StringComparison.CurrentCulture) + 1);
            servicePath = servicePath?.Replace('.', '/');

            return servicePath == "testhost/"
                ? builder  // skip loading per-service configuration files while testing core project.
                : builder.LoadYamlFiles(runtimeInfoService, servicePath); // load per-service configuration.
        }

        private static IConfigurationBuilder LoadYamlFiles(this IConfigurationBuilder builder, IRuntimeInfoService runtimeInfoService, string root = "")
        {
            builder.AddYamlFile($"{root}default.yaml", false); // add default.yaml

            // handle test.yaml, development.yaml, production.yaml.
            switch (runtimeInfoService.BuildType)
            {
                case BuildType.Test:
                    builder.AddYamlFile($"{root}test.yaml", false); // if we are running within unit tests.
                    break;
                case BuildType.Development:
                    builder.AddYamlFile($"{root}development.yaml", false); // on debug builds, add development.yaml too.
                    break;
                case BuildType.Production:
                    builder.AddYamlFile($"{root}production.yaml", false); // on release builds, instead use production.yaml.
                    break;
            }

            builder.AddYamlFile($"{root}local.yaml", true); // if local.yaml exists add it to builder too.

            return builder;
        }
    }
}
