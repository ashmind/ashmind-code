using System;
using System.Linq;

using MbUnit.Framework;

using AshMind.Code.Analysis;
using AshMind.Code.Usage.Strategies.Specific.Web;

namespace AshMind.Code.Usage.Tests.Of.Strategies.Specific.Web {
    [TestFixture]
    public class GlobalAsaxMethodsSelectionStrategyTest {
        [Test]
        public void TestSelectFrom() {
            var globalAsaxMethods = new AnalysisDataResolver()
                                            .Resolve(typeof(GlobalAsaxStub))
                                            .GetMethods();

            var selectedMethods = new GlobalAsaxMethodsSelectionStrategy()
                   .SelectFrom(globalAsaxMethods, null)
                   .ToArray();

            Assert.AreElementsEqualIgnoringOrder(globalAsaxMethods, selectedMethods);
        }
    }
}
