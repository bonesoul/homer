#region license
// 
//     homer - The complete home automation for Homer Simpson.
//     Copyright (C) 2020, Hüseyin Uslu - shalafiraistlin at gmail dot com
//     https://github.com/bonesoul/homer
// 
//      “Commons Clause” License Condition v1.0
//
//      The Software is provided to you by the Licensor under the License, as defined below, subject to the following condition.
//  
//      Without limiting other conditions in the License, the grant of rights under the License will not include, and the License
//      does not grant to you, the right to Sell the Software.
//
//      For purposes of the foregoing, “Sell” means practicing any or all of the rights granted to you under the License to provide
//      to third parties, for a fee or other consideration (including without limitation fees for hosting or consulting/ support
//      services related to the Software), a product or service whose value derives, entirely or substantially, from the functionality
//      of the Software.Any license notice or attribution required by the License must also include this Commons Clause License
//      Condition notice.
//
//      License: MIT License
//      Licensor: Hüseyin Uslu
#endregion

using System.Collections.Generic;
using System.Threading.Tasks;
using Homer.Core.Host;
using Homer.Core.Internals.Bootstrap;
using Homer.Core.Internals.Registries;
using Homer.Core.Internals.Services.Configuration;
using Homer.Server.Internals;
using Homer.Server.Servers;
using Stashbox;

namespace Homer.Server
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var bootstrapper = new Bootstrapper(); // IoC kernel bootstrapper.
            var host = new ServerHost(bootstrapper); // server host.

            // setup registries
            var registries = new List<IRegistry>
            {
                new Internals.ServerRegistry(bootstrapper.Container)
            };

            // initialize service host.
            await host.InitializeAsync(registries, args);

            // initialize bonjour server.
            var bonjourServer = bootstrapper.Container.Resolve<IBonjourServer>(); // resolve bonjour service.
            await bonjourServer.RunAsync();
        }
    }
}
