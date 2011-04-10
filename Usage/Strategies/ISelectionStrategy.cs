using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AshMind.Code.Usage.Strategies {
    internal interface ISelectionStrategy<T> {
        IEnumerable<T> SelectFrom(IEnumerable<T> original, ISelectionContext context);
    }
}
