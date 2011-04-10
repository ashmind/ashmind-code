using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using HLib;

using AshMind.Extensions;

namespace AshMind.Code.Usage.BusinessDiet.SummaryFormats {
    public class PlainTextSummary {
        public string OutputDirectory { get; private set; }

        public PlainTextSummary(string outputDirectory) {
            this.OutputDirectory = outputDirectory;
        }

        public void Write(AssemblyStatistic[] statistics, TimeSpan time) {
            var outputDirectory = !this.OutputDirectory.IsNullOrEmpty()
                ? this.OutputDirectory
                : statistics[0].AssemblyData.File.DirectoryName;

            foreach (var statistic in statistics) {
                this.WriteStatistic(outputDirectory, statistic);
            }

            string summaryFile = Path.Combine(outputDirectory, "diet.summary");

            using (var writer = new StreamWriter(summaryFile)) {
                writer.WriteLine("Total assemblies: {0}.", statistics.Length);
                writer.WriteLine();

                var table = new ConsoleTable();
                table.SetHeaders(new[] { "Assembly", "Unused", "Ignored*" });

                this.AddPartitionSummary(table, "Roots", statistics.Where(s => s.InspectedAsRoot));
                table.AppendRow(new[] { string.Empty, string.Empty, string.Empty });
                this.AddPartitionSummary(table, "Other", statistics.Where(s => !s.InspectedAsRoot));

                writer.Write(table.ToString());
                writer.WriteLine("  * ignored code was probably code-generated");

                writer.WriteLine();
                writer.WriteLine("Analysed in {0}.", time);
            }
        }

        private void WriteStatistic(string outputDirectory, AssemblyStatistic statistic) {
            var methodsByType = from method in statistic.UnusedMembers
                                orderby method.Name
                                group method by method.DeclaringType.Name into type
                                orderby type.Key
                                select type;
            
            string report = Path.Combine(
                outputDirectory,
                statistic.AssemblyData.File.Name + ".unused"
            );

            using (var writer = new StreamWriter(report)) {
                foreach (var type in methodsByType) {
                    writer.WriteLine("{0}", type.Key);

                    foreach (var method in type) {
                        writer.Write("    ");
                        writer.WriteLine(method.Name);
                    }

                    writer.WriteLine();
                }
            }
        }

        private void AddPartitionSummary(ConsoleTable table, string title, IEnumerable<AssemblyStatistic> statistics) {
            table.AppendRow(new[] { title, string.Empty, string.Empty });

            var rows = from statistic in statistics
                       let name = statistic.AssemblyData.Name
                       orderby name
                       select new[] {
                           "  " + name, FormatRatio(statistic.UnusedMemberRatio), FormatRatio(statistic.IgnoredMemberRatio)
                       };

            rows.ForEach(table.AppendRow);
        }

        private string FormatRatio(double ratio) {
            return ratio.ToString("P1").PadLeft(6);
        }
    }
}
