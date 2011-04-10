using System;
using System.ComponentModel;
using System.Linq;

using AshMind.Code.Analysis;

namespace AshMind.Code.Smells.Bloat {
    public class LongMethod : LocalSmell<MethodData> {
        public LongMethod() {
            this.TolerableInstructionCount = 250;
            this.TolerableLineCount = 20;
        }

        public override bool SpreadsFrom(MethodData member) {
            return member.Lines.Length > this.TolerableLineCount
                || member.IL.Length > this.TolerableInstructionCount;
        }
        
        public override object Explain(MethodData method) {
            return string.Format(
                "{0} IL instructions, while only {1} allowed.",
                method.IL.Length, this.TolerableInstructionCount
            );
        }

        [DefaultValue(250)]
        public int TolerableInstructionCount { get; set; }

        [DefaultValue(20)]
        public int TolerableLineCount        { get; set; }
    }
}
