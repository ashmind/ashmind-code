using System;
using System.Linq;
using System.Reflection;

using MbUnit.Framework;

using AshMind.Code.Analysis;
using AshMind.Code.Usage.Strategies;
using AshMind.Code.Usage.Strategies.Specific;
using AshMind.Code.Usage.Tests.Of.Strategies.Specific.TestCodeGenerated;

namespace AshMind.Code.Usage.Tests.Of.Strategies.Specific {
    [TestFixture]
    public class CodeGeneratedMethodsSelectionStrategyTest {
        private AnalysisDataResolver analyzer;

        [FixtureSetUp]
        public void SetUp() {
            //AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += (sender, args) => {
            //    return Assembly.ReflectionOnlyLoad(args.Name);
            //};
            //this.assembly = Assembly.ReflectionOnlyLoadFrom(this.GetType().Assembly.Location);

            this.analyzer = new AnalysisDataResolver();
        }
        
        [Test]
        [Row(typeof(ClassWithCodeGeneratedMethod),                  "CodeGeneratedMethod")]
        [Row(typeof(IInterfaceWithCodeGeneratedMethod),             "CodeGeneratedMethod")]
        [Row(typeof(CodeGeneratedClass),                            ".ctor")]
        [Row(typeof(CodeGeneratedClass),                            "TestMethod")]
        [Row(typeof(CodeGeneratedClassWithNestedClass.NestedClass), "TestMethod")]
        public void Test(Type type, string methodName) {
            var testClass = this.analyzer.Resolve(type);

            var testMethod = (methodName != ConstructorInfo.ConstructorName)
                ? testClass.GetMethods().Single(m => m.Inner.Name == methodName)
                : testClass.GetConstructors().Single();

            var strategy = new TestableCodeGeneratedMethodsSelectionStrategy();
            Assert.IsTrue(strategy.ShouldSelect(testMethod, null));
        }
        
        private class TestableCodeGeneratedMethodsSelectionStrategy : GeneratedCodeSelectionStrategy {
            public new bool ShouldSelect(IMemberData method, ISelectionContext context) {
                return base.ShouldSelect(method, context);
            }
        }
    }
}