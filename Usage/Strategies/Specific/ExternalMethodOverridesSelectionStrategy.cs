using System;
using System.Linq;

using AshMind.Code.Analysis;

namespace AshMind.Code.Usage.Strategies.Specific {
    internal class ExternalMethodOverridesSelectionStrategy : IndependentSelectionStrategyBase<MethodData> {
        protected override bool ShouldSelect(MethodData method, ISelectionContext context) {
            Func<TypeData, bool> notInContext = type => !context.Types.Contains(type);

            return method.GetOverridesAndImplements().Any(
                m => notInContext(m.DeclaringType)
            );
        }
    }
}
