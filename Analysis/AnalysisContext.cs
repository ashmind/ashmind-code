using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Code.Analysis.Internal;
using AshMind.Code.Analysis.Sources;

namespace AshMind.Code.Analysis {
    public class AnalysisContext {
        public AnalysisDataResolver Resolver { get; private set; }
        internal SourceProvider SourceProvider { get; private set; }
        internal AssemblyLoader AssemblyLoader { get; private set; }
        
        public AnalysisContext() {
            this.Resolver = new AnalysisDataResolver(this);
            this.SourceProvider = new SourceProvider();
            this.AssemblyLoader = new AssemblyLoader();
        }

        public AnalysisContext(AnalysisDataResolver resolver) {
            this.Resolver = resolver;
            this.SourceProvider = new SourceProvider();
            this.AssemblyLoader = new AssemblyLoader();
        }
    }
}
