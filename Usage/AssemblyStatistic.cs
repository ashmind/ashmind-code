using System;
using System.Collections.Generic;
using System.Linq;

using AshMind.Code.Analysis;

namespace AshMind.Code.Usage {
    public class AssemblyStatistic {
        public IAssemblyData AssemblyData           { get; internal set; }
        public IMemberData[] UnusedMembers          { get; internal set; }
        public bool InspectedAsRoot                 { get; internal set; }
        public double UnusedMemberRatio             { get; internal set; }
        public double IgnoredMemberRatio            { get; internal set; }
    }
}
