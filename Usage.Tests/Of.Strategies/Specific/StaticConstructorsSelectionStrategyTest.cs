using System;
using System.Linq;
using System.Reflection;

using MbUnit.Framework;

using AshMind.Code.Analysis;
using AshMind.Code.Analysis;
using AshMind.Code.Usage.Strategies;
using AshMind.Code.Usage.Strategies.Specific;

namespace AshMind.Code.Usage.Tests.Of.Strategies.Specific {
    [TestFixture]
    public class StaticConstructorsSelectionStrategyTest {
        // ReSharper disable EmptyConstructor
        static StaticConstructorsSelectionStrategyTest() {
            // just for test
        }
        // ReSharper restore EmptyConstructor

        [Test]
        [Row("Should select static constructor",       BindingFlags.Static | BindingFlags.NonPublic, true)]
        [Row("Should not select instance constructor", BindingFlags.Instance | BindingFlags.Public, false)]
        public void TestShouldSelectWithDifferentConstructors(string description, BindingFlags flags, bool shouldSelect) {
            var constructor = new AnalysisDataResolver().Resolve(this.GetType().GetConstructors(flags)[0]);

            bool result = new TestableStaticConstructorsSelectionStrategy().ShouldSelect(constructor, null);
            Assert.AreEqual(shouldSelect, result);
        }

        private class TestableStaticConstructorsSelectionStrategy : StaticConstructorsSelectionStrategy {
            public new bool ShouldSelect(MethodData method, ISelectionContext context) {
                return base.ShouldSelect(method, context);
            }
        }
    }
}
