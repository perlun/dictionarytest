using System;
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
    public class CustomDictionary<TKey, TValue>
    {
        // Note: the value here MUST be an even power of two, otherwise the algorithm used by this class will fail.
        const int INITIAL_BUCKET_SIZE = 1024;

        private CustomDictionaryBucket<TKey, TValue>?[] BucketList = new CustomDictionaryBucket<TKey, TValue>?[INITIAL_BUCKET_SIZE];

        // 1024 = 0b1000000000 => 0b0111111111
        private int BucketSizeMask = INITIAL_BUCKET_SIZE - 1;

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
                throw new NotImplementedException();
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
    }
}
