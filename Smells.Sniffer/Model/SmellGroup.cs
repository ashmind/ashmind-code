using System;
using System.Collections.Generic;
using System.Linq;

namespace AshMind.Code.Smells.Sniffer.Model {
    public class SmellGroup {
        public SmellGroup(string name, IEnumerable<SmellInfo> smells) {
            this.Name = name;
            this.Smells = smells.ToArray();
        }

        public SmellInfo[] Smells { get; private set; }
        public string Name        { get; private set; }
    }
}
