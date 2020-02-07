using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Homer.Core.Internals;
using Homer.Core.Internals.Bootstrap;
using Homer.Core.Internals.Factories.Core;
using Homer.Core.Internals.Registries;
using Homer.Core.Internals.Services.Configuration;
using Homer.Core.Internals.Services.Runtime;
using Serilog;
using Stashbox;

namespace Homer.Core.Host
{
    /// <summary>
    /// Service host.
    /// </summary>
    public class ServerHost : IServerHost
    {
        private readonly Bootstrapper _bootstrapper;

        private ILogger _logger; // logger to use with unhandled exceptions.

        public ServerHost(Bootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper;
        }

        /// <inheritdoc />
        public async Task InitializeAsync(IReadOnlyList<IRegistry> registries, string[] args, bool needsNetworkingSupport = false)
        {
            // init unhandled exception handler.
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler; // Catch any unhandled exceptions if we are in release mode.

            // setup default culture.
            CultureInfo.DefaultThreadCurrentCulture = Culture.Default;
            CultureInfo.DefaultThreadCurrentUICulture = Culture.Default;

            // init bootstrapper.
            _bootstrapper.Run(registries.ToList());

            // load the runtime service.
            var runtimeInfoService = _bootstrapper.Container.Resolve<IRuntimeInfoService>();

            // load the configuration.
            var configurationService = _bootstrapper.Container.Resolve<IConfigurationService>();
            if (!configurationService.Load(args)) Environment.Exit(-1); // if we can't load the config, exit the process.

            // init the logger.
            var coreFactory = _bootstrapper.Container.Resolve<ICoreFactory>(); // get core object factory.
            coreFactory.GetLogManager(); // resolve log manager, so it gets internalized.
            _logger = Log.ForContext<ServerHost>(); // create logger to be used later.

            // print startup banner & info.
            runtimeInfoService.PrintBanner(); // print startup banner.
            configurationService.LogSummary(); // log the configuration summary.
        }

        /// <summary>
        /// Unhandled exception emitter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            if (!(e.ExceptionObject is Exception exception)) // if we can't get the exception, whine about it.
            {
                _logger.Error("can't get exception object from UnhandledExceptionEventArgs");
                throw new ArgumentNullException(nameof(e));
            }

            if (!e.IsTerminating)
            {
                _logger.Fatal(exception, "terminating because of unhandled exception: {Message:l}", exception.Message);
            }
            else
            {
                _logger.Error(exception, "caught unhandled exception: {Message:l}", exception.Message);

#if !DEBUG
                    Environment.Exit(-1); // prevent console window from being closed when we are in development mode.
#endif
            }
        }
    }
}
