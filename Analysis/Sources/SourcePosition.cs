using System;
using System.Collections.Generic;
using System.Linq;

namespace AshMind.Code.Analysis.Sources {
    public struct SourcePosition {
        public SourcePosition(int row, int column) : this() {
            this.Row = row;
            this.Column = column;
        }

        public int Row { get; private set; }
        public int Column { get; private set; }
    }
}
