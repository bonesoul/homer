#region license
// 
//      hypeengine
// 
//      Copyright (c) 2016 - 2019, Int6ware
// 
//      This file is part of hypeengine project. Unauthorized copying of this file, via any medium is strictly prohibited.
//      The hypeengine or its components/sources can not be copied and/or distributed without the express permission of Int6ware.
#endregion

using System.IO;
using System.Runtime.CompilerServices;
using McMaster.Extensions.CommandLineUtils;
using static Bullseye.Targets;
using static SimpleExec.Command;

namespace HypeEngine.Build
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
                "src/platforms/homekit/Smarty.Platform.HomeKit.csproj",
                "src/core/Smarty.Core.csproj"
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
