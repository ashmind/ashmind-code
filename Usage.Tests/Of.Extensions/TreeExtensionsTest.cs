using System;
using System.Collections.Generic;
using System.Linq;

using MbUnit.Framework;

using AshMind.Code.Usage.Extensions;

namespace AshMind.Code.Usage.Tests.Of.Extensions {
    [TestFixture]
    public class TreeExtensionsTest {
        #region Graph Class

        private class Graph {
            public string Name { get; private set; }
            public IList<Graph> Links { get; private set; }

            public Graph(string name) {
                this.Name = name;
                this.Links = new List<Graph>();
            }

            public override string ToString() {
                return this.Name;
            }
        }

        #endregion

        [Test]
        public void TestSelectDescendantsAndSelves() {
            var graphs = Enumerable.Range(1, 5)
                                   .Select(i => new Graph(i.ToString()))
                                   .ToArray();

            Action<int, int> link = (index1, index2) => 
                graphs[index1 - 1].Links.Add(graphs[index2 - 1]);

            link(1, 2);
            link(1, 3);
            link(2, 1);
            link(2, 4);
            link(3, 2);
            link(3, 4);
            link(4, 5);
            link(4, 5);

            var descendants = new[] { graphs[0] }.SelectDescendantsAndSelves(g => g.Links).ToArray();

            CollectionAssert.AreEquivalent(graphs, descendants);
            CollectionAssert.AllItemsAreUnique(descendants);
        }
    }
}
