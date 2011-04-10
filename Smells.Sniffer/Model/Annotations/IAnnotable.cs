using System;
using System.Collections.Generic;
using System.Linq;

namespace AshMind.Code.Smells.Sniffer.Model.Annotations {
    internal interface IAnnotable {
        void Annotate<T>(T annotation)
            where T : class;

        void Deannotate<T>()
            where T : class;

        T Annotation<T>()
            where T : class;
    }
}
