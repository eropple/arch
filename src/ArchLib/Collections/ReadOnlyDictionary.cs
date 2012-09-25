using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArchLib.Collections
{
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _backingDictionary;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> backingDictionary)
        {
            _backingDictionary = backingDictionary;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _backingDictionary.GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new InvalidOperationException("ReadOnlyDictionary doesn't support this operation.");
        }

        public void Clear()
        {
            _backingDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _backingDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _backingDictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _backingDictionary.Remove(item);
        }

        public int Count
        {
            get { return _backingDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return _backingDictionary.IsReadOnly; }
        }

        public bool ContainsKey(TKey key)
        {
            return _backingDictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            _backingDictionary.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            throw new InvalidOperationException("ReadOnlyDictionary doesn't support this operation.");
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _backingDictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get { return _backingDictionary[key]; }
            set { throw new InvalidOperationException("ReadOnlyDictionary doesn't support this operation."); }
        }

        public ICollection<TKey> Keys
        {
            get { return _backingDictionary.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return _backingDictionary.Values; }
        }
    }
}
