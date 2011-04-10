using System;
using System.Collections.Generic;
using System.Linq;

using MbUnit.Framework;

using AshMind.Code.Smells.Sniffer.Model.Annotations;

namespace AshMind.Code.Smells.Sniffer.Tests.Of.Model.Annotations {
    [TestFixture]
    public class AnnotationSupportTest {
        private class Annotation {}

        [Test]
        public void TestAnnotateAddsToAnnotations() {
            var support = new AnnotationSupport();
            var annotation = new Annotation();

            support.Annotate(annotation);

            Assert.AreSame(annotation, support.Annotation<Annotation>());
        }

        [Test]
        public void TestAnnotateTwiceUsesLastValue() {
            var support = new AnnotationSupport();
            var annotations = new[] { new Annotation(), new Annotation() };

            support.Annotate(annotations[0]);
            support.Annotate(annotations[1]);

            Assert.AreSame(annotations[1], support.Annotation<Annotation>());
        }
    }
}
