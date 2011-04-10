using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AshMind.Code.Analysis.Collections;

namespace AshMind.Code.Analysis {
    public interface IAssemblyData : IAnalysisData {
        ReadOnlyCollection<TypeData> Types { get; }
        IAssemblyData[] References { get; }

        FileInfo File { get; }
    }
}
