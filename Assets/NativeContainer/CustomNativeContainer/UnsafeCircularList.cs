using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace NativeContainer
{
    [BurstCompile]
    internal unsafe struct UnsafeCircularDisposeJob : IJob
    {
        [NativeDisableUnsafePtrRestriction]
        public void* Ptr;
        public Allocator Allocator;

        public void Execute()
        {
            UnsafeUtility.FreeTracked(Ptr, Allocator);
        }
    }
    
    /// <summary>
    /// An unmanaged, resizable list.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    [DebuggerDisplay("Length = {Length}, Capacity = {Capacity}, IsCreated = {IsCreated}, IsEmpty = {IsEmpty}")]
    [DebuggerTypeProxy(typeof(UnsafeCircularListTDebugView<>))]
    [GenerateTestsForBurstCompatibility(GenericTypeArguments = new[] { typeof(int) })]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct UnsafeCircularList<T>: INativeDisposable
        , INativeList<T>
        , IEnumerable<T> // Used by collection initializers.
        where T : unmanaged
    {
        // Raw pointers aren't usually allowed inside structures that are passed to jobs, but because it's protected
        // with the safety system, you can disable that restriction for it
        [NativeDisableUnsafePtrRestriction] 
        public T* Ptr;
        public Allocator m_AllocatorLabel;
        public int m_Head;
        public int m_Tail;
        public int m_Length;
        public int m_Capacity;


        /// <summary>
        /// Initializes and returns a NativeCircularList with a capacity of one.
        /// </summary>
        /// <param name="allocator">The allocator to use.</param>
        public UnsafeCircularList(Allocator allocator)
            : this(1, allocator)
        {
        }

        /// <summary>
        /// Initializes and returns a NativeCircularList.
        /// </summary>
        /// <param name="initialCapacity">The initial capacity of the list.</param>
        /// <param name="allocator">The allocator to use.</param>
        public UnsafeCircularList(int initialCapacity, Allocator allocator)
        {
            this = default;
            Initialize(initialCapacity, allocator);
        }

        internal void Initialize(int initialCapacity, Allocator allocator)
        {
            Ptr = null;
            m_Head = 0;
            m_Tail = 0;
            m_Length = 0;
            m_Capacity = math.max(initialCapacity, 1);
            m_AllocatorLabel = allocator;

            var totalSize = sizeof(T) * (long)m_Capacity;
            Ptr = (T*)UnsafeUtility.MallocTracked(totalSize, UnsafeUtility.AlignOf<T>(), m_AllocatorLabel, 1);
        }
        
        public static UnsafeCircularList<T>* Create(int initialCapacity, Allocator allocator)
        {
            UnsafeCircularList<T>* listData = (UnsafeCircularList<T>*)UnsafeUtility.MallocTracked(UnsafeUtility.SizeOf<UnsafeCircularList<T>>(), UnsafeUtility.AlignOf<UnsafeCircularList<T>>(), allocator, 1);
            *listData = new UnsafeCircularList<T>(initialCapacity, allocator);
            return listData;
        }
        
        public static void Destroy(UnsafeCircularList<T>* listData, Allocator allocator)
        {
            listData->Dispose(allocator);
            UnsafeUtility.FreeTracked(listData, allocator);
        }
        
        public static void Destroy(UnsafeCircularList<T>* listData)
        {
            Destroy(listData, listData->m_AllocatorLabel);
        }

        /// <summary>
        /// The element at a given index.
        /// </summary>
        /// <param name="index">An index into this list.</param>
        /// <value>The value to store at the `index`.</value>
        /// <exception cref="IndexOutOfRangeException">Thrown if `index` is out of bounds.</exception>
        public T this[int index]
        {
            get
            {
                CheckIndexInRange(index, Length);
                // Read from the buffer and return the value
                return UnsafeUtility.ReadArrayElement<T>(Ptr, (m_Head + index) % m_Capacity);
            }
            set
            {
                CheckIndexInRange(index, Length);
                // Write the value into the buffer
                UnsafeUtility.WriteArrayElement(Ptr, (m_Head + index) % m_Capacity, value);
            }
        }

        /// <summary>
        /// Returns a reference to the element at an index.
        /// </summary>
        /// <param name="index">An index.</param>
        /// <returns>A reference to the element at the index.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if index is out of bounds.</exception>
        public ref T ElementAt(int index)
        {
            CheckIndexInRange(index, Length);
            return ref UnsafeUtility.ArrayElementAsRef<T>(Ptr, (m_Head + index) % m_Capacity);
        }

        /// <summary>
        /// The count of elements.
        /// </summary>
        /// <value>The current count of elements. Always less than or equal to the capacity.</value>
        /// <remarks>To decrease the memory used by a list, set <see cref="Capacity"/> after reducing the length of the list.</remarks>
        public int Length
        {
            get
            {
                return m_Length;
            }
            set
            {
                // 不提供直接修改长度
            }
        }

        /// <summary>
        /// The number of elements that fit in the current allocation.
        /// </summary>
        /// <value>The number of elements that fit in the current allocation.</value>
        /// <param name="value">The new capacity. Must be greater or equal to the length.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the new capacity is smaller than the length.</exception>
        public int Capacity
        {
            get
            {
                return m_Capacity;
            }

            set
            {
                SetCapacity(value);
            }
        }

        /// <summary>
        /// Adds an element to the end of this list.
        /// </summary>
        /// <param name="value">The value to add to the end of this list.</param>
        /// <remarks>
        /// Length is incremented by 1. If necessary, the capacity is increased.
        /// </remarks>
        public void Add(in T value)
        {
            CheckCapacity();
            UnsafeUtility.WriteArrayElement(Ptr, m_Tail, value);
            m_Tail = (m_Tail + 1) % m_Capacity;
            m_Length += 1;
        }

        /// <summary>
        /// Removes the element at the rear of this list.
        /// </summary>
        /// <remarks>
        /// Length is decremented by 1.
        /// </remarks>
        public void RemoveTail()
        {
            if (m_Length == 0)
            {
                return;
            }

            m_Tail = (m_Tail + m_Capacity - 1) % m_Capacity;
            m_Length -= 1;
        }

        /// <summary>
        /// Adds an element to the beginning of this list.
        /// </summary>
        /// <param name="value">The value to add to the beginning of this list.</param>
        /// <remarks>
        /// Length is incremented by 1. If necessary, the capacity is increased to accommodate new elements.
        /// </remarks>
        public void AddHead(in T value)
        {
            CheckCapacity();
            // Calculate new head position
            m_Head = (m_Head - 1 + m_Capacity) % m_Capacity;
            UnsafeUtility.WriteArrayElement(Ptr, m_Head, value);
            m_Length += 1;
        }

        /// <summary>
        /// Removes the element at the front of this list.
        /// </summary>
        /// <remarks>
        /// Length is decremented by 1.
        /// </remarks>
        public void RemoveHead()
        {
            if (m_Length == 0)
            {
                return;
            }

            m_Head = (m_Head + 1) % m_Capacity;
            m_Length -= 1;
        }

        /// <summary>
        /// Handles capacity increase by expanding the internal array and rearranging elements.
        /// </summary>
        private void CheckCapacity()
        {
            if (m_Length == m_Capacity)
            {
                Realloc(m_Capacity * 2);
            }
        }

        /// <summary>
        /// Whether this list is full.
        /// </summary>
        /// <value>True if this list is empty.</value>
        public bool IsFull => m_Length == m_Capacity;

        /// <summary>
        /// Whether this list is empty.
        /// </summary>
        /// <value>True if the list is empty or if the list has not been constructed.</value>
        public bool IsEmpty => !IsCreated || m_Length == 0;

        /// <summary>
        /// Whether this list has been allocated (and not yet deallocated).
        /// </summary>
        /// <value>True if this list has been allocated (and not yet deallocated).</value>
        public bool IsCreated => Ptr != null;

        /// <summary>
        /// Releases all resources (memory and safety handles).
        /// </summary>
        public void Dispose()
        {
            if (!IsCreated)
            {
                return;
            }

            if (m_AllocatorLabel > Allocator.None)
            {
                // Free the buffer
                UnsafeUtility.FreeTracked(Ptr, m_AllocatorLabel);
                m_AllocatorLabel = Allocator.Invalid;
            }

            Ptr = null;
            m_Length = 0;
            m_Capacity = 0;
        }
        
        private void Dispose(Allocator allocator)
        {
            UnsafeUtility.FreeTracked(Ptr, allocator);
            
            Ptr = null;
            m_Length = 0;
            m_Capacity = 0;
        }
        
        /// <summary>
        /// Creates and schedules a job that releases all resources (memory and safety handles) of this list.
        /// </summary>
        /// <param name="inputDeps">The dependency for the new job.</param>
        /// <returns>The handle of the new job. The job depends upon `inputDeps` and releases all resources (memory and safety handles) of this list.</returns>
        public JobHandle Dispose(JobHandle inputDeps)
        {
            if (!IsCreated)
            {
                return inputDeps;
            }

            if (m_AllocatorLabel > Allocator.None)
            {
                var jobHandle = new UnsafeCircularDisposeJob { Ptr = Ptr, Allocator = m_AllocatorLabel }.Schedule(inputDeps);

                Ptr = null;
                m_AllocatorLabel = Allocator.Invalid;

                return jobHandle;
            }

            Ptr = null;
            return inputDeps;
        }

        public void Clear()
        {
            m_Head = 0;
            m_Tail = 0;
            m_Length = 0;
        }

        /// <summary>
        /// Sets the capacity.
        /// </summary>
        /// <param name="capacity">The new capacity.</param>
        void SetCapacity(int capacity)
        {
            var sizeOf = sizeof(T);
            var newCapacity = math.max(capacity, 64 / sizeOf);
            newCapacity = math.ceilpow2(newCapacity);

            Realloc(newCapacity);
        }

        void Realloc(int newCapacity)
        {
            if (newCapacity > 0 && newCapacity > m_Capacity)
            {
                var totalSize = sizeof(T) * (long)newCapacity;
                T* newBuffer =
                    (T*)UnsafeUtility.MallocTracked(totalSize, UnsafeUtility.AlignOf<T>(), m_AllocatorLabel, 1);
                T* sourcePtr = Ptr;
                T* destPtr = newBuffer;

                // 如果数组没有回绕
                if (m_Head < m_Tail)
                {
                    int sizeInBytes = UnsafeUtility.SizeOf<T>() * m_Length;
                    UnsafeUtility.MemCpy((byte*)destPtr, (byte*)sourcePtr + m_Head * UnsafeUtility.SizeOf<T>(),
                        sizeInBytes);
                }
                else
                {
                    // 复制head到数组末尾的部分
                    int firstPartSize = (m_Capacity - m_Head) * UnsafeUtility.SizeOf<T>();
                    UnsafeUtility.MemCpy((byte*)destPtr, (byte*)sourcePtr + m_Head * UnsafeUtility.SizeOf<T>(),
                        firstPartSize);
                    // 复制从0到tail的部分
                    UnsafeUtility.MemCpy((byte*)destPtr + firstPartSize, (byte*)sourcePtr,
                        m_Tail * UnsafeUtility.SizeOf<T>());
                }

                UnsafeUtility.FreeTracked(sourcePtr, m_AllocatorLabel);
                Ptr = newBuffer;
                m_Capacity = newCapacity;
                m_Head = 0;
                m_Tail = m_Length;
            }
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(ref this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        static void CheckTotalSize(int initialCapacity, long totalSize)
        {
            // Make sure we cannot allocate more than int.MaxValue (2,147,483,647 bytes)
            // because the underlying UnsafeUtility.Malloc is expecting a int.
            // TODO: change UnsafeUtility.Malloc to accept a UIntPtr length instead to match C++ API
            if (totalSize > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(initialCapacity),
                    $"Capacity * sizeof(T) cannot exceed {int.MaxValue} bytes");
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        static void CheckIndexInRange(int value, int length)
        {
            if (value < 0)
                throw new IndexOutOfRangeException($"Value {value} must be positive.");

            if ((uint)value >= (uint)length)
                throw new IndexOutOfRangeException(
                    $"Value {value} is out of range in NativeCircularList of '{length}' Length.");
        }
        
        public struct Enumerator : IEnumerator<T>
        {
            internal T* m_Ptr;
            private int m_Head;
            private int m_Tail;
            private int m_Length;
            private int m_Capacity;
            private int m_Index;
            private T value;
            
            public Enumerator(ref UnsafeCircularList<T> list)
            {
                this.m_Ptr = list.Ptr;
                this.m_Index = -1;
                this.m_Head = list.m_Head;
                this.m_Tail = list.m_Tail;
                this.m_Length = list.m_Length;
                this.m_Capacity = list.m_Capacity;
                this.value = default (T);
            }
            
            public void Dispose() { }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                ++this.m_Index;
                if (this.m_Index < this.m_Length)
                {
                    int index = (m_Head + m_Index) % m_Capacity;
                    this.value = UnsafeUtility.ReadArrayElement<T>(m_Ptr, index);
                    return true;
                }
                this.value = default (T);
                return false;
            }
            
            public void Reset() => m_Index = -1;
            
            public T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => this.value;
            }

            object IEnumerator.Current => Current;
        }

    }
    
    internal sealed class UnsafeCircularListTDebugView<T>
        where T : unmanaged
    {
        UnsafeCircularList<T> Data;

        public UnsafeCircularListTDebugView(UnsafeCircularList<T> data)
        {
            Data = data;
        }

        public unsafe T[] Items
        {
            get
            {
                T[] result = new T[Data.Length];

                for (var i = 0; i < result.Length; ++i)
                {
                    result[i] = Data.Ptr[i];
                }

                return result;
            }
        }
    }
}