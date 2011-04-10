using System;
using System.Linq;
using System.Reflection;

using AshMind.Code.Analysis.Sources;

namespace AshMind.Code.Analysis {
    public abstract class MemberData<TMemberInfo, TMembersMemberData> : AnalysisData<TMembersMemberData>, IMemberData
        where TMemberInfo : MemberInfo 
        where TMembersMemberData : IMemberData
    {
        private readonly TMemberInfo inner;
        private readonly IAssemblyData assembly;
        private readonly TypeData declaringType;

        protected MemberData(TMemberInfo member, AnalysisContext context) : base(context) {
            Argument.VerifyNotNull("member", member);

            this.inner = member;
            
            if (member.DeclaringType != null)
                this.declaringType = context.Resolver.Resolve(member.DeclaringType);

            this.assembly = context.Resolver.Resolve(member.Module.Assembly);
        }

        protected override string BuildName() {
            return this.Inner.Name;
        }
        
        public TMemberInfo Inner {
            get { return inner; }
        }

        MemberInfo IMemberData.Inner {
            get { return this.Inner; }
        }
        
        public IAssemblyData Assembly {
            get { return this.assembly; }
        }

        public TypeData DeclaringType {
            get { return this.declaringType; }
        }

        public virtual SourceDocument GetSourceDocument() {
            return this.GetAllMembers()
                            .Select(m => m.GetSourceDocument())
                            .FirstOrDefault(source => source != null);
        }

        public override bool Equals(object obj) {
            var member = obj as MemberData<TMemberInfo, TMembersMemberData>;
            if (member == null)
                return false;

            return this.Inner.Equals(member.Inner);
        }

        public override int GetHashCode() {
            return this.Inner.GetHashCode();
        }

        public override string ToString() {
            return this.Name;
        }
    }
}
