using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AshMind.Code.Smells.Sniffer.Internal {
    internal class CollectionPair<T> {
        private readonly ObservableCollection<T> inner = new ObservableCollection<T>();
        private readonly ReadOnlyObservableCollection<T> readOnly;

        public CollectionPair() {
            this.readOnly = new ReadOnlyObservableCollection<T>(Inner);
        }

        public ObservableCollection<T> Inner {
            get { return inner; }
        }

        public ReadOnlyObservableCollection<T> ReadOnly {
            get { return readOnly; }
        }
    }
}
