using System;
using System.CodeDom.Compiler;
using System.Linq;

using AshMind.Code.Analysis;

namespace AshMind.Code.Usage.Strategies.Specific {
    internal class GeneratedCodeSelectionStrategy : IndependentSelectionStrategyBase<IMemberData> {
        private static readonly Type GeneratedCodeAttributeType = typeof(GeneratedCodeAttribute);

        protected override bool ShouldSelect(IMemberData method, ISelectionContext context) {
            return IsCodeGenerated(method);
        }

        private bool IsCodeGenerated(IMemberData method) {
            var isGenerated = HasGeneratedCodeAttribute(method);
            if (isGenerated)
                return true;

            var type = method.DeclaringType;
            while (type != null) {
                isGenerated = HasGeneratedCodeAttribute(type);
                if (isGenerated)
                    return true;

                type = type.DeclaringType;
            }

            return false;
        }

        private bool HasGeneratedCodeAttribute(IMemberData member) {
            return Attribute.IsDefined(member.Inner, GeneratedCodeAttributeType);
        }
    }
}
