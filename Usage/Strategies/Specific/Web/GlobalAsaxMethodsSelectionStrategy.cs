using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AshMind.Code.Analysis;

namespace AshMind.Code.Usage.Strategies.Specific.Web {
    internal class GlobalAsaxMethodsSelectionStrategy : MemberSelectionStrategy<MethodData> {
        private static readonly string ApplicationTypeName = typeof(HttpApplication).AssemblyQualifiedName;
        private static readonly HashSet<string> ApplicationEventNames = new HashSet<string> {
            "Application_Start",
            "Application_BeginRequest",
            "Application_AuthenticateRequest",
            "Application_AuthorizeRequest",
            "Application_ResolveRequestCache",
            "Application_AcquireRequestState",
            "Application_PreRequestHandlerExecute",
            "Application_PostRequestHandlerExecute",
            "Application_ReleaseRequestState",
            "Application_UpdateRequestCache",
            "Application_EndRequest",
            "Application_PreSendRequestHeaders",
            "Application_PreSendRequestContent",
            "Application_Error",
            "Session_End",
            "Session_Start"
        };

        public override IEnumerable<MethodData> SelectFrom(IEnumerable<MethodData> original, ISelectionContext context) {
            return from method in original
                   where ApplicationEventNames.Contains(method.Inner.Name) && !method.Inner.IsPrivate
                   where this.GetBaseTypes(method.DeclaringType.Inner).Any(type => type.AssemblyQualifiedName == ApplicationTypeName)
                   let parameters = method.Inner.GetParameters()
                   where parameters.Length == 2
                      && parameters[0].ParameterType == typeof(object)
                      && parameters[1].ParameterType == typeof(EventArgs)
                   select method;
        }

        private IEnumerable<Type> GetBaseTypes(Type type) {
            type = type.BaseType;
            while (type != typeof(object) && type != null) {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}
