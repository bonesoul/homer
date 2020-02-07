using System;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;

namespace Homer.Server.Servers
{
    public class BonjourServer: IBonjourServer
    {
        public ILogger Logger { get; }

        public BonjourServer()
        {
            Logger = Log.ForContext<BonjourServer>();
        }

        public async Task RunAsync()
        {
            try
            {
                Logger.Information("starting bonjour server..");
            }
            catch (Exception e)
            {
                Logger.Error("task failed..");
            }
        }
    }
}
