using System;
using System.IO;

namespace Homer.Core.Utils.IO
{
    /// <summary>
    /// File helpers.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// The current assemblies root.
        /// </summary>
        public static string AssemblyRoot => AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// Loads a file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string LoadFile(string file)
        {
            // Get the absolute path to the JSON file
            var path = Path.IsPathRooted(file)
                ? file
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), file);

            if (!File.Exists(path)) throw new ArgumentException($"Could not find file at path: {path}");

            var data = File.ReadAllText(path); // Load the file
            return data;
        }
    }
}
