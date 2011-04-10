using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;

namespace AshMind.Code.Analysis.Sources {
    public class SourceDocument {
        public string[] Lines { get; private set; }
        public string Content { get; private set; }
        public string Location { get; private set; }

        internal ISymbolDocument Symbols { get; private set; }

        internal SourceDocument(string[] lines, string location, ISymbolDocument symbols) {
            this.Lines = lines;
            this.Content = string.Join(Environment.NewLine, lines);

            this.Location = location;

            this.Symbols = symbols;
        }
    }
}
