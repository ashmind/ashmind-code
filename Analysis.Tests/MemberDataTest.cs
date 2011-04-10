using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MbUnit.Framework;

using AshMind.Code.Analysis.Tests.TestGenerics;

namespace AshMind.Code.Analysis.Tests {
    [TestFixture]
    public class MemberDataTest {
        [Test]
        public void TestGetGenericDefinitionOnNonGenericMethod() {
            var original = typeof(GenericClass<int>).GetMethod("NonGenericMethod", Type.EmptyTypes);
            var expected = typeof(GenericClass<>).GetMethod("NonGenericMethod", Type.EmptyTypes);

            this.TestGetGenericDefinition(original, expected);
        }

        [Test]
        public void TestGetGenericDefinitionOnGenericMethod() {
            var original = typeof(GenericClass<int>).GetMethod("GenericMethod", Type.EmptyTypes).MakeGenericMethod(typeof(int));
            var expected = typeof(GenericClass<>).GetMethod("GenericMethod", Type.EmptyTypes);

            this.TestGetGenericDefinition(original, expected);
        }

        [Test]
        public void TestGetGenericDefinitionOnPropertyAccessor() {
            var original = typeof(GenericClass<int>).GetProperty("NonGenericProperty").GetGetMethod();
            var expected = typeof(GenericClass<>).GetProperty("NonGenericProperty").GetGetMethod();

            this.TestGetGenericDefinition(original, expected);
        }

        [Test]
        public void TestGetGenericDefinitionOnConstructor() {
            var original = typeof(GenericClass<int>).GetConstructor(Type.EmptyTypes);
            var expected = typeof(GenericClass<>).GetConstructor(Type.EmptyTypes);

            this.TestGetGenericDefinition(original, expected);
        }

        [Test]
        public void TestGetGenericDefinitionOnOperator() {
            var original = typeof(GenericClass<int>).GetMethod("op_Addition");
            var expected = typeof(GenericClass<>).GetMethod("op_Addition");

            this.TestGetGenericDefinition(original, expected);
        }

        private void TestGetGenericDefinition(MethodBase original, MethodBase expected) {
            var originalData = new AnalysisDataResolver().Resolve(original);
            var foundData = originalData.GetGenericDefinition();

            Assert.AreEqual(expected, foundData.Inner);
        }
    }
}
