using System;
using System.Collections.Generic;

namespace ArchLib.Utility.ObjectModel
{
    /// <summary>
    /// A lookup table that wraps an IDictionary. Conceals all methods except an indexer, Keys, and Values.
    /// </summary>
    public class LookupTable<TKey, TValue>
        where TValue : class
    {
        private readonly IDictionary<TKey, TValue> _backingStore;

        public LookupTable(IDictionary<TKey, TValue> backingStore)
        {
            _backingStore = backingStore;
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue val = null;
                return _backingStore.TryGetValue(key, out val) ? val : null;
            }
        }

        public ICollection<TKey> Keys { get { return _backingStore.Keys; } }
        public ICollection<TValue> Values { get { return _backingStore.Values; } }
    }
}
