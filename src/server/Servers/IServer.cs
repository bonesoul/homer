using System.Threading.Tasks;
using Serilog;

namespace Homer.Server.Servers
{
    public interface IServer
    {
        /// <summary>
        /// Master logger instance.
        /// </summary>
        ILogger Logger { get;}

        /// <summary>
        /// Runs the job.
        /// </summary>
        /// <returns></returns>
        Task RunAsync();
    }
}
