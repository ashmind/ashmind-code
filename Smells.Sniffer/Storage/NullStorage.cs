using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AshMind.Code.Smells.Sniffer.Storage {
    internal class NullStorage : IStorage {
        public void Save(string key, object data) {
        }
    }
}
