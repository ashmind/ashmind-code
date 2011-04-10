using System;
using System.Collections.Generic;
using System.Linq;

using MbUnit.Framework;

using AshMind.Code.Analysis.Collections;

namespace AshMind.Code.Analysis.Tests.Of.Collections {
    [TestFixture]
    public class LazyReadOnlyCollectionTest {
        #region MutableClass
        
        private class MutableClass {
            public string Text { get; set; }
        }

        #endregion

        [Test]
        public void TestLazyForEachIsAppliedWhenEnumerating() {
            var source = Enumerable.Range(0, 10)
                                   .Select(i => new MutableClass { Text = i.ToString() });

            var collection = new LazyReadOnlyCollection<MutableClass>(source);
            collection.LazyForEach(m => m.Text = "changed");
        
            foreach (var item in collection) {
                Assert.AreEqual("changed", item.Text);        
            }
        }
    }
}
