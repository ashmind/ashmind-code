using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AshMind.Code.Analysis.Internal {
    internal class GenericSupport {
        private readonly ISupportsGenerics host;
        private readonly AnalysisDataResolver resolver;

        private TypeData[] arguments;

        public GenericSupport(ISupportsGenerics host, AnalysisDataResolver resolver) {
            this.host = host;
            this.resolver = resolver;
        }

        public TypeData[] Arguments {
            get {
                if (this.arguments == null)
                    this.arguments = this.GetArguments();

                return this.arguments;
            }
        }

        private TypeData[] GetArguments() {
            if (!this.host.IsGeneric)
                return new TypeData[0];

            return this.host.GetGenericArguments()
                            .Select(argument => this.resolver.Resolve(argument))
                            .ToArray();
        }

        public void AppendGenericNamePartTo(StringBuilder nameBuilder, bool hasGenericSpecifier) {
            const int BacktickLength = 1;

            var arguments = this.Arguments;
            if (arguments.Length == 0)
                return;

            if (hasGenericSpecifier) {
                var genericSpecifierLength = ((arguments.Length/10) + 1) + BacktickLength; // `1, `2, etc
                nameBuilder.Remove(nameBuilder.Length - genericSpecifierLength, genericSpecifierLength);
            }

            nameBuilder.Append("<").Append(arguments[0].Name);
            foreach (var argument in arguments.Skip(1)) {
                nameBuilder.Append(", ").Append(argument.Name);
            }
            nameBuilder.Append(">");
        }
    }
}
