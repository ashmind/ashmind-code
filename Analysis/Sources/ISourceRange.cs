using System;
using System.Collections.Generic;
using System.Linq;

namespace AshMind.Code.Analysis.Sources {
    public interface ISourceRange {
        SourceDocument Document { get; }
        SourcePosition Start { get; }
        SourcePosition End { get; }
    }
}
