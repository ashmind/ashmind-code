using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using ClrTest.Reflection;

using AshMind.Code.Analysis.Internal;
using AshMind.Code.Analysis.Internal.MethodSupport;
using AshMind.Code.Analysis.Sources;
using AshMind.Constructs;

namespace AshMind.Code.Analysis {
    public class MethodData : MemberData<MethodBase, IMemberData>, ISupportsGenerics {
        private readonly GenericSupport generic;
        private readonly MethodIL il;
        private readonly MethodSource source;

        // temporary performance hack
        private static readonly string[] EmptyLines = new string[0];

        private MethodData @base;

        internal MethodData(MethodBase method, AnalysisContext context, IWithAccessors declaringMember) : base(method, context) {
            this.generic = new GenericSupport(this, this.Resolver);

            this.il = new MethodIL(method);
            this.source = new MethodSource(method, context.SourceProvider);

            this.DeclaringMember = declaringMember;
        }

        private void EnsureBase() {
            if (this.@base != null || !(this.Inner is MethodInfo))
                return;

            @base = this.Resolver.Resolve(
                (this.Inner as MethodInfo).GetBaseDefinition()
            );
        }

        public MethodData Base {
            get {
                this.EnsureBase();
                return this.@base;
            }
        }

        public ILInstruction[] IL {
            get { return this.il.Instructions; }
        }

        public string[] Lines {
            get { return EmptyLines; }
        }

        public override SourceDocument GetSourceDocument() {
            return this.source.Document;
        }

        public IWithAccessors DeclaringMember { get; private set; }

        public IEnumerable<MethodData> GetOverridesAndImplements() {
            return Inheritance.GetBaseAndImplementedMethods(this);
        }

        public MethodData GetGenericDefinition() {
            var method = this.Inner;
            if (this.Inner.IsGenericMethod)
                method = (method as MethodInfo).GetGenericMethodDefinition();

            var type = method.DeclaringType;
            if (type == null) // C++ artefacts (assembly-level methods)
                return this.Resolver.Resolve(method);

            if (type.IsGenericType && !type.IsGenericTypeDefinition) {
                var genericDefinition = this.Resolver.Resolve(type.GetGenericTypeDefinition());
                var methodsOfGeneric = this.GetMembersLikeThis(genericDefinition);
                return methodsOfGeneric.First(m => m.Inner.MetadataToken == method.MetadataToken);
            }

            return this.Resolver.Resolve(method);
        }

        private IEnumerable<MethodData> GetMembersLikeThis(TypeData type) {
            if (this.Inner is ConstructorInfo)
                return type.GetConstructors();

            var methods = type.GetMethods();
            if (!this.Inner.IsSpecialName)
                return methods;

            return methods.Union(
                from property in type.GetProperties()
                from accessor in property.Accessors
                select accessor
            );
        }

        public IEnumerable<MethodData> GetReferencedMethods() {
            var references = from instruction in this.IL
                             let reference = GetReferenceFromInstruction(instruction)
                             where reference != null
                             select this.Resolver.Resolve(reference);

            return references;
        }

        private static readonly Func<ILInstruction, MethodBase> GetReferenceFromInstruction =
            Switch.Type<ILInstruction>().To<MethodBase>()
                  .Case<InlineMethodInstruction>(call => call.Method)
                  .Case<InlineTokInstruction>(methodof => methodof.Member as MethodBase)
                  .Compile();

        protected override string BuildName() {
            var builder = new StringBuilder(this.Inner.Name);
            generic.AppendGenericNamePartTo(builder, false);

            builder.Append("(");
            this.AppendParametersTo(builder);
            builder.Append(")");

            this.AppendReturnTypeTo(builder);

            return builder.ToString();
        }

        private void AppendReturnTypeTo(StringBuilder builder) {
            var method = this.Inner as MethodInfo;
            if (method == null)
                return;

            var returnType = method.ReturnType;
            if (returnType == typeof(void))
                return;

            var data = this.Resolver.Resolve(returnType);
            builder.Append(" : ").Append(data.Name);
        }

        private void AppendParametersTo(StringBuilder builder) {
            var parameters = this.Inner.GetParameters();
            if (parameters.Length == 0)
                return;

            this.AppendParameter(builder, parameters[0]);
            foreach (var parameter in parameters.Skip(1)) {
                builder.Append(", ");
                this.AppendParameter(builder, parameter);
            }    
        }

        private void AppendParameter(StringBuilder builder, ParameterInfo parameter) {
            var typeData = this.Resolver.Resolve(parameter.ParameterType);
            builder.Append(typeData.Name).Append(" ").Append(parameter.Name);
        }

        #region ISupportsGenerics Members

        Type[] ISupportsGenerics.GetGenericArguments() {
            return this.Inner.GetGenericArguments();
        }

        bool ISupportsGenerics.IsGeneric {
            get { return this.Inner.IsGenericMethod; }
        }

        #endregion
    }
}
