using System;
using System.Collections.Generic;
using System.Linq;

using AshMind.Code.Analysis;

namespace AshMind.Code.Usage.Strategies {
    internal interface ISelectionContext {
        HashSet<TypeData> Types { get; }
    }
}
