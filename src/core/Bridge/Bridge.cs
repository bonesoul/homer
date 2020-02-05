using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using DaanV2.UUID;
using Smarty.Core.Accessory;

namespace Smarty.Core.Bridge
{
    public class Bridge : IBridge
    {
        const int MaxAccessories = 149; // Maximum number of bridged accessories per bridge.

        /// <inheritdoc />
        public UUID Id { get; }

        /// <inheritdoc />
        public bool IsBridged { get; }

        /// <inheritdoc />
        public bool IsReacheable { get; }

        /// <inheritdoc />
        public IReadOnlyList<IAccessoryBase> Accessories { get; }

        /// <summary>
        /// internal list of accessories.
        /// </summary>
        private List<IAccessoryBase> _accessories;

        public Bridge()
        {
            this.IsBridged = false; // a bridge can not be bridged again.
            _accessories = new List<IAccessoryBase>();
            Accessories= new ReadOnlyCollection<IAccessoryBase>(_accessories);
        }
    }
}
