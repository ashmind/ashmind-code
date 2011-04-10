using System;
using System.Linq;

using AshMind.Code.Analysis;
using AshMind.Code.Usage.Strategies;

namespace AshMind.Code.Usage {
    using IMemberSelectionStrategy = ISelectionStrategy<IMemberData>;

    internal class UnusedMemberFinderConfiguration {
        public IMemberSelectionStrategy Entry           { get; set; }
        public IMemberSelectionStrategy DefinitelyUsed  { get; set; }
        public IMemberSelectionStrategy Ignored         { get; set; }
    }
}
