using System;
using System.Linq;
using System.Reflection;
using System.Web.Services;

using AshMind.Code.Analysis;

namespace AshMind.Code.Usage.Strategies.Specific.Web {
    internal class WebMethodsSelectionStrategy : IndependentSelectionStrategyBase<MethodData> {
        private static readonly string WebMethodAttributeFullName = typeof(WebMethodAttribute).AssemblyQualifiedName;
        private static readonly string WebServiceAttributeFullName = typeof(WebServiceAttribute).AssemblyQualifiedName;

        protected override bool ShouldSelect(MethodData value, ISelectionContext context) {
            return IsWebMethod(value) || IsWebServiceConstructor(value);
        }

        private bool IsWebServiceConstructor(MethodData value) {
            return value.Inner is ConstructorInfo
                && value.Inner.GetParameters().Length == 0
                && HasAttribute(value.DeclaringType, WebServiceAttributeFullName); 
        }

        private bool IsWebMethod(MethodData value) {
            return value.Inner.IsPublic 
                && value.Inner is MethodInfo
                && HasAttribute(value, WebMethodAttributeFullName);
        }

        private bool HasAttribute(IMemberData member, string attributeAssemblyQualifiedName) {
            return CustomAttributeData.GetCustomAttributes(member.Inner)
                                      .Any(data => data.Constructor.DeclaringType.AssemblyQualifiedName == attributeAssemblyQualifiedName);
        }
    }
}
