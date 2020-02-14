using System.Globalization;

namespace Homer.Core.Internals
{
    /// <summary>
    /// Culture info.
    /// </summary>
    public static class Culture
    {
        /// <summary>
        /// Default culture
        /// </summary>
        public static CultureInfo Default { get; }

        static Culture()
        {
            Default = new CultureInfo("en-US");
        }
    }
}
