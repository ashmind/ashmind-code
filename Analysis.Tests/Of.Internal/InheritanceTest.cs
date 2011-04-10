using System;
using System.Linq;
using AshMind.Code.Analysis.Internal;
using AshMind.Code.Analysis.Tests.Of.Internal.TestHierarchy;
using MbUnit.Framework;

namespace AshMind.Code.Analysis.Tests.Of.Internal {
    [TestFixture]
    public class InhertianceTest {
        [Test]
        public void TestGetBaseAndImplementedMethods() {
            const string TestMethodName = "TestMethod";
            var resolver = new AnalysisDataResolver();

            var methods = Inheritance.GetBaseAndImplementedMethods(
                resolver.Resolve(typeof(TestInheritedClass).GetMethod(TestMethodName))
            );

            Assert.AreElementsEqualIgnoringOrder(
                methods.ToArray(), new[] { 
                    resolver.Resolve(typeof(TestInterface).GetMethod(TestMethodName)),
                    resolver.Resolve(typeof(TestBaseClass).GetMethod(TestMethodName))
                }
            );
        }
    }
}