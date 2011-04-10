using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AshMind.Code.Usage.Strategies {
    internal class UnifiedSelectionStrategy<T> : ISelectionStrategy<T> {
        private readonly IEnumerable<ISelectionStrategy<T>> m_strategies;

        public UnifiedSelectionStrategy(params ISelectionStrategy<T>[] strategies) {
            m_strategies = strategies;
        }

        public IEnumerable<T> SelectFrom(IEnumerable<T> original, ISelectionContext context) {
            return m_strategies.SelectMany(strategy => strategy.SelectFrom(original, context));
        }
    }
}
