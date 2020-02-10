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

using System;
using System.Linq;
using System.Threading.Tasks;
using Homer.Core.Host;
using Homer.Platform.HomeKit.Bridges.Setup;
using Makaretu.Dns;
using Serilog;
using uuid.net.Static_Classes.UUID_Factory;

namespace Homer.Servers
{
    public class Server : IServer
    {
        private ILogger _logger;

        public Server()
        {
            _logger = Log.ForContext<Server>();
        }

        public async Task RunAsync()
        {
            try
            {
                var generator = UUIDFactory.CreateGenerator(5, 1);
                var uuid = generator.Generate("homer");

                var bridge =  new BridgeSetupManager(uuid, "homer");

                var mdns = new MulticastService();

                mdns.QueryReceived += (s, e) =>
                {
                    var names = e.Message.Questions.Select(q => q.Name + " " + q.Type);
                    _logger.Information($"got a query for {string.Join(", ", names)}");
                };
                mdns.AnswerReceived += (s, e) =>
                {
                    var names = e.Message.Answers.Select(q => q.Name + " " + q.Type).Distinct();
                    _logger.Information($"got answer for {string.Join(", ", names)}");
                };
                mdns.NetworkInterfaceDiscovered += (s, e) =>
                {
                    foreach (var nic in e.NetworkInterfaces)
                    {
                        _logger.Information($"discovered NIC '{nic.Name}'");
                    }
                };

                foreach (var a in MulticastService.GetIPAddresses())
                {
                    _logger.Information($"IP address {a}");
                }

                var serviceDiscovery = new ServiceDiscovery(mdns);
                var hap = new ServiceProfile("hap", "_hap._tcp", 5012);
                serviceDiscovery.Advertise(hap);
                mdns.Start();
            }
            catch (Exception e)
            {
                _logger.Error("server initialization failed...");
            }
        }
    }
}
