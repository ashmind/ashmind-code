using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace AshMind.Code.Smells.Sniffer.Internal {
    public class BoundNameScope : INameScope {
        public DependencyObject Target { get; set; }

        public void RegisterName(string name, object scopedElement) {
            NameScope.GetNameScope(this.Target).RegisterName(name, scopedElement);
        }

        public void UnregisterName(string name) {
            NameScope.GetNameScope(this.Target).UnregisterName(name);
        }

        public object FindName(string name) {
            return NameScope.GetNameScope(this.Target).FindName(name);
        }
    }
}
