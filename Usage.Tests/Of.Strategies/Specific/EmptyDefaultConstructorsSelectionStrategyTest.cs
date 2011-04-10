using System;
using System.Linq;

using MbUnit.Framework;

using AshMind.Code.Analysis;
using AshMind.Code.Usage.Strategies;
using AshMind.Code.Usage.Strategies.Specific;
using AshMind.Code.Usage.Tests.Of.Strategies.Specific.TestConstructors;

namespace AshMind.Code.Usage.Tests.Of.Strategies.Specific {
    [TestFixture]
    public class EmptyDefaultConstructorsSelectionStrategyTest {
        [Test]
        [Row(typeof(ClassWithEmptyConstructor),     true)]
        [Row(typeof(EmptyClass),                    true)]
        [Row(typeof(InheritingEmptyClass),          true)]
        [Row(typeof(ClassWithNotEmptyConstructor),  false)]
        public void Test(Type type, bool expectTrue) {
            var constructor = new AnalysisDataResolver().Resolve(type.GetConstructor(Type.EmptyTypes));
            var strategy = new TestableEmptyDefaultConstructorsSelectionStrategy();

            bool result = strategy.ShouldSelect(constructor, null);
            if (expectTrue)
                Assert.IsTrue(result);
            else
                Assert.IsFalse(result);
        }

        private class TestableEmptyDefaultConstructorsSelectionStrategy : EmptyDefaultConstructorsSelectionStrategy {
            public new bool ShouldSelect(MethodData method, ISelectionContext context) {
                return base.ShouldSelect(method, context);
            }
        }
    }
}