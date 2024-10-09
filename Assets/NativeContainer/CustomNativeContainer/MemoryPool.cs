using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace NativeContainer
{
    public unsafe struct MemoryPool<T> : IDisposable where T : struct
    {
        private NativeList<IntPtr> freeBlocks;
        private Allocator allocator;

        public MemoryPool(int initialCapacity, Allocator allocator)
        {
            this.allocator = allocator;
            freeBlocks = new NativeList<IntPtr>(initialCapacity, allocator);

            for (int i = 0; i < initialCapacity; i++)
            {
                IntPtr newBlock = new IntPtr(UnsafeUtility.Malloc(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), allocator));
                freeBlocks.Add(newBlock);
            }
        }

        public IntPtr Get()
        {
            if (freeBlocks.Length == 0)
            {
                ExpandPool(10);
            }

            int lastIndex = freeBlocks.Length - 1;
            IntPtr block = freeBlocks[lastIndex];
            freeBlocks.RemoveAtSwapBack(lastIndex);
            return block;
        }

        public void Release(IntPtr block)
        {
            freeBlocks.Add(block);
        }

        private void ExpandPool(int additionalCapacity)
        {
            for (int i = 0; i < additionalCapacity; i++)
            {
                IntPtr newBlock = new IntPtr(UnsafeUtility.Malloc(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), allocator));
                freeBlocks.Add(newBlock);
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < freeBlocks.Length; i++)
            {
                UnsafeUtility.Free(freeBlocks[i].ToPointer(), allocator);
            }
            freeBlocks.Dispose();
        }
    }
}