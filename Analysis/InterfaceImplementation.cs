using System;
using System.Collections.Generic;
using System.Linq;

namespace AshMind.Code.Analysis {
    public class InterfaceImplementation {
        public TypeData Interface { get; private set; }
        public IDictionary<IMemberData, IMemberData> Members { get; private set; }

        public InterfaceImplementation(TypeData type, TypeData @interface, AnalysisContext context) {
            this.Interface = @interface;
            this.LoadMemberMap(type, context);
        }

        private void LoadMemberMap(TypeData type, AnalysisContext context) {
            this.Members = new Dictionary<IMemberData, IMemberData>();

            var map = type.Inner.GetInterfaceMap(this.Interface.Inner);
            for (int i = 0; i < map.InterfaceMethods.Length; i++) {
                var source = context.Resolver.Resolve(map.InterfaceMethods[i]);
                var target = context.Resolver.Resolve(map.TargetMethods[i]);

                this.Members.Add(source, target);
            }
        }
    }
}
