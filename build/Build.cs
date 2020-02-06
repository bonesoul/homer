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

using System.IO;
using System.Runtime.CompilerServices;
using McMaster.Extensions.CommandLineUtils;
using static Bullseye.Targets;
using static SimpleExec.Command;

namespace Homer.Build
{
    [Command(ThrowOnUnexpectedArgument = false)]
    [SuppressDefaultHelpOption]
    public class Build
    {
        static void Main(string[] args) => CommandLineApplication.Execute<Build>(args);

        [Option("-h|-?|--help", "Show help message", CommandOptionType.NoValue)]
        public bool ShowHelp { get; } = false;

        [Option("-c|--configuration", "The configuration to build", CommandOptionType.SingleValue)]
        public string Configuration { get; } = "Release";

        [Option("-f|--framework", "The framework to target", CommandOptionType.SingleValue)]
        public string Framework { get; } = "netcoreapp3.1";

        public string[] RemainingArguments { get; } = null;

        private static string GetSolutionDirectory() => Path.GetFullPath(Path.Combine(GetScriptDirectory(), @".."));

        private static string GetScriptDirectory([CallerFilePath] string filename = null) => Path.GetDirectoryName(filename);

        public void OnExecute(CommandLineApplication app)
        {
            if (ShowHelp)
            {
                app.ShowHelp();
                app.Out.WriteLine("Bullseye help:");
                app.Out.WriteLine();
                RunTargetsAndExit(new[] { "-h" });
                return;
            }

            Directory.SetCurrentDirectory(GetSolutionDirectory());

            var artifactsDir = Path.GetFullPath("bin");
            var logsDir = Path.Combine(artifactsDir, "logs");
            var buildLogFile = Path.Combine(logsDir, "build.binlog");

            var projects = new []
            {
                "contrib/uuid.net/uuid.net.csproj",
                "src/platforms/homekit/Homer.Platform.HomeKit.csproj",
                "src/core/Homer.Core.csproj",
                "src/server/Homer.Server.csproj"
            };

            var testProjects = new string[]
            {
            };

            Target(
                "artifactDirs",
                () =>
                {
                    Directory.CreateDirectory(artifactsDir);
                    Directory.CreateDirectory(logsDir);
                });

            Target(
                "clean",
                DependsOn("artifactDirs"),
                forEach: projects,
                action: project => Run("dotnet", $"clean {project} -c {Configuration} -f {Framework} -o \"{artifactsDir}\" /p:Platform=x64 /bl:\"{buildLogFile}\""));

            Target(
                "build",
                DependsOn("clean"),
                forEach: projects,
                action: project => Run("dotnet", $"build {project} -c {Configuration} -f {Framework} -o \"{artifactsDir}\" /p:Platform=x64 /bl:\"{buildLogFile}\""));

            Target(
                "test",
                () => Run("dotnet", "test"));


            Target("default", DependsOn("build"));

            RunTargetsAndExit(RemainingArguments);
        }
    }
}
