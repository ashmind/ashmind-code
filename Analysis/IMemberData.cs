using System.Collections.Generic;
using System.Reflection;

using AshMind.Code.Analysis.Sources;

namespace AshMind.Code.Analysis {
    public interface IMemberData : IAnalysisData {
        MemberInfo Inner { get; }
        IAssemblyData Assembly { get; }
        TypeData DeclaringType { get; }

        SourceDocument GetSourceDocument();
    }
}