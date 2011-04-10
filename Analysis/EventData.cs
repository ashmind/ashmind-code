using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AshMind.Code.Analysis.Collections;

namespace AshMind.Code.Analysis {
    public class EventData : MemberData<EventInfo, MethodData>, IWithAccessors {
        internal EventData(AnalysisContext context, EventInfo member) : base(member, context) {
            this.ForceGetMembers();
        }

        protected override IEnumerable<MethodData> GetMembers() {
            var inner = this.Inner;
            return (
                from method in new[] { inner.GetAddMethod(true), inner.GetRemoveMethod(true), inner.GetRaiseMethod(true) }
                where method != null
                select this.Context.Resolver.Resolve(method, this)
            );
        }

        public ReadOnlyCollection<MethodData> Accessors {
            get { return this.MembersCore; }
        }
    }
}
