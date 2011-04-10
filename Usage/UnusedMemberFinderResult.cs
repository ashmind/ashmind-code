using System;
using System.Collections.Generic;
using System.Linq;

using AshMind.Code.Analysis;

namespace AshMind.Code.Usage {
    internal class UnusedMemberFinderResult {
        public UnusedMemberFinderResult(HashSet<IMemberData> allMembers, HashSet<IMemberData> unusedMembers, HashSet<IMemberData> ignoredMembers) {
            this.AllMembers = allMembers;
            this.UnusedMembers = unusedMembers;
            this.IgnoredMembers = ignoredMembers;
        }

        public HashSet<IMemberData> AllMembers      { get; private set; }
        public HashSet<IMemberData> UnusedMembers   { get; private set; }
        public HashSet<IMemberData> IgnoredMembers  { get; private set; }
    }
}
