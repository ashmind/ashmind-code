using System;
using System.Collections.Generic;
using System.Linq;

namespace AshMind.Code.Analysis.Tests.Of.Internal.TestHierarchy {
    public class TestInheritedClass : TestBaseClass, TestInterface {
        public override void TestMethod() {
            base.TestMethod();
        }
    }
}