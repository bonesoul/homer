using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nacl;

namespace Homer.Platform.HomeKit.Accessories.Info
{
    public class AccessoryInfo : IAccessoryInfo
    {
        public string Username { get; }

        public string DisplayName { get; }

        public AccessoryCategory Category { get; }

        public string PinCode { get; }

        public string SecretKey { get; }

        public string PublicKey { get; }

        public IReadOnlyList<string> PairedClients { get; }

        public int ConfigVersion { get; }

        public string ConfigHash { get; }

        public string SetupId { get; }

        public bool RelayEnabled { get; }

        public int RelayState { get; }

        public string RelayAccessoryId { get; }

        public string RelayAdminId { get; }

        public IReadOnlyList<string> RelayPairedControllers { get; }

        public string BagUrl { get; }

        public AccessoryInfo(IAccessoryBase accessoryBase, dynamic info)
        {
            if (accessoryBase == null) throw new ArgumentNullException(nameof(accessoryBase));

            Username = info.username;

            DisplayName = accessoryBase.DisplayName;
            Category = accessoryBase.Category;
            PinCode = info.pin;

            // create secret & private keys.
            SecretKey = "GK4GzNY+fbkRPd5fwYUaca70iENh2A1QRss1KBtpWU4=";
            var publicKey = TweetNaCl.CryptoBoxKeypair(Convert.FromBase64String(SecretKey));
            PublicKey = Encoding.ASCII.GetString(publicKey);

            // generate setup id.
            SetupId = RandomString(4);
        }


        private static readonly Random _random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
