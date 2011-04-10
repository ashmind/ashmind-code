using System;
using System.Linq;
using System.Reflection;

using AshMind.Code.Analysis;

namespace AshMind.Code.Usage.Strategies.Specific.Web {
    internal class AspNetFactoryMethodsSelectionStrategy : IndependentSelectionStrategyBase<MethodData> {
        private const string FactoryNamespace = "__ASP";
        private const string FactoryTypePrefix = "FastObjectFactory_";
        private const string FactoryMethodPrefix = "Create_";
        
        protected override bool ShouldSelect(MethodData value, ISelectionContext context) {
            var type = value.DeclaringType;

            return value.Inner.IsPrivate
                && type.Inner.Namespace == FactoryNamespace
                && type.Name.StartsWith(FactoryTypePrefix)
                && (
                    value.Name.StartsWith(FactoryMethodPrefix) && value.Inner.IsStatic
                    ||
                    value.Inner is ConstructorInfo
                );
        }
    }
}
