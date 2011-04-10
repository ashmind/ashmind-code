using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AshMind.Code.Analysis.Collections {
    public abstract class ReadOnlyCollection<T> : IList<T> {
        public abstract T this[int index] { get; }

        #region IList<T> Members

        public abstract int IndexOf(T item);

        void IList<T>.Insert(int index, T item) {
            throw new NotSupportedException();
        }

        void IList<T>.RemoveAt(int index) {
            throw new NotSupportedException();
        }

        T IList<T>.this[int index] {
            get { return this[index]; }
            set { throw new NotSupportedException(); }
        }

        #endregion

        #region ICollection<T> Members

        void ICollection<T>.Add(T item) {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear() {
            throw new NotSupportedException();
        }

        public virtual bool Contains(T item) {
            return this.IndexOf(item) > -1;
        }

        public abstract void CopyTo(T[] array, int arrayIndex);

        public abstract int Count { get; }

        public bool IsReadOnly {
            get { return true; }
        }

        bool ICollection<T>.Remove(T item) {
            throw new NotSupportedException();
        }

        #endregion

        #region IEnumerable<T> Members

        public abstract IEnumerator<T> GetEnumerator();

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        #endregion
    }
}
