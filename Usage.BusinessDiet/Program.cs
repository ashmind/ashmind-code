using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using ConsoleFx;
using ConsoleFx.Validators;

using AshMind.Code.Analysis;
using AshMind.Code.Usage.BusinessDiet.SummaryFormats;  
using AshMind.Extensions;

namespace AshMind.Code.Usage.BusinessDiet {
    using Switch = ConsoleFx.SwitchAttribute;

    [CommandLine]
    public partial class Program {
        private string outputDirectory;

        [ExecutionPoint(ProgramMode.Normal)]
        public int Run(string[] parameters) {
            Debug.Listeners.Add(new ConsoleTraceListener());

            if (!this.VerifyParametersSpecified(parameters)) 
                return 0;

            var assemblyPaths = this.GetFileNames(parameters).ToList();

            if (!this.VerifyAssembliesFound(parameters, assemblyPaths))
                return 0;

            var assemblies = this.LoadAssemblies(assemblyPaths);

            var watch = new Stopwatch();
            watch.Start();
            var statistics = Unused.Statistics(assemblies);
            watch.Stop();

            this.CreateOutputDirectoryIfRequired();

            var format = new PlainTextSummary(outputDirectory);
            format.Write(statistics, watch.Elapsed);

            return 0;
        }

        private IAssemblyData[] LoadAssemblies(IList<string> assemblyPaths) {
            var analyzer = new AnalysisDataResolver();

            return (
                from path in assemblyPaths
                let assembly = Assembly.LoadFrom(path)
                select analyzer.Resolve(assembly)
            ).ToArray();
        }

        private bool VerifyParametersSpecified(string[] parameters) {
            if (parameters.Length == 0) {
                ConsoleEx.WriteLine(ConsoleColor.Yellow, null, "No input assemblies specified.");
                this.DisplayUsage();
                return false;
            }

            return true;
        }

        private bool VerifyAssembliesFound(string[] parameters, ICollection<string> assemblyPaths) {
            if (assemblyPaths.Count == 0) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Found no input assemblies matching:");
                parameters.ForEach(p => Console.WriteLine("  {0}", p));
                Console.ResetColor();
                return false;
            }

            return true;
        }

        private void CreateOutputDirectoryIfRequired() {
            if (this.outputDirectory.IsNullOrEmpty())
                return;

            Directory.CreateDirectory(this.outputDirectory);
        }
        
        [Switch("output", ShortName = "o", MinParameters = 0, MaxParameters = 1)]
        [SwitchUsage(ProgramMode.Help, SwitchUsage.NotAllowed)]
        [SwitchUsage(ProgramMode.Normal, SwitchUsage.Optional)]
        [PathValidator(ParameterIndex.FirstParameter)]
        public void SetOutputDirectory(string[] parameters) {
            if (parameters.Length > 0)
                outputDirectory = parameters[0];
        }

        [ErrorHandler(typeof(Exception), DisplayUsage = false)]
        public void HandleError(Exception exception) {
            if (exception is CommandLineException) {
                this.DisplayUsage();
                return;
            }

            ConsoleEx.WriteLine(ConsoleColor.Red, null, "Exception: {0}", exception);
        }

        [Usage]
        public void DisplayUsage() {
            ConsoleEx.WriteLine("Usage:       BusinessDiet [/o[utput]:directory] <files>");
            ConsoleEx.WriteLine();
            ConsoleEx.WriteLine("Switches:");
            ConsoleEx.WriteLine("  /output    The directory to put reports in.");
            ConsoleEx.WriteLine("  /help      Display usage message.");
        }

        private IEnumerable<string> GetFileNames(string[] includes) {
            Func<string, string> getDirectory = include => {
                string directory = Path.GetDirectoryName(include);
                if (directory.Length == 0)
                    directory = Directory.GetCurrentDirectory();

                return directory;
            };

            return from include in includes
                   let directory = getDirectory(include)
                   let pattern = Path.GetFileName(include)
                   from file in Directory.GetFiles(directory, pattern)
                   select file;
        }
    }
}
