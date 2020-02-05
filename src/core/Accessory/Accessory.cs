using System;
using System.Collections.Generic;
using System.Text;
using DaanV2.UUID;
using Smarty.Core.Bridge;

namespace Smarty.Core.Accessory
{
    public class Accessory : IAccessory
    {
        /// <inheritdoc />
        public UUID Id { get; }

        /// <inheritdoc />
        public bool IsBridged { get; }

        /// <inheritdoc />
        public bool IsReacheable { get; }

        /// <inheritdoc />
        public IBridge ParentBridge { get; }

        public Accessory()
        {

        }
    }
}
