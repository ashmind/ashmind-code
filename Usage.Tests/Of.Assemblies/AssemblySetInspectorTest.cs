using System;
using System.Collections.Generic;
using System.Linq;

using Moq;

using MbUnit.Framework;

using AshMind.Code.Analysis;
using AshMind.Code.Usage.Assemblies;

namespace AshMind.Code.Usage.Tests.Of.Assemblies {
    [TestFixture]
    public class AssemblySetInspectorTest {
        [Test]
        public void TestPartitionSelectsCorrectRoots() {
            var nodes = new[] { MockAssembly(), MockAssembly(), MockAssembly() }.ToList();
            nodes.Add(MockAssembly(nodes[0]));

            var roots = new[] {
                MockAssembly(nodes[1], nodes[2]),
                MockAssembly(nodes[0], nodes[3])
            };

            var result = new AssemblySetInspector().Partition(roots.Concat(nodes));

            Assert.IsTrue(result.Roots.SetEquals(roots));
            Assert.IsTrue(result.Other.SetEquals(nodes));
        }

        private IAssemblyData MockAssembly(params IAssemblyData[] references) {
            var mock = new Mock<IAssemblyData>();
            mock.ExpectGet(x => x.References).Returns(references ?? new IAssemblyData[0]);

            return mock.Object;
        }
    }
}
