using System;
using System.Collections.Generic;
using System.Linq;

using AshMind.Extensions;

namespace AshMind.Code.Usage.Extensions {
    internal static class Graph {
        public static HashSet<T> SelectDescendantsAndSelves<T>(this IEnumerable<T> nodes, Func<T, IEnumerable<T>> selector) {
            var nodeSet = nodes.ToSet();
            var descendants = nodeSet.SelectDescendants(selector);

            nodeSet.UnionWith(descendants);

            return nodeSet;
        }

        public static HashSet<T> SelectDescendants<T>(this IEnumerable<T> nodes, Func<T, IEnumerable<T>> selector) {
            var collected = new HashSet<T>();
            
            var repeat = true;
            var lastLevel = nodes;

            while (repeat) {
                var level = (
                    from node in lastLevel
                    from child in selector(node)
                    where !collected.Contains(child)
                    select child
                ).ToArray();

                repeat = level.Length > 0;
                collected.AddRange(level);
                lastLevel = level;
            }

            return collected;
        }
    }
}
