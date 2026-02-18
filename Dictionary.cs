
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
            if (key is null) throw new ArgumentNullException(nameof(key));
            int hash = key.GetHashCode();
            return Math.Abs(hash) % buckets.Length; 
        }

        public V this[K key] 
        { 
            get
            {
                int bucketIndex = GetBucketIndex(key);
                List<KeyValuePair<K, V>> bucket = buckets[bucketIndex];

                for(int i = 0; i < bucket.Count; i++)   
                {
                    if(EqualityComparer<K>.Default.Equals(bucket[i].Key, key))
                    {
                        return bucket[i].Value;
                    }
                }
                throw new KeyNotFoundException();
            }
            set
            {
                int bucketIndex = GetBucketIndex(key);
                List<KeyValuePair<K, V>> bucket = buckets[bucketIndex];
                for(int i = 0; i < bucket.Count; i++)   
                {
                    if(EqualityComparer<K>.Default.Equals(bucket[i].Key, key))
                    {
                        bucket[i] = new KeyValuePair<K, V>(key, value);
                        return;
                    }
                }
                bucket.Add(new KeyValuePair<K, V>(key, value));
                count++;
            }
        }

        public ICollection<K> Keys 
        { 
            get
            {
                var keys = new List<K>();
                foreach (var bucket in buckets)
                {
                    foreach (var keyValuePair in bucket)
                    {
                        keys.Add(keyValuePair.Key);
                    }
                }
                return keys;
            }
        }    

        public ICollection<V> Values 
        { 
            get
            {
                var values = new List<V>();

                foreach (var bucket in buckets)
                {
                    foreach (var keyValuePair in bucket)
                    {
                        values.Add(keyValuePair.Value);
                    }
                }
                return values;
            }
        }

        public int Count => count;
        public bool IsReadOnly => false;
        
        //Metoder
        public void Add(KeyValuePair<K, V> item) => Add(item.Key, item.Value);
        public void Add(K key, V value)
        {
            int bucketIndex = GetBucketIndex(key);
            var bucket = buckets[bucketIndex];
            var comparer = EqualityComparer<K>.Default;

            for (int i = 0; i < bucket.Count; i++)
            {
                if (comparer.Equals(bucket[i].Key, key))
                {
                    throw new ArgumentException("An item with the same key has already been added.", nameof(key));
                }
            }

            bucket.Add(new KeyValuePair<K, V>(key, value));
            count++;
        }

        public void Clear()
        {
            foreach (var bucket in buckets)
            {
                bucket.Clear();
            }

            count = 0;
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            int bucketIndex = GetBucketIndex(item.Key);
            var bucket = buckets[bucketIndex];
            var keyCmp = EqualityComparer<K>.Default;
            var valCmp = EqualityComparer<V>.Default;

            for (int i = 0; i < bucket.Count; i++)
            {
               if (keyCmp.Equals(bucket[i].Key, item.Key) && valCmp.Equals(bucket[i].Value, item.Value))
                {
                    return true;
                } 
            }

            return false;
        }            
           
        public bool ContainsKey(K key)
        {
            int bucketIndex = GetBucketIndex(key);
            var bucket = buckets[bucketIndex];
            var keyCmp = EqualityComparer<K>.Default;
            
            for (int i = 0; i < bucket.Count; i++)
            {
               if (keyCmp.Equals(bucket[i].Key, key))
                {
                    return true;
                } 
            }

            return false;
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < Count) throw new ArgumentException("Not enough space in the target array.");

            int i = arrayIndex;

            foreach (var bucket in buckets)
            {
                for (int j= 0; j < bucket.Count; j++)
                {
                    array[i++] = bucket[j];
                }
            }
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            int bucketIndex = GetBucketIndex(item.Key);
            var bucket = buckets[bucketIndex];
            var keyCmp = EqualityComparer<K>.Default;
            var valCmp = EqualityComparer<V>.Default;

            for (int i = 0; i < bucket.Count; i++)
            {
               if (keyCmp.Equals(bucket[i].Key, item.Key) && valCmp.Equals(bucket[i].Value, item.Value))
                {
                    bucket.RemoveAt(i);
                    count--;
                    return true;
                } 
            }
            return false;
        }

        public bool Remove(K key)
        {
            int bucketIndex = GetBucketIndex(key);
            var bucket = buckets[bucketIndex];

            for(int i = 0; i < bucket.Count; i++)
            {
                if(EqualityComparer<K>.Default.Equals(bucket[i].Key, key))
                {
                    bucket.RemoveAt(i);
                    count--;
                    return true;
                }
            }
            return false;
        }

        public bool TryGetValue(K key, out V value)
        {
            int bucketIndex = GetBucketIndex(key);
            var bucket = buckets[bucketIndex];  
            for(int i = 0; i < bucket.Count; i++)
            {
                if(EqualityComparer<K>.Default.Equals(bucket[i].Key, key))
                {
                    value = bucket[i].Value;
                    return true;
                }
            }
            value = default!;
            return false;
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
           foreach (var bucket in buckets)
            {
                for (int i = 0; i < bucket.Count; i++)
                {
                    yield return bucket[i];
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }       
    }
}