using System;
using System.Collections.Generic;
using System.Linq;

using AshMind.Code.Analysis;
using AshMind.Extensions;

namespace AshMind.Code.Usage {
    internal class UsedMemberCollector {
        public HashSet<IMemberData> Collected { get; private set; }

        private readonly IEnumerable<IMemberData> seed;
        private readonly Func<IMemberData, bool> filter;
        private readonly ILookup<MethodData, MethodData> methodsByBases;
        private readonly Queue<IMemberData> queued = new Queue<IMemberData>();

        public UsedMemberCollector(IEnumerable<IMemberData> seed, Func<IMemberData, bool> filter, ILookup<MethodData, MethodData> methodsByBases) {
            this.filter = filter;
            this.seed = seed;
            this.methodsByBases = methodsByBases;

            this.Collected = seed.ToSet();
        }

        public void CollectAll() {
            this.CollectAllUsedBy(this.seed);
        }

        private void CollectAllUsedBy(IEnumerable<IMemberData> members) {
            foreach (var member in members) {
                this.CollectAllUsedBy(member);
            }
        }

        private void CollectAllUsedBy(IMemberData member) {
            this.CollectDirectlyUsedBy(member);

            while (this.queued.Count > 0) {
                var item = this.queued.Dequeue();
                this.CollectAllUsedBy(item);
            }
        }

        private void CollectDirectlyUsedBy(IMemberData member) {
            var method = member as MethodData;
            if (method == null)
                return; // stub

            this.CollectDirectlyUsedBy(method);
        }

        private void CollectDirectlyUsedBy(MethodData method) {
            method.GetReferencedMethods().Cast<IMemberData>().ForEach(this.Collect);
            this.methodsByBases[method].Cast<IMemberData>().ForEach(this.Collect);

            var generic = method.GetGenericDefinition();
            if (generic != null)
                this.Collect(generic);

            var propertyOrEvent = method.DeclaringMember;
            if (propertyOrEvent != null)
                this.Collect(propertyOrEvent);
        }

        private void Collect(IMemberData member) {
            if (!this.filter(member))
                return;

            if (!this.Collected.Add(member))
                return;
            this.queued.Enqueue(member);
        }
    }
}
