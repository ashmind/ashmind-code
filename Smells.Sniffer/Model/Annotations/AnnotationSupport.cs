using System;
using System.Collections.Generic;
using System.Linq;

namespace AshMind.Code.Smells.Sniffer.Model.Annotations {
    internal class AnnotationSupport : IAnnotable {
        private readonly IDictionary<Type, object> annotations = new Dictionary<Type, object>();

        public T Annotation<T>()
            where T : class 
        {
            object value;
            var found = annotations.TryGetValue(typeof(T), out value);
            return found ? (T)value : null;
        }

        public void Annotate<T>(T annotation)
            where T : class 
        {
            if (annotation == null)
                throw new ArgumentNullException("annotation");

            annotations[typeof(T)] = annotation;
        }

        public void Deannotate<T>() 
            where T : class 
        {
            annotations.Remove(typeof(T));
        }
    }
}
