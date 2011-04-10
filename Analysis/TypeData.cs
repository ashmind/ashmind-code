using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using AshMind.Code.Analysis.Collections;
using AshMind.Code.Analysis.Internal;

namespace AshMind.Code.Analysis {
    public class TypeData : MemberData<Type, IMemberData>, ISupportsGenerics {
        private const BindingFlags IncludeAll =
            BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;

        private ReadOnlyCollection<IMemberData> allDirectMembers;
        private ReadOnlyCollection<InterfaceImplementation> implementedInterfaces;
        private readonly GenericSupport generic;

        internal TypeData(Type type, AnalysisContext context) : base(type, context) {
            this.generic = new GenericSupport(this, this.Resolver);
        }

        protected override string BuildName() {
            var builder = new StringBuilder(this.Inner.Name);
            this.generic.AppendGenericNamePartTo(builder, true);

            return builder.ToString();
        }
        
        public TypeData[] GenericArguments {
            get { return this.generic.Arguments; }
        }

        protected override IEnumerable<IMemberData> GetMembers() {
            var members = this.GetAllMembers(false);
            var accessorOwners = members.OfType<IWithAccessors>();

            return members.Except(
                from owner in accessorOwners
                from accessor in owner.Accessors
                select (IMemberData)accessor
            );
        }

        public override IEnumerable<IMemberData> GetAllMembers() {
            return this.GetAllMembers(true);
        }

        public IEnumerable<IMemberData> GetAllMembers(bool fromNestedTypes) {
            if (allDirectMembers == null) {
                allDirectMembers = new LazyReadOnlyCollection<IMemberData>(
                    from member in this.Inner.GetMembers(IncludeAll)
                    orderby (member is PropertyInfo || member is EventInfo) ? 1 : 2
                    let result = this.Context.Resolver.Resolve(member)
                    where result != null
                    orderby result.Name
                    select result
                );
            }

            if (!fromNestedTypes)
                return allDirectMembers;

            return allDirectMembers.Union(
                from type in allDirectMembers.OfType<TypeData>()
                from member in type.GetAllMembers(true)
                select member
            );
        }

        public ReadOnlyCollection<IMemberData> Members {
            get { return this.MembersCore; }
        }

        public ReadOnlyCollection<InterfaceImplementation> ImplementedInterfaces {
            get {
                if (this.implementedInterfaces == null) {
                    this.implementedInterfaces = new LazyReadOnlyCollection<InterfaceImplementation>(
                        this.Inner.GetInterfaces()
                            .Select(@interface => this.ResolveImplementation(@interface))
                    );
                }

                return this.implementedInterfaces;
            }
        }

        private InterfaceImplementation ResolveImplementation(Type @interface) {
            var interfaceData = this.Resolver.Resolve(@interface);
            return new InterfaceImplementation(this, interfaceData, this.Context);
        }
        
        public IEnumerable<MethodData> GetMethods() {
            return this.Members.OfType<MethodData>().Except(this.GetConstructors());
        }

        public IEnumerable<PropertyData> GetProperties() {
            return this.Members.OfType<PropertyData>();
        }

        public IEnumerable<EventData> GetEvents() {
            return this.Members.OfType<EventData>();
        }

        public IEnumerable<MethodData> GetConstructors() {
            return this.Members.OfType<MethodData>().Where(m => m.Inner is ConstructorInfo);
        }

        #region ISupportsGenerics Members

        Type[] ISupportsGenerics.GetGenericArguments() {
            return this.Inner.GetGenericArguments();
        }

        bool ISupportsGenerics.IsGeneric {
            get { return this.Inner.IsGenericType; }
        }

        #endregion
    }
}
