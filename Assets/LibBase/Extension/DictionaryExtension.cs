using System;
using System.Collections.Generic;

namespace LibBase.Extension {
    public class DictionarySafe<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public DictionarySafe() : base()
        {
            
        }

        public DictionarySafe(int capacity) : base(capacity)
        {
            
        }

        public DictionarySafe(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
            
        }

        public DictionarySafe(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary,
            comparer)
        {
            
        }

        public new TValue this[TKey key] {
            get
            {
                TValue value;
                TryGetValue(key, out value);
                return value;
            }
            set { base[key] = value; }
        }
    }
}