using System;
using System.Collections.Generic;
using System.Linq;

using AshMind.Code.Analysis;

namespace AshMind.Code.Usage.Strategies {
    // If I try to implement both ISelectionStrategy<MemberData> and ISelectionStrategy<TMemberData> on
    // generic MemberSelectionStrategyBase, it will cause error CS0695 (types may unify for some type parameter 
    // substitutions)

    internal abstract class MemberSelectionStrategy : ISelectionStrategy<IMemberData> {
        protected abstract IEnumerable<IMemberData> SelectFrom(IEnumerable<IMemberData> original, ISelectionContext context); 

        IEnumerable<IMemberData> ISelectionStrategy<IMemberData>.SelectFrom(IEnumerable<IMemberData> original, ISelectionContext context) {
            return this.SelectFrom(original, context);
        }        
    }

    internal abstract class MemberSelectionStrategy<TMemberData> : MemberSelectionStrategy, ISelectionStrategy<TMemberData>
        where TMemberData : IMemberData
    {
        public abstract IEnumerable<TMemberData> SelectFrom(IEnumerable<TMemberData> original, ISelectionContext context);

        protected override IEnumerable<IMemberData> SelectFrom(IEnumerable<IMemberData> original, ISelectionContext context) {
            return this.SelectFrom(original.OfType<TMemberData>(), context).Cast<IMemberData>();
        }
    }
}
