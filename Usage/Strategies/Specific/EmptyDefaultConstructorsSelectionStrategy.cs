using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using ClrTest.Reflection;

using AshMind.Code.Analysis;

namespace AshMind.Code.Usage.Strategies.Specific {
    internal class EmptyDefaultConstructorsSelectionStrategy : IndependentSelectionStrategyBase<MethodData> {
        protected override bool ShouldSelect(MethodData value, ISelectionContext context) {
            var constructor = value.Inner as ConstructorInfo;

            return constructor != null
                && constructor.GetParameters().Length == 0
                && IsEmptyOrJustCallsBase(value);
        }

        private bool IsEmptyOrJustCallsBase(MethodData constructor) {
            var instructions = constructor.IL.Where(i => i.OpCode != OpCodes.Nop)
                                             .Take(4).ToArray();

            if (instructions.Length == 1)
                return true; // only ret

            var expected = instructions.Length == 3
                        && instructions[0].OpCode == OpCodes.Ldarg_0
                        && instructions[1].OpCode == OpCodes.Call;

            if (!expected)
                return false;

            var called = (instructions[1] as InlineMethodInstruction).Method as ConstructorInfo;
            
            return called != null
                && called.GetParameters().Length == 0
                && called.DeclaringType == constructor.Inner.DeclaringType.BaseType;
        }
    }
}