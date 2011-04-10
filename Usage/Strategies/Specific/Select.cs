using System;
using System.Linq;

using AshMind.Code.Analysis;
using AshMind.Code.Usage.Strategies.Specific.Web;

namespace AshMind.Code.Usage.Strategies.Specific {
    internal static class Select {
        public static MemberSelectionStrategy<MethodData> ExternallyVisibleMethods {
            get { return new ExternallyVisibleMethodsSelectionStrategy(); }
        }

        public static MemberSelectionStrategy<MethodData> StaticConstructors {
            get { return new StaticConstructorsSelectionStrategy(); }
        }

        public static MemberSelectionStrategy<MethodData> EmptyDefaultConstructors {
            get { return new EmptyDefaultConstructorsSelectionStrategy(); }
        }

        public static MemberSelectionStrategy<MethodData> OverridesOfExternalMethods {
            get { return new ExternalMethodOverridesSelectionStrategy(); }
        }

        public static MemberSelectionStrategy<IMemberData> GeneratedCode {
            get { return new GeneratedCodeSelectionStrategy(); }
        }

        public static class Web {
            public static MemberSelectionStrategy<MethodData> HttpApplicationEvents {
                get { return new GlobalAsaxMethodsSelectionStrategy(); }
            }

            public static MemberSelectionStrategy<MethodData> Methods {
                get { return new WebMethodsSelectionStrategy(); }
            }

            public static MemberSelectionStrategy<MethodData> FactoryMethods {
                get { return new AspNetFactoryMethodsSelectionStrategy(); }
            }
        }

        public static ISelectionStrategy<T> Many<T>(params ISelectionStrategy<T>[] strategies) {
            return new UnifiedSelectionStrategy<T>(strategies);
        }
    }
}
