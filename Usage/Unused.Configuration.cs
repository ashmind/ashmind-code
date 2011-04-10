using System;
using System.Linq;

using AshMind.Code.Analysis;
using AshMind.Code.Usage.Strategies.Specific;

namespace AshMind.Code.Usage {
    partial class Unused {
        private static readonly UnusedMemberFinderConfiguration configuration = new UnusedMemberFinderConfiguration {
            Entry = Select.Many<IMemberData>(
                                  Select.ExternallyVisibleMethods,
                                  Select.StaticConstructors
                              ),

            DefinitelyUsed = Select.Many<IMemberData>(
                                  Select.OverridesOfExternalMethods,
                                  Select.StaticConstructors,
                                  Select.Web.HttpApplicationEvents,
                                  Select.Web.Methods,
                                  Select.Web.FactoryMethods
                              ),

            Ignored = Select.Many<IMemberData>(
                                  Select.GeneratedCode,
                                  Select.EmptyDefaultConstructors
                              )
        };
    }
}
