using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AshMind.Code.Analysis.Internal {
    internal static class Inheritance {
        public static IEnumerable<MethodData> GetBaseAndImplementedMethods(MethodData method) {
            if (!(method.Inner is MethodInfo)) // constructor
                return Enumerable.Empty<MethodData>();

            return Inheritance.GetBaseAndImplementedMethodsForMethod(method);
        }

        private static IEnumerable<MethodData> GetBaseAndImplementedMethodsForMethod(MethodData method) {
            var bases = Inheritance.GetBaseMethods(method);
            var implements = Inheritance.GetImplementedMethods(method);
            
            return Enumerable.Concat(bases, implements);
        }

        public static IEnumerable<MethodData> GetBaseMethods(MethodData method) {
            var lastBaseMethod = method;
            var baseMethod = method.Base;

            while (lastBaseMethod != baseMethod) {
                yield return baseMethod;

                lastBaseMethod = baseMethod;
                baseMethod = baseMethod.Base;
            }
        }

        public static IEnumerable<MethodData> GetImplementedMethods(MethodData method) {
            var type = method.DeclaringType;
            if (type.Inner.IsInterface)
                return Enumerable.Empty<MethodData>();

            return from implementation in type.ImplementedInterfaces
                   from map in implementation.Members.AsEnumerable()
                   where map.Value == method
                   select (MethodData)map.Key;
        }
    }
}