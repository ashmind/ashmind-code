using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AshMind.Code.Analysis.Sources;

namespace AshMind.Code.Analysis.Internal.MethodSupport {
    internal class MethodSource {
        private readonly MethodBase method;
        private readonly SourceProvider provider;
        private string[] lines;
        private SourceDocument document;

        public MethodSource(MethodBase method, SourceProvider provider) {
            this.method = method;
            this.provider = provider;
        }

        private void EnsureLoaded() {
            if (this.lines != null)
                return;

            this.Load();
        }

        private void Load() {
            this.lines = new string[0];
            var range = this.provider.GetSourceRanges(this.method).FirstOrDefault();

            if (range != null)
                this.document = range.Document;
        }

        public string[] Lines {
            get {
                this.EnsureLoaded();
                return this.lines;
            }
        }

        public SourceDocument Document {
            get {
                this.EnsureLoaded();
                return this.document;
            }
        }
    }
}