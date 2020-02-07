using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Homer.Core.Internals.Services.Configuration;
using Homer.Core.Utils.IO;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Homer.Core.Internals.Logging
{
    /// <summary>
    /// Log manager.
    /// </summary>_
    public class LogManager : ILogManager
    {
        // the log root.
        private string _logRoot;

        // the service attached the logs.
        private readonly string _service;

        // console log template for master log.
        private const string MasterLogConsoleTemplate = "{Timestamp:HH:mm:ss} [{Level:u3}] [{Source:l}] {Message}{NewLine}{Exception}";

        // file log template for master log.
        private const string MasterLogFileTemplate = "{Timestamp} [{Level:u3}] [{Source:l}] {Message}{NewLine}{Exception}";

        // template for custom logs.
        private const string JobFileLoggerTemplate = "{Timestamp} [{Level:u3}] [{Source:l}] {Message}{NewLine}{Exception}";

        // console template for custom logs.
        private const string JobConsoleLoggerTemplate = "{Timestamp:HH:mm:ss} [{Level:u3}] [{Source:l}] {Message}{NewLine}{Exception}";

        // job loggers.
        private readonly IDictionary<string, ILogger> _pluginLoggers;

        // configuration service
        private readonly IConfigurationService _configurationService;

        public LogManager(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            _pluginLoggers = new Dictionary<string, ILogger>(); // list of custom job loggers.
            CheckLogDirectory(); // check the log directory exists, create it if needed.

            // get the project name
            var service = Assembly.GetEntryAssembly()?.GetName().Name;
            _service = service?.Substring(service.IndexOf('.', StringComparison.CurrentCulture) + 1);

            CreateMasterLogger(); // create the main logger.
        }

        /// <summary>
        /// Checks log directory and creates if it does not exist.
        /// </summary>
        private void CheckLogDirectory()
        {
            _logRoot = Path.Combine(FileHelper.AssemblyRoot, "logs"); // get log folder.
            if (!Directory.Exists(_logRoot)) Directory.CreateDirectory(_logRoot); // make sure log root exists, else create it.
        }

        /// <summary>
        /// Creates master logger.
        /// </summary>
        private void CreateMasterLogger()
        {
            // check for job logger's root path.
            var root = Path.Combine(_logRoot, _service.Replace('.', '/')); // get the root path.
            if (!Directory.Exists(root)) Directory.CreateDirectory(root); // if doesn't exist create it.
            var path = Path.Combine(root, $"task.log"); // get the path.

            var builder = new LoggerConfiguration()
                .Enrich.WithThreadId() // enrich with thread id.
                .Enrich.With(new SourceEnricher()) // used for enriching logs with sources.
                .MinimumLevel.Verbose(); // lower the default minimum level to verbose as sinks can only rise them but not lower.

            if (_configurationService.Configuration.Logs.Master.Console.Enabled) // if console log is enabled.
                builder = builder.WriteTo.Console(GetLogLevel(_configurationService.Configuration.Logs.Master.Console.Level), MasterLogConsoleTemplate, theme: AnsiConsoleTheme.Code);

            if (_configurationService.Configuration.Logs.Master.File.Enabled) // if file log is enabled.
                builder = builder.WriteTo.File(path, GetLogLevel(_configurationService.Configuration.Logs.Master.File.Level), MasterLogFileTemplate);

            Log.Logger = builder.CreateLogger(); // create the logger.
        }

        /// <inheritdoc />
        public ILogger GetPluginLogger(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            if (_pluginLoggers.ContainsKey(name)) // if the requested job logger already exists,
                return _pluginLoggers[name]; // return a reference to it.

            // check for job logger's root path.
            var root = Path.Combine(_logRoot, _service.Replace('.', '/')); // get the root path.
            if (!Directory.Exists(root)) Directory.CreateDirectory(root); // if doesn't exist create it.
            var path = Path.Combine(root, $"{name}.log"); // get the path.

            var builder = new LoggerConfiguration() // create the logger.
                .Enrich.With(new SourceEnricher()) // used for enriching logs with sources.
                .MinimumLevel.Verbose(); // lower the default minimum level to verbose as sinks can only rise them but not lower.

            if (_configurationService.Configuration.Logs.Jobs.Console.Enabled) // if console log is enabled.
                builder = builder.WriteTo.Console(GetLogLevel(_configurationService.Configuration.Logs.Jobs.Console.Level), JobConsoleLoggerTemplate, theme: AnsiConsoleTheme.Code);

            if (_configurationService.Configuration.Logs.Jobs.File.Enabled) // if file log is enabled.
                builder = builder.WriteTo.File(path, GetLogLevel(_configurationService.Configuration.Logs.Jobs.File.Level), JobFileLoggerTemplate);

            _pluginLoggers[name] = builder.CreateLogger(); // create the logger.

            return _pluginLoggers[name];
        }

        /// <summary>
        /// Encrichs the log 
        /// </summary>
        private class SourceEnricher : ILogEventEnricher
        {
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddPropertyIfAbsent(logEvent.Properties.Keys.Contains("SourceContext")
                    ? propertyFactory.CreateProperty("Source", logEvent.Properties["SourceContext"].ToString().Replace("\"", "", true, Culture.Default).Split('.').Last())
                    : propertyFactory.CreateProperty("Source", "n/a"));
            }
        }

        private LogEventLevel GetLogLevel(string input)
        {
            LogEventLevel level;
            switch (input)
            {
                case "verbose":
                    level = LogEventLevel.Verbose;
                    break;
                case "debug":
                    level = LogEventLevel.Debug;
                    break;
                case "info":
                    level = LogEventLevel.Information;
                    break;
                case "warning":
                    level = LogEventLevel.Warning;
                    break;
                case "error":
                    level = LogEventLevel.Error;
                    break;
                case "fatal":
                    level = LogEventLevel.Fatal;
                    break;
                default:
                    level = LogEventLevel.Verbose;
                    break;
            }

            return level;
        }
    }
}
