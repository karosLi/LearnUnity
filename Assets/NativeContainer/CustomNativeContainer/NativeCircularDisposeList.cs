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
using UnityEngine.Internal;

namespace NativeContainer
{
    /// <summary>
    /// An indexable collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public interface INativeCircularDisposeListIndexable<T> where T : struct,IDisposable
    {
        /// <summary>
        /// The current number of elements in the collection.
        /// </summary>
        /// <value>The current number of elements in the collection.</value>
        int Length { get; }

        /// <summary>
        /// Returns a reference to the element at a given index.
        /// </summary>
        /// <param name="index">The index to access. Must be in the range of [0..Length).</param>
        /// <returns>A reference to the element at the index.</returns>
        ref T ElementAt(int index);
    }

    /// <summary>
    /// A resizable list.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public interface INativeCircularDisposeList<T> : INativeCircularListIndexable<T> where T : struct,IDisposable
    {
        /// <summary>
        /// The number of elements that fit in the current allocation.
        /// </summary>
        /// <value>The number of elements that fit in the current allocation.</value>
        /// <param name="value">A new capacity.</param>
        int Capacity { get; set; }
        
        /// <summary>
        /// Whether this list is full.
        /// </summary>
        /// <value>True if this list is empty.</value>
        bool IsFull { get; }

        /// <summary>
        /// Whether this list is empty.
        /// </summary>
        /// <value>True if this list is empty.</value>
        bool IsEmpty { get; }

        /// <summary>
        /// The element at an index.
        /// </summary>
        /// <param name="index">An index.</param>
        /// <value>The element at the index.</value>
        /// <exception cref="IndexOutOfRangeException">Thrown if index is out of bounds.</exception>
        T this[int index] { get; set; }

        /// <summary>
        /// Sets the length to 0.
        /// </summary>
        /// <remarks>Does not change the capacity.</remarks>
        void Clear();
    }
    
    /// <summary>
    /// An unmanaged, resizable circular list.
    /// </summary>
    /// <remarks>The elements are stored contiguously in a buffer rather than as linked nodes.</remarks>
    /// <typeparam name="T">The type of the elements.</typeparam>
    [StructLayout(LayoutKind.Sequential)]
    [NativeContainer]
    [DebuggerDisplay("Length = {Length}")]
    [DebuggerTypeProxy(typeof(NativeCircularDisposeListDebugView<>))]
    public unsafe struct NativeCircularDisposeList<T> 
        : INativeDisposable
        , INativeCircularList<T>
        , IEnumerable<T> // Used by collection initializers.
        where T : unmanaged,IDisposable
    {
        // Raw pointers aren't usually allowed inside structures that are passed to jobs, but because it's protected
        // with the safety system, you can disable that restriction for it
        [NativeDisableUnsafePtrRestriction]
        internal void* m_Buffer;
        internal Allocator m_AllocatorLabel;
        internal NativeReference<int> m_Head;
        internal NativeReference<int> m_Tail;
        internal NativeReference<int> m_Length;
        internal NativeReference<int> m_Capacity;
        
        // You should only declare and use safety system members with the ENABLE_UNITY_COLLECTIONS_CHECKS define.
        // In final builds of projects, the safety system is disabled for performance reasons, so these APIs aren't
        // available in those builds.
#if ENABLE_UNITY_COLLECTIONS_CHECKS
    
        // The AtomicSafetyHandle field must be named exactly 'm_Safety'.
        internal AtomicSafetyHandle m_Safety;
    
        // Statically register this type with the safety system, using a name derived from the type itself
        internal static readonly int s_staticSafetyId = AtomicSafetyHandle.NewStaticSafetyId<NativeCircularList<T>>();
#endif
        
        /// <summary>
        /// Initializes and returns a NativeCircularList with a capacity of one.
        /// </summary>
        /// <param name="allocator">The allocator to use.</param>
        public NativeCircularDisposeList(Allocator allocator)
            : this(1, allocator)
        {
        }
        
        /// <summary>
        /// Initializes and returns a NativeCircularList.
        /// </summary>
        /// <param name="initialCapacity">The initial capacity of the list.</param>
        /// <param name="allocator">The allocator to use.</param>
        public NativeCircularDisposeList(int initialCapacity, Allocator allocator)
        {
            this = default;
            Initialize(initialCapacity, allocator);
        }
        
        internal void Initialize(int initialCapacity, Allocator allocator)
        {
            m_Head = new NativeReference<int>(allocator);
            m_Tail = new NativeReference<int>(allocator);
            m_Length = new NativeReference<int>(allocator);
            m_Capacity = new NativeReference<int>(math.max(initialCapacity, 1), allocator);
            m_AllocatorLabel = allocator;
            
            var totalSize = sizeof(T) * (long)m_Capacity.Value;
            m_Buffer = UnsafeUtility.MallocTracked(totalSize, UnsafeUtility.AlignOf<T>(), m_AllocatorLabel, 1);
            
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            CheckTotalSize(m_Capacity.Value, totalSize);
            
            // Create the AtomicSafetyHandle and DisposeSentinel
            m_Safety = AtomicSafetyHandle.Create();

            // Set the safety ID on the AtomicSafetyHandle so that error messages describe this container type properly.
            AtomicSafetyHandle.SetStaticSafetyId(ref m_Safety, s_staticSafetyId);
        
            // Automatically bump the secondary version any time this container is scheduled for writing in a job
            AtomicSafetyHandle.SetBumpSecondaryVersionOnScheduleWrite(m_Safety, true);

            // Check if this is a nested container, and if so, set the nested container flag
            if (UnsafeUtility.IsNativeContainerType<T>()) 
                AtomicSafetyHandle.SetNestedContainer(m_Safety, true);
#endif
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
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
#endif
                CheckIndexInRange(index, Length);
                // Read from the buffer and return the value
                return UnsafeUtility.ReadArrayElement<T>(m_Buffer, (m_Head.Value + index) % m_Capacity.Value);
            }
            set
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif
                CheckIndexInRange(index, Length);
                // Write the value into the buffer
                UnsafeUtility.WriteArrayElement(m_Buffer, (m_Head.Value + index) % m_Capacity.Value, value);
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
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif
            CheckIndexInRange(index, Length);
            return ref UnsafeUtility.ArrayElementAsRef<T>(m_Buffer, (m_Head.Value + index) % m_Capacity.Value);
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
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
#endif
                return m_Length.Value;
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
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
#endif
                return m_Capacity.Value;
            }

            set
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion(m_Safety);
#endif
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
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion(m_Safety);
#endif
            CheckCapacity();
            UnsafeUtility.WriteArrayElement(m_Buffer, m_Tail.Value, value);
            m_Tail.Value = (m_Tail.Value + 1) % m_Capacity.Value;
            m_Length.Value += 1;
        }
        
        /// <summary>
        /// Removes the element at the rear of this list.
        /// </summary>
        /// <remarks>
        /// Length is decremented by 1.
        /// </remarks>
        public void RemoveTail(bool dispose = true)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion(m_Safety);
#endif
            if (m_Length.Value == 0)
            {
                return;
            }

            if (dispose)
            {
                this[m_Tail.Value].Dispose(); 
            }
            m_Tail.Value = (m_Tail.Value + m_Capacity.Value - 1) % m_Capacity.Value;
            m_Length.Value -= 1;
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
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion(m_Safety);
#endif
            CheckCapacity();
            // Calculate new head position
            m_Head.Value = (m_Head.Value - 1 + m_Capacity.Value) % m_Capacity.Value;
            UnsafeUtility.WriteArrayElement(m_Buffer, m_Head.Value, value);
            m_Length.Value += 1;
        }

        /// <summary>
        /// Removes the element at the front of this list.
        /// </summary>
        /// <remarks>
        /// Length is decremented by 1.
        /// </remarks>
        public void RemoveHead(bool dispose = true)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion(m_Safety);
#endif
            if (m_Length.Value == 0)
            {
                return;
            }

            if (dispose)
            {
                this[m_Head.Value].Dispose();
            }
            
            m_Head.Value = (m_Head.Value + 1) % m_Capacity.Value;
            m_Length.Value -= 1;
        }
        
        /// <summary>
        /// Handles capacity increase by expanding the internal array and rearranging elements.
        /// </summary>
        private void CheckCapacity()
        {
            if (m_Length.Value == m_Capacity.Value)
            {
                Realloc(m_Capacity.Value * 2);
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
        public bool IsEmpty => !IsCreated || m_Length.Value == 0;

        /// <summary>
        /// Whether this list has been allocated (and not yet deallocated).
        /// </summary>
        /// <value>True if this list has been allocated (and not yet deallocated).</value>
        public bool IsCreated => m_Buffer != null;
        
        /// <summary>
        /// Releases all resources (memory and safety handles).
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < m_Length.Value; i++)
            {
                this[i].Dispose();
            }
            
            m_Head.Dispose();
            m_Tail.Dispose();
            m_Length.Dispose();
            m_Capacity.Dispose();
            
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckDeallocateAndThrow(m_Safety);
            AtomicSafetyHandle.Release(m_Safety);
#endif
            // Free the buffer
            UnsafeUtility.FreeTracked(m_Buffer, m_AllocatorLabel);
            m_Buffer = null;
        }
        
        /// <summary>
        /// Creates and schedules a job that releases all resources (memory and safety handles) of this list.
        /// </summary>
        /// <param name="inputDeps">The dependency for the new job.</param>
        /// <returns>The handle of the new job. The job depends upon `inputDeps` and releases all resources (memory and safety handles) of this list.</returns>
        public JobHandle Dispose(JobHandle inputDeps)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckDeallocateAndThrow(m_Safety);
            var jobHandle = new NativeCircularDisposeListDisposeJob { Data = new NativeCircularDisposeListDispose { m_Buffer = m_Buffer, m_AllocatorLabel = m_AllocatorLabel, m_Safety = m_Safety } }.Schedule(inputDeps);
            AtomicSafetyHandle.Release(m_Safety);
#else
            var jobHandle = new NativeCircularListDisposeJob { Data = new NativeCircularListDispose { m_Buffer = m_Buffer, m_AllocatorLabel = m_AllocatorLabel } }.Schedule(inputDeps);
#endif
            m_Buffer = null;
            m_Head.Dispose();
            m_Tail.Dispose();
            m_Length.Dispose();
            m_Capacity.Dispose();
            return jobHandle;
        }

        public void Clear()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion(m_Safety);
#endif
            
            for (int i = 0; i < m_Length.Value; i++)
            {
                this[i].Dispose();
            }
            
            m_Head.Value = 0;
            m_Tail.Value = 0;
            m_Length.Value = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(ref this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
            if (newCapacity > 0 && newCapacity > m_Capacity.Value)
            {
                var totalSize = sizeof(T) * (long)newCapacity;
                void* newBuffer = UnsafeUtility.MallocTracked(totalSize, UnsafeUtility.AlignOf<T>(), m_AllocatorLabel, 1);
                void* sourcePtr = m_Buffer;
                void* destPtr = newBuffer;

                // 如果数组没有回绕
                if (m_Head.Value < m_Tail.Value)
                {
                    int sizeInBytes = UnsafeUtility.SizeOf<T>() * m_Length.Value;
                    UnsafeUtility.MemCpy((byte*)destPtr, (byte*)sourcePtr + m_Head.Value * UnsafeUtility.SizeOf<T>(), sizeInBytes);
                }
                else
                {
                    // 复制head到数组末尾的部分
                    int firstPartSize = (m_Capacity.Value - m_Head.Value) * UnsafeUtility.SizeOf<T>();
                    UnsafeUtility.MemCpy((byte*)destPtr, (byte*)sourcePtr + m_Head.Value * UnsafeUtility.SizeOf<T>(), firstPartSize);
                    // 复制从0到tail的部分
                    UnsafeUtility.MemCpy((byte*)destPtr + firstPartSize, (byte*)sourcePtr, m_Tail.Value * UnsafeUtility.SizeOf<T>());
                }
                
                UnsafeUtility.FreeTracked(sourcePtr, m_AllocatorLabel);
                m_Buffer = newBuffer;
                m_Capacity.Value = newCapacity;
                m_Head.Value = 0;
                m_Tail.Value = m_Length.Value;
            }
        }
        
        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        static void CheckTotalSize(int initialCapacity, long totalSize)
        {
            // Make sure we cannot allocate more than int.MaxValue (2,147,483,647 bytes)
            // because the underlying UnsafeUtility.Malloc is expecting a int.
            // TODO: change UnsafeUtility.Malloc to accept a UIntPtr length instead to match C++ API
            if (totalSize > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(initialCapacity), $"Capacity * sizeof(T) cannot exceed {int.MaxValue} bytes");
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

        [ExcludeFromDocs]
        public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            private NativeCircularDisposeList<T> m_list;
            private int m_Head;
            private int m_Tail;
            private int m_Index;
            private T value;

            public Enumerator(ref NativeCircularDisposeList<T> list)
            {
                this.m_list = list;
                this.m_Index = -1;
                this.m_Head = list.m_Head.Value;
                this.m_Tail = list.m_Tail.Value;
                this.value = default (T);
            }

            public void Dispose()
            {
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe bool MoveNext()
            {
                ++this.m_Index;
                if (this.m_Index < this.m_list.m_Length.Value)
                {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    AtomicSafetyHandle.CheckReadAndThrow(this.m_list.m_Safety);
#endif
                    int index = (m_Head + m_Index) % m_list.m_Capacity.Value;
                    this.value = UnsafeUtility.ReadArrayElement<T>(this.m_list.m_Buffer, index);
                    return true;
                }
                this.value = default (T);
                return false;
            }

            public void Reset() => this.m_Index = -1;

            public T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.value;
            }

            object IEnumerator.Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (object) this.Current;
            }
        }
    }
    
    [NativeContainer]
    internal unsafe struct NativeCircularDisposeListDispose
    {
        [NativeDisableUnsafePtrRestriction]
        public void* m_Buffer;
        public Allocator m_AllocatorLabel;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal AtomicSafetyHandle m_Safety;
#endif

        public void Dispose()
        {
            UnsafeUtility.FreeTracked(m_Buffer, m_AllocatorLabel);
        }
    }

    [BurstCompile]
    internal unsafe struct NativeCircularDisposeListDisposeJob : IJob
    {
        internal NativeCircularDisposeListDispose Data;

        public void Execute()
        {
            Data.Dispose();
        }
    }
    
    public sealed class NativeCircularDisposeListDebugView<T> where T : unmanaged,IDisposable
    {
        NativeCircularDisposeList<T> m_List;

        public NativeCircularDisposeListDebugView(NativeCircularDisposeList<T> list)
        {
            m_List = list;
        }

        public T[] Items
        {
            get
            {
                var array = new T[m_List.Length];
                for (int i = 0; i < m_List.Length; i++)
                {
                    array[i] = m_List[i];
                }
                return array;
            }
        }
    }
}