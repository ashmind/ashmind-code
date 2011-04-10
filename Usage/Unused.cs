using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AshMind.Code.Analysis;
using AshMind.Code.Usage.Assemblies;
using AshMind.Extensions;

namespace AshMind.Code.Usage {
    public static partial class Unused {
        private static PartitionedAssemblySet Prepare(IEnumerable<IAssemblyData> assemblies) {
            var inspector = new AssemblySetInspector();

            assemblies = inspector.Filter(assemblies).ToList();
            inspector.PreloadReferences(assemblies);

            return inspector.Partition(assemblies);
        }

        public static HashSet<IMemberData> Members(IEnumerable<IAssemblyData> assemblies) {
            var assemblySet = Prepare(assemblies);
            var result = new UnusedMemberFinder(configuration).FindIn(assemblySet);

            return result.UnusedMembers;
        }
        
        public static AssemblyStatistic[] Statistics(IEnumerable<IAssemblyData> assemblies) {
            var assemblySet = Prepare(assemblies);
            var result = new UnusedMemberFinder(configuration).FindIn(assemblySet);

            return (
                from assembly in assemblies
                let inAssembly = new Func<IMemberData, bool>(m => m.Assembly == assembly)
                let allMethods = result.AllMembers.Where(inAssembly)
                let unusedMethods = result.UnusedMembers.Where(inAssembly)
                let ignoredMethods = result.IgnoredMembers.Where(inAssembly)
                select new AssemblyStatistic {
                    AssemblyData       = assembly,
                    UnusedMembers      = unusedMethods.ToArray(),
                    UnusedMemberRatio  = (double)unusedMethods.Count() / allMethods.Count(),
                    IgnoredMemberRatio = (double)ignoredMethods.Count() / allMethods.Count(),
                    InspectedAsRoot    = assemblySet.Roots.Contains(assembly)
                }
            ).ToArray();
        }
    }
}
