using System;
using System.Collections.Generic;
using System.Linq;

namespace AshMind.Code.Analysis.Internal {
    internal class Cache<TKey, TValue> {
        private readonly IDictionary<TKey, TValue> dataCache = new Dictionary<TKey, TValue>();

        public TActualValue Get<TActualValue>(TKey key, Func<TActualValue> create)
            where TActualValue : class, TValue 
        {
            TValue data;
            var cached = this.dataCache.TryGetValue(key, out data);
            if (!cached) {
                data = create();
                this.dataCache.Add(key, data);
            }

            return (TActualValue)data;
        }
    }
}