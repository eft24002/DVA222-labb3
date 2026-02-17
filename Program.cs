using System;
using System.Collections;
using System.Collections.Generic;

namespace DVA222_Map
{
    public class Map<K, V> : IDictionary<K, V>
    {
        private List<KeyValuePair<K, V>>[] buckets;
        private int count;
        public Map()
        {
            buckets = new List<KeyValuePair<K, V>>[16];
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new List<KeyValuePair<K, V>>();
            }
            count = 0;
        }
        private int GetBucketIndex(K key)
        {
            int hash = key!.GetHashCode();
            return Math.Abs(hash) % buckets.Length; 
        }

        public V this[K key] 
        { 
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); 
        }

        public ICollection<K> Keys => throw new NotImplementedException();     
        public ICollection<V> Values => throw new NotImplementedException();
        public int Count => count;
        public bool IsReadOnly => false;
        
        //Metoder
        public void Add(KeyValuePair<K, V> item) => Add(item.Key, item.Value);
        public void Add(K key, V value)
        {
            //  
        }

        public void Clear()
        {
            
        }

        public bool Contains(KeyValuePair<K, V> item) => ContainsKey(item.Key);   
        public bool ContainsKey(K key)
        {
            //
            return false;
        }
        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            //
        }

        public bool Remove(KeyValuePair<K, V> item) => Remove(item.Key);
        public bool Remove(K key)
        {
            //
            return false;
        }

        //public bool TryGetValue(KeyValuePair<K, V> item) => TryGetValue(item.Key, out V value);
        public bool TryGetValue(K key, out V value)
        {
            //
            value = default(V)!;
            return false;
        }
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach (var bucket in buckets)
            {
                foreach (var pair in bucket)
                {
                    yield return pair;
                }
            }
        }
    
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        
        







                                            




         

    }
}