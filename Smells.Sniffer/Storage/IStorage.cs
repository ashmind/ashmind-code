using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AshMind.Code.Smells.Sniffer.Storage {
    public interface IStorage {
        void Save(string key, object data);
    }
}
