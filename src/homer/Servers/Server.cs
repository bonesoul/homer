using System;
using System.Threading.Tasks;
using Serilog;

namespace Homer.Servers
{
    public class Server : IServer
    {
        public ILogger Logger { get; set; }

        public async Task RunAsync()
        {
            throw new NotImplementedException();
        }
    }
}
