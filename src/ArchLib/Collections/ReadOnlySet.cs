using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArchLib.Collections
{
    /// <summary>
    /// A read-only adapter on top of an ISet[T] implementation.
    /// </summary>
    /// <typeparam name="T">The contained type.</typeparam>
    public class ReadOnlySet<T> : ISet<T>
    {
        private readonly ISet<T> _backingSet;

        public ReadOnlySet(ISet<T> backingSet)
        {
            _backingSet = backingSet;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _backingSet.GetEnumerator();
        }

        void ICollection<T>.Add(T item)
        {
            throw new InvalidOperationException("Not supported by ReadOnlySet.");
        }

        public void Clear()
        {
            throw new InvalidOperationException("Not supported by ReadOnlySet.");
        }

        public bool Contains(T item)
        {
            return _backingSet.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _backingSet.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            throw new InvalidOperationException("Not supported by ReadOnlySet.");
        }

        public int Count
        {
            get { return _backingSet.Count; }
        }

        public bool IsReadOnly { get { return false; } }

        public bool Add(T item)
        {
            throw new InvalidOperationException("Not supported by ReadOnlySet.");
        }

        public void UnionWith(IEnumerable<T> other)
        {
            _backingSet.UnionWith(other);
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            _backingSet.IntersectWith(other);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            _backingSet.ExceptWith(other);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            _backingSet.SymmetricExceptWith(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return _backingSet.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return _backingSet.IsSupersetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return _backingSet.IsProperSupersetOf(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return _backingSet.IsProperSubsetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return _backingSet.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return _backingSet.SetEquals(other);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
