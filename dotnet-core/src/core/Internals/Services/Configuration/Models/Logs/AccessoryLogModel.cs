#region license
// 
//      hypeengine
// 
//      Copyright (c) 2016 - 2019, Int6ware
// 
//      This file is part of hypeengine project. Unauthorized copying of this file, via any medium is strictly prohibited.
//      The hypeengine or its components/sources can not be copied and/or distributed without the express permission of Int6ware.
#endregion

namespace Homer.Core.Internals.Services.Configuration.Models.Logs
{
    public class AccessoryLogModel
    {
        /// <summary>
        /// Console log.
        /// </summary>
        public ConsoleLogModel Console { get; set; }

        /// <summary>
        /// File log.
        /// </summary>
        public FileLogModel File { get; set; }
    }
}
