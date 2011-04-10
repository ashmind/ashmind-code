using System;
using System.Linq;

using AshMind.Code.Analysis;

namespace AshMind.Code.Usage.Strategies.Specific {
    internal class ExternallyVisibleMethodsSelectionStrategy : IndependentSelectionStrategyBase<MethodData> {
        protected override bool ShouldSelect(MethodData method, ISelectionContext context) {
            return method.Inner.IsPublic || method.Inner.IsFamily || method.Inner.IsFamilyOrAssembly;
        }
    }
}
