using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Code.Analysis.Collections;

namespace AshMind.Code.Analysis {
    public interface IWithAccessors : IMemberData {
        ReadOnlyCollection<MethodData> Accessors { get; }
    }
}
