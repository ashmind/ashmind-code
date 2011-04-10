using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

using AshMind.Code.Analysis.Collections;

namespace AshMind.Code.Analysis {
    [DebuggerDisplay("Name = {Name}")]
    public abstract class AnalysisData<TMemberData> : IAnalysisData
        where TMemberData : IMemberData
    {
        private readonly AnalysisContext context;
        private ReadOnlyCollection<TMemberData> membersCore;
        private ReadOnlyCollection<IMemberData> membersCoreUntyped;

        private string name;

        protected AnalysisData(AnalysisContext context) {
            this.context = context;
        }
        
        public string Name {
            get {
                if (this.name == null)
                    this.name = this.BuildName();
                
                return this.name;
            }
        }

        protected abstract string BuildName();

        public AnalysisContext Context {
            get { return context; }
        }

        protected void ForceGetMembers() {
            this.MembersCore.Count();
        }

        protected virtual IEnumerable<TMemberData> GetMembers() {
            return Enumerable.Empty<TMemberData>();
        }
        
        protected ReadOnlyCollection<TMemberData> MembersCore {
            get {
                if (this.membersCore == null)
                    this.membersCore = new LazyReadOnlyCollection<TMemberData>(this.GetMembers());
                
                return this.membersCore;
            }
        }

        ReadOnlyCollection<IMemberData> IAnalysisData.Members {
            get {
                if (this.membersCoreUntyped == null)
                    this.membersCoreUntyped = new LazyReadOnlyCollection<IMemberData>(this.MembersCore.Cast<IMemberData>());

                return this.membersCoreUntyped;
            }
        }

        public virtual IEnumerable<IMemberData> GetAllMembers() {
            foreach (var member in this.MembersCore) {
                yield return member;
                foreach (var submember in member.GetAllMembers()) {
                    yield return submember;
                }
            }
        }

        protected AnalysisDataResolver Resolver {
            get { return this.context.Resolver; }
        }

        #region INotifyPropertyChanged Members

        private PropertyChangedEventHandler propertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

        #endregion
    }
}
