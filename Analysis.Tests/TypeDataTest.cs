using System;
using System.Collections.Generic;
using System.Linq;

using MbUnit.Framework;

using AshMind.Code.Analysis;

namespace AshMind.Code.Analysis.Tests {
    [TestFixture]
    public class TypeDataTest {
        #region Test Classes

        // ReSharper disable UnusedMemberInPrivateClass
        #pragma warning disable 67
        private class TestClass {
            public event EventHandler Event;
            public int Property { get; set; }
        }
        #pragma warning restore 67
        // ReSharper restore UnusedMemberInPrivateClass

        // ReSharper disable UnusedTypeParameter
        private class TestClass<T> {}
        private class TestClass<T1, T2> {}
        // ReSharper restore UnusedTypeParameter

        #endregion

        [RowTest]
        [Row(typeof(TestClass),                         "TestClass")]
        [Row(typeof(TestClass<TestClass>),              "TestClass<TestClass>")]
        [Row(typeof(TestClass<TestClass<TestClass>>),   "TestClass<TestClass<TestClass>>")]
        [Row(typeof(TestClass<TestClass, int>),         "TestClass<TestClass, Int32>")]
        [Row(typeof(TestClass<>),                       "TestClass<T>")]
        public void TestName(Type type, string expectedName) {
            var typeData = new AnalysisDataResolver().Resolve(type);

            Assert.AreEqual(expectedName, typeData.Name);
        }

        [RowTest]
        [Row("Property")]
        [Row("Event")]
        public void TestGetAllMembersReturnMethodsWithCorrectDeclaringMemberFor(string memberNameAndKind) {
            var type = new AnalysisDataResolver().Resolve<TestClass>();
            var accessors = type.GetAllMembers(false)
                                .OfType<MethodData>()
                                .Where(m => m.Name.EndsWith(memberNameAndKind))
                                .ToArray();

            var declaringMember = type.Members.Single(m => m.Name == memberNameAndKind);

            foreach (var accessor in accessors) {
                Assert.AreEqual(declaringMember, accessor.DeclaringMember);
            }
        }

        [RowTest]
        [Row("Property")]
        [Row("Event")]
        public void TestMembersExcludesAccessorsOf(string memberNameAndKind) {
            var type = new AnalysisDataResolver().Resolve<TestClass>();
            var member = type.Members.OfType<IWithAccessors>()
                                     .Single(m => m.Name == memberNameAndKind);

            CollectionAssert.IsNotSubsetOf(
                member.Accessors.ToArray(),
                type.Members.ToArray()
            );
        }
    }
}
