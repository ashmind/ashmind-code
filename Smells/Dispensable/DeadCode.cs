using System;
using System.Collections.Generic;
using System.Linq;

using AshMind.Code.Analysis;
using AshMind.Code.Usage;

namespace AshMind.Code.Smells.Dispensable {
    public class DeadCode : ISmell {
        public HashSet<IMemberData> FindSources(IEnumerable<IAssemblyData> assemblies) {
            return Unused.Members(assemblies);
        }

        public object Explain(IMemberData data) {
            return "It is not possible to explain this smell in current version.";
        }
    }
}
