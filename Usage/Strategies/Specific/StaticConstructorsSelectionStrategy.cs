using System;
using System.Linq;
using System.Reflection;

using AshMind.Code.Analysis;

namespace AshMind.Code.Usage.Strategies.Specific {
    internal class StaticConstructorsSelectionStrategy : IndependentSelectionStrategyBase<MethodData> {
        protected override bool ShouldSelect(MethodData method, ISelectionContext context) {
            return method.Inner.IsSpecialName && method.Inner.Name == ConstructorInfo.TypeConstructorName;
        }
    }
}
