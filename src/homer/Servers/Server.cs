using System;
using System.Threading.Tasks;
using Homer.Platform.HomeKit.Bridges;
using Serilog;
using uuid.net.Generators.Abstract_Classes.Generator_Base;
using uuid.net.Static_Classes.UUID_Factory;

namespace Homer.Servers
{
    public class Server : IServer
    {
        public ILogger Logger { get; set; }

        public async Task RunAsync()
        {
            try
            {
                var generator = UUIDFactory.CreateGenerator(5, 1);
                var uuid = generator.Generate("homer");

                var bridge = new Bridge(uuid, "homer");
            }
            catch (Exception e)
            {
                Logger.Error("server initialization failed...");
            }
        }
    }
}
