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
    public class ConsoleLogModel
    {
        /// <summary>
        /// Is enabled?
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Log level.
        /// </summary>
        public string Level { get; set; }
    }
}
