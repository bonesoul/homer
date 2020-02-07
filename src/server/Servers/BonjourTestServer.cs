using System;
using System.Threading.Tasks;
using ArkaneSystems.Arkane.Zeroconf;
using Serilog;

namespace Homer.Server.Servers
{
    public class BonjourTestServer: IBonjourTestServer
    {
        public ILogger Logger { get; }

        private RegisterService _service;

        private TxtRecord _txtRecord;

        public BonjourTestServer()
        {
            Logger = Log.ForContext<BonjourTestServer>();

            _service = new RegisterService
            {
                Name = "homer",
                RegType = "_hap._tcp",
                ReplyDomain = "local.",
                Port = 3689
            };

            // for details
            // https://github.com/KhaosT/HAP-NodeJS/blob/75913a6e5fb8ef99d36a21057da7f1452955e7fb/src/lib/Advertiser.ts#L38
            _txtRecord = new TxtRecord
            {
                {"md", "homer"}, // display name.
                {"c#", "1"}, // "accessory conf" - represents the "configuration version" of an Accessory. Increasing this "version number" signals iOS devices to re-fetch /accessories data.
                {"ff", "0"},
                {"id", "22:32:43:54:54:01"}, // username??
                {"pv", "1.0"}, // protocolVersion,
                {"s#", "1"},  // "accessory state"
                {"sf", "1"}, // "sf == 1" means "discoverable by HomeKit iOS clients"
                {"ci", "2"} // category
                // "sh": this._setupHash
            };

            _service.TxtRecord = _txtRecord;

            _service.Response += _service_Response;
        }

        public async Task RunAsync()
        {
            try
            {
                Logger.Information("starting bonjour server..");
                _service.Register();
                //Browse();
            }
            catch (Exception e)
            {
                Logger.Error("task failed..");
            }
        }

        private void Browse()
        {
            var browser = new ServiceBrowser();

            browser.ServiceAdded += delegate (object o, ServiceBrowseEventArgs aargs)
            {
                Logger.Information("Found Service: {0}", aargs.Service.Name);

                aargs.Service.Resolved += delegate (object oo, ServiceResolvedEventArgs argss)
                {
                    IResolvableService s = (IResolvableService)argss.Service;
                    Console.WriteLine("Resolved Service: {0} - {1}:{2} ({3} TXT record entries)",
                        s.FullName, s.HostEntry.AddressList[0], s.Port, s.TxtRecord.Count);
                };

                aargs.Service.Resolve();
            };

            browser.Browse("_hap._tcp", "local");
        }

        private void _service_Response(object o, RegisterServiceEventArgs args)
        {
            switch (args.ServiceError)
            {
                case ServiceErrorCode.NameConflict:
                    Logger.Error("*** Name Collision! '{0}' is already registered",
                        args.Service.Name);
                    break;
                case ServiceErrorCode.None:
                    Logger.Information("*** Registered name = '{0}'", args.Service.Name);
                    break;
                case ServiceErrorCode.Unknown:
                    Logger.Error("*** Error registering name = '{0}'", args.Service.Name);
                    break;
            }
        }
    }
}
