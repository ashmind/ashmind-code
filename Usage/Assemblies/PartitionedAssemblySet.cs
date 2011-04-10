using System;
using System.Collections.Generic;
using System.Linq;

using AshMind.Code.Analysis;
using AshMind.Extensions;

namespace AshMind.Code.Usage.Assemblies {
    internal class PartitionedAssemblySet {
        public PartitionedAssemblySet(IEnumerable<IAssemblyData> roots, IEnumerable<IAssemblyData> other) {
            this.Roots = roots.ToSet();
            this.Other = other.ToSet();
        }

        public HashSet<IAssemblyData> Roots { get; private set; }
        public HashSet<IAssemblyData> Other { get; private set; }
    }
}
