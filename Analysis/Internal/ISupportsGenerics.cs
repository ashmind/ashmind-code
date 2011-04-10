using System;
using System.Collections.Generic;
using System.Linq;

namespace AshMind.Code.Analysis.Internal {
    internal interface ISupportsGenerics {
        Type[] GetGenericArguments();
        bool IsGeneric { get; }
    }
}
