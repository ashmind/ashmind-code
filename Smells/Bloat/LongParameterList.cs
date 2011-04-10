using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using AshMind.Code.Analysis;

namespace AshMind.Code.Smells.Bloat {
    public class LongParameterList : LocalSmell<MethodData> {
        public LongParameterList() {
            this.TolerableCount = 4;
        }

        public override bool SpreadsFrom(MethodData method) {
            return this.GetParameterCount(method) > this.TolerableCount;
        }

        public override object Explain(MethodData method) {
            return string.Format(
                "{0} parameters, while only {1} allowed.",
                this.GetParameterCount(method), this.TolerableCount
            );
        }

        private int GetParameterCount(MethodData method) {
            return method.Inner.GetParameters().Length;
        }

        [DefaultValue(4)]
        public int TolerableCount { get; set; }
    }
}
