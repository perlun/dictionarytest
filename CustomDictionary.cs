using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DictionaryTest
{
    internal struct CustomDictionaryBucket<TKey, TValue>
    {
        internal TKey Key;
        internal TValue Value;
        internal int Hash;
    }

    // Simple dictionary implementation, based on a simple HashTable.
    //
    // Caveats:
    //
    // - Not thread safe.
    // - Quite slow when the bucket has to be resized (since it involves re-populating all the bucket entries with
    //   new hashes)
    public class CustomDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        // Note: the value here MUST be an even power of two, otherwise the algorithm used by this class will fail.
        const int INITIAL_BUCKET_SIZE = 1024;

        private CustomDictionaryBucket<TKey, TValue>?[] BucketList = new CustomDictionaryBucket<TKey, TValue>?[INITIAL_BUCKET_SIZE];

        // 1024 = 0b1000000000 => 0b0111111111
        private int BucketSizeMask = INITIAL_BUCKET_SIZE - 1;

        public ICollection<TKey> Keys => throw new NotImplementedException();

        public ICollection<TValue> Values => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        private void PutEntryInList(int bucketIndex, TKey key, TValue value, int hashCode)
        {
            BucketList[bucketIndex] = new CustomDictionaryBucket<TKey, TValue>
            {
                Key = key,
                Value = value,
                Hash = hashCode
            };
        }

        private void GrowBucketList()
        {
            var oldBucketList = BucketList;
            BucketList = new CustomDictionaryBucket<TKey, TValue>?[oldBucketList.Count() * 2];
            BucketSizeMask = BucketList.Count() - 1;

            // Ensure all the data gets put into the new bucketlist now.
            foreach (var b in oldBucketList)
            {
                if (b == null)
                {
                    continue;
                }

                this[b.Value.Key] = b.Value.Value;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                var hashCode = key.GetHashCode();
                int bucketIndex = hashCode & BucketSizeMask;

                var entry = BucketList[bucketIndex];

                if (entry == null ||
                    entry?.Hash == hashCode)
                {
                    return entry.Value.Value;
                }
                else
                {
                    // Try to find a subsequent slot, since the item might have been moved due to collisions.
                    int size = BucketList.Count();

                    for (int i = bucketIndex; i < size; i++)
                    {
                        if (BucketList[i].Value.Hash == hashCode)
                        {
                            return BucketList[i].Value.Value;
                        }
                    }

                    // The item does not exist in this dictionary.
                    return default(TValue);
                }
            }

            set
            {
                var hashCode = key.GetHashCode();
                int bucketIndex = hashCode & BucketSizeMask;

                var oldEntry = BucketList[bucketIndex];
                if (oldEntry == null ||
                    oldEntry?.Hash == hashCode)
                {
                    PutEntryInList(bucketIndex, key, value, hashCode);
                }
                else
                {
                    // Try to find another slot, as close as possible (to ensure lookups are reasonably fast.)
                    int size = BucketList.Count();
                    int i = bucketIndex;

                    while (i < size)
                    {
                        i++;

                        if (i == size)
                        {
                            // The bucket list is full; it's time to grow it.
                            GrowBucketList();

                            // Call ourselves recursively, to actually do the insertion.
                            this[key] = value;
                        }

                        if (BucketList[i] == null)
                        {
                            PutEntryInList(i, key, value, hashCode);
                            break;
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            int count = 0;

            foreach (var b in BucketList)
            {
                if (b == null)
                {
                    continue;
                }

                sb.AppendLine($"{b.Value.Key} => {b.Value.Value}");
                count++;
            }

            sb.AppendLine($"Number of entries in dictionary: {count}");

            return sb.ToString();
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
