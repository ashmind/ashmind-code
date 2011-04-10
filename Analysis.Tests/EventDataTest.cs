using System;
using System.Collections.Generic;
using System.Linq;

using MbUnit.Framework;

namespace AshMind.Code.Analysis.Tests {
    [TestFixture]
    public class EventDataTest {
        #region TestClass

        #pragma warning disable 67
        private class TestClass {
            private event EventHandler PrivateEvent;
        }
        #pragma warning restore 67

        #endregion

        [Test]
        public void TestAccessorsIncludePrivateMembers() {
            var testClass = new AnalysisDataResolver().Resolve<TestClass>();
            var @event = testClass.GetEvents().Where(e => e.Name == "PrivateEvent").Single();

            Assert.AreEqual(2, @event.Accessors.Count);
        }
    }
}
