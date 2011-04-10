using System;
using System.Collections.Generic;
using System.Linq;

using AshMind.Code.Analysis;

namespace AshMind.Code.Usage.Strategies.Specific {
    internal abstract class IndependentSelectionStrategyBase<TMemberData> : MemberSelectionStrategy<TMemberData>
        where TMemberData : IMemberData 
    {
        public override IEnumerable<TMemberData> SelectFrom(IEnumerable<TMemberData> original, ISelectionContext context) {
            return original.Where(item => this.ShouldSelect(item, context));
        }

        protected abstract bool ShouldSelect(TMemberData value, ISelectionContext context);
    }
}
