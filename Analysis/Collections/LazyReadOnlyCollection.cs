using System;
using System.Collections.Generic;
using System.Linq;

using AshMind.Extensions;

namespace AshMind.Code.Analysis.Collections {
    public class LazyReadOnlyCollection<T> : ReadOnlyCollection<T> {
        private readonly IList<T> inner = new List<T>();
        private readonly IEnumerator<T> sourceEnumerator;
        private bool completed;

        private Action<T> lazyForEach;

        public LazyReadOnlyCollection(IEnumerable<T> source) {
            this.sourceEnumerator = source.GetEnumerator();
        }

        public bool Loaded {
            get { return this.completed; }
        }

        public void LazyForEach(Action<T> action) {
            this.inner.ForEach(action);
            if (this.completed)
                return;

            lazyForEach += action;
        }

        private void ApplyLazyForEach(T item) {
            if (this.lazyForEach != null)
                this.lazyForEach(item);
        }

        private void MoveToEnd() {
            this.MoveUntil((item, i) => false);
        }

        private void MoveTo(int index) {
            if (index <= this.inner.Count - 1)
                return;

            this.MoveUntil((item, i) => i == index);
        }

        private int? MoveUntil(Func<T, int, bool> condition) {
            if (this.completed)
                return null;

            var index = inner.Count - 1;
            T item;
            do {
                var hasNext = sourceEnumerator.MoveNext();
                if (!hasNext) {
                    sourceEnumerator.Dispose();
                    this.completed = true;
                    return null;
                }

                item = sourceEnumerator.Current;
                index += 1;

                this.ApplyLazyForEach(item);

                this.inner.Add(item);
            } while (!condition(item, index));

            return index;
        }

        public override T this[int index] {
            get {
                this.MoveTo(index);
                return this.inner[index];
            }
        }

        public override int IndexOf(T item) {
            var index = this.inner.IndexOf(item);
            if (index != -1)
                return index;

            return this.MoveUntil((lastItem, lastIndex) => object.Equals(lastItem, item)) ?? -1;
        }

        public override int Count {
            get {
                this.MoveToEnd();
                return this.inner.Count;
            }
        }

        public override IEnumerator<T> GetEnumerator() {
            foreach (var item in this.inner) {
                yield return item;
            }

            if (this.completed)
                yield break;

            while (this.sourceEnumerator.MoveNext()) {
                var item = sourceEnumerator.Current;

                this.ApplyLazyForEach(item);

                this.inner.Add(item);
                yield return item;
            }
            
            this.sourceEnumerator.Dispose();
            this.completed = true;
        }

        public override void CopyTo(T[] array, int arrayIndex) {
            this.MoveToEnd();
            this.inner.CopyTo(array, arrayIndex);
        }
    }
}
