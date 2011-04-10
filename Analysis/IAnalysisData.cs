using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using AshMind.Code.Analysis.Collections;

namespace AshMind.Code.Analysis {
    public interface IAnalysisData : INotifyPropertyChanged {
        string Name { get; }
        ReadOnlyCollection<IMemberData> Members { get; }
        IEnumerable<IMemberData> GetAllMembers();
    }
}
