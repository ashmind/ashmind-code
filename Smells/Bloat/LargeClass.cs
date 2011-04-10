using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using AshMind.Code.Analysis;

namespace AshMind.Code.Smells.Bloat {
    public class LargeClass : LocalSmell<TypeData> {
        public LargeClass() {
            this.TolerableMemberCount = 30; 
        }
        
        public override bool SpreadsFrom(TypeData type) {
            return this.GetMemberCount(type) > this.TolerableMemberCount;
        }

        public override object Explain(TypeData type) {
            return string.Format(
                "{0} members, while only {1} allowed.",
                this.GetMemberCount(type), this.TolerableMemberCount
            );
        }

        private int GetMemberCount(TypeData type) {
            return type.GetAllMembers(false).Count();
        }

        [DefaultValue(30)]
        public int TolerableMemberCount { get; set; }
    }
}
