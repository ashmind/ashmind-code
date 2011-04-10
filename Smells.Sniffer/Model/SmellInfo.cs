using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AshMind.Code.Smells.Sniffer.Model {
    public class SmellInfo {
        public SmellInfo(ISmell smell) {
            this.Smell = smell;
            this.ParseName();
        }

        private void ParseName() {
            this.Name = string.Join(
                " ", Regex.Split(this.Smell.GetType().Name, "(?<=[a-z])(?=[A-Z])")
            );
        }

        public string Name { get; private set; }
        public ISmell Smell { get; private set; }
    }
}
