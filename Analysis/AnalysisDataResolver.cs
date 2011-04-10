using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AshMind.Code.Analysis;
using AshMind.Code.Analysis.Internal;
using AshMind.Constructs;

namespace AshMind.Code.Analysis {
    public class AnalysisDataResolver {
        public AnalysisContext Context { get; private set; }

        private readonly Cache<object, IAnalysisData> dataCache = new Cache<object, IAnalysisData>();

        public AnalysisDataResolver() {
            this.Context = new AnalysisContext(this);
        }

        public AnalysisDataResolver(AnalysisContext context) {
            this.Context = context;
        }

        public IMemberData Resolve(MemberInfo member) {
            return Switch.Type(member).To<IMemberData>()
                         .Case<Type>(this.Resolve)
                         .Case<MethodBase>(this.Resolve)
                         .Case<PropertyInfo>(this.Resolve)
                         .Case<EventInfo>(this.Resolve)
                         .Result;
        }

        public IAssemblyData Resolve(Assembly assembly) {
            return this.dataCache.Get(assembly, () => new AssemblyData(assembly, this.Context));
        }

        public TypeData Resolve<T>() {
            return this.Resolve(typeof(T));
        }

        public TypeData Resolve(Type type) {
            return this.dataCache.Get(type, () => new TypeData(type, this.Context));
        }

        public MethodData Resolve(MethodBase method) {
            return this.Resolve(method, null);
        }

        public MethodData Resolve(MethodBase method, IWithAccessors declaringMember) {
            return this.dataCache.Get(method, () => new MethodData(method, this.Context, declaringMember));
        }

        public PropertyData Resolve(PropertyInfo property) {
            return this.dataCache.Get(property, () => new PropertyData(property, this.Context));
        }

        public EventData Resolve(EventInfo @event) {
            return this.dataCache.Get(@event, () => new EventData(this.Context, @event));
        }
    }
}
