using System;
using Unity.Collections;

namespace NativeContainer
{
    public static class NativeContainerExtension
    {
        // 移除某个值
        public static void RemoveValue<TKey, TValue>(this NativeParallelMultiHashMap<TKey, TValue> hashMap, TKey key, TValue value) where TKey : unmanaged, IEquatable<TKey>
            where TValue : unmanaged, IEquatable<TValue>
        {
            if (hashMap.ContainsKey(key))
            {
                NativeParallelMultiHashMapIterator<TKey> it;
                TValue item;
                if (hashMap.TryGetFirstValue(key, out item, out it))
                {
                    do
                    {
                        if (item.Equals(value))
                        {
                            hashMap.Remove(it);
                        }
                    }
                    while (hashMap.TryGetNextValue(out item, ref it));
                }
            }
        }
    }
}