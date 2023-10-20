using System;
using System.Collections.Generic;

namespace Utilities {
    public class LowDictionary<TValue>
    {
        private Dictionary<string, TValue> dictionary = new Dictionary<string, TValue>();

        public TValue this[string key]
        {
            get
            {
                if (dictionary.ContainsKey(key.ToLower()))
                {
                    return dictionary[key.ToLower()];
                }
                else
                {
                    throw new KeyNotFoundException($"Key '{key}' not found in the dictionary.");
                }
            }
            set
            {
                dictionary[key.ToLower()] = value;
            }
        }

        public bool ContainsKey(string key) {
            return dictionary.ContainsKey(key.ToLower());
        }
    }
}
