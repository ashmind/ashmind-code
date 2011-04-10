using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AshMind.Code.Analysis.Collections;

namespace AshMind.Code.Analysis {
    public class PropertyData : MemberData<PropertyInfo, MethodData>, IWithAccessors {
        public PropertyData(PropertyInfo property, AnalysisContext context) : base(property, context) {
            this.ForceGetMembers();
        }

        protected override IEnumerable<MethodData> GetMembers() {
            var methods = new List<MethodData>();

            var getter = this.Inner.GetGetMethod(true);
            if (getter != null) {
                this.Getter = this.Resolver.Resolve(getter, this);
                methods.Add(this.Getter);
            }

            var setter = this.Inner.GetSetMethod(true);
            if (setter != null) {
                this.Setter = this.Resolver.Resolve(setter, this);
                methods.Add(this.Setter);
            }

            return methods;
        }

        public MethodData Getter { get; private set; }
        public MethodData Setter { get; private set; }

        public ReadOnlyCollection<MethodData> Accessors {
            get { return this.MembersCore; }
        }
    }
}
