using System;
using System.Collections.Generic;
using System.Linq;

using AshMind.Code.Analysis;

namespace AshMind.Code.Smells {
    public interface ISmell {
        HashSet<IMemberData> FindSources(IEnumerable<IAssemblyData> assemblies);
        object Explain(IMemberData data);
    }
}
