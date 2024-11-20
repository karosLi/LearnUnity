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
    public interface INativeCircularListIndexable<T> where T : struct
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
    public interface INativeCircularList<T> : INativeCircularListIndexable<T> where T : struct
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
    [DebuggerTypeProxy(typeof(NativeCircularListDebugView<>))]
    public unsafe struct NativeCircularList<T> 
        : INativeDisposable
        , INativeCircularList<T>
        , IEnumerable<T> // Used by collection initializers.
        where T : unmanaged
    {
        // Raw pointers aren't usually allowed inside structures that are passed to jobs, but because it's protected
        // with the safety system, you can disable that restriction for it
        [NativeDisableUnsafePtrRestriction]
        internal UnsafeCircularList<T>* m_ListData;
        
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
        public NativeCircularList(Allocator allocator)
            : this(1, allocator)
        {
        }
        
        /// <summary>
        /// Initializes and returns a NativeCircularList.
        /// </summary>
        /// <param name="initialCapacity">The initial capacity of the list.</param>
        /// <param name="allocator">The allocator to use.</param>
        public NativeCircularList(int initialCapacity, Allocator allocator)
        {
            this = default;
            Initialize(initialCapacity, allocator);
        }
        
        internal void Initialize(int initialCapacity, Allocator allocator)
        {
            var totalSize = sizeof(T) * (long)initialCapacity;
            
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            CheckTotalSize(initialCapacity, totalSize);
            
            // Create the AtomicSafetyHandle and DisposeSentinel
            m_Safety = AtomicSafetyHandle.Create();

            // Set the safety ID on the AtomicSafetyHandle so that error messages describe this container type properly.
            AtomicSafetyHandle.SetStaticSafetyId(ref m_Safety, s_staticSafetyId);
        
            // Automatically bump the secondary version any time this container is scheduled for writing in a job
            AtomicSafetyHandle.SetBumpSecondaryVersionOnScheduleWrite(m_Safety, true);
            
#endif
            m_ListData = UnsafeCircularList<T>.Create(initialCapacity, allocator);
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
                // Read from the buffer and return the value
                return (*m_ListData)[index];
            }
            set
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif
                (*m_ListData)[index] = value;
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
            return ref m_ListData->ElementAt(index);
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
                return m_ListData->Length;
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
                return m_ListData->Capacity;
            }

            set
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion(m_Safety);
#endif
                m_ListData->Capacity = value;
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
            m_ListData->Add(value);
        }
        
        /// <summary>
        /// Removes the element at the rear of this list.
        /// </summary>
        /// <remarks>
        /// Length is decremented by 1.
        /// </remarks>
        public void RemoveTail()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion(m_Safety);
#endif
            m_ListData->RemoveTail();
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
            m_ListData->AddHead(value);
        }

        /// <summary>
        /// Removes the element at the front of this list.
        /// </summary>
        /// <remarks>
        /// Length is decremented by 1.
        /// </remarks>
        public void RemoveHead()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion(m_Safety);
#endif
            m_ListData->RemoveHead();
        }
        
        /// <summary>
        /// Whether this list is full.
        /// </summary>
        /// <value>True if this list is empty.</value>
        public bool IsFull => m_ListData->IsFull;
        
        /// <summary>
        /// Whether this list is empty.
        /// </summary>
        /// <value>True if the list is empty or if the list has not been constructed.</value>
        public bool IsEmpty => m_ListData == null || m_ListData->Length == 0;

        /// <summary>
        /// Whether this list has been allocated (and not yet deallocated).
        /// </summary>
        /// <value>True if this list has been allocated (and not yet deallocated).</value>
        public bool IsCreated => m_ListData != null;
        
        /// <summary>
        /// Releases all resources (memory and safety handles).
        /// </summary>
        public void Dispose()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (!AtomicSafetyHandle.IsDefaultValue(m_Safety))
            {
                AtomicSafetyHandle.CheckExistsAndThrow(m_Safety);
            }
#endif
            if (!IsCreated)
            {
                return;
            }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            CollectionHelper.DisposeSafetyHandle(ref m_Safety);
#endif
            
            UnsafeCircularList<T>.Destroy(m_ListData);
            m_ListData = null;
        }
        
        internal void Dispose(Allocator allocator)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (!AtomicSafetyHandle.IsDefaultValue(m_Safety))
            {
                AtomicSafetyHandle.CheckExistsAndThrow(m_Safety);
            }
#endif
            if (!IsCreated)
            {
                return;
            }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            CollectionHelper.DisposeSafetyHandle(ref m_Safety);
#endif
            UnsafeCircularList<T>.Destroy(m_ListData);
            m_ListData = null;
        }
        
        /// <summary>
        /// Creates and schedules a job that releases all resources (memory and safety handles) of this list.
        /// </summary>
        /// <param name="inputDeps">The dependency for the new job.</param>
        /// <returns>The handle of the new job. The job depends upon `inputDeps` and releases all resources (memory and safety handles) of this list.</returns>
        public JobHandle Dispose(JobHandle inputDeps)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (!AtomicSafetyHandle.IsDefaultValue(m_Safety))
            {
                AtomicSafetyHandle.CheckExistsAndThrow(m_Safety);
            }
#endif
            if (!IsCreated)
            {
                return inputDeps;
            }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var jobHandle = new NativeCircularListDisposeJob { Data = new NativeCircularListDispose { m_ListData = m_ListData, m_AllocatorLabel = m_ListData->m_AllocatorLabel, m_Safety = m_Safety } }.Schedule(inputDeps);
            AtomicSafetyHandle.Release(m_Safety);
#else
            var jobHandle = new NativeCircularListDisposeJob { Data = new NativeCircularListDispose { m_ListData = m_ListData, m_AllocatorLabel = m_ListData->m_AllocatorLabel } }.Schedule(inputDeps);
#endif
            m_ListData = null;

            return jobHandle;
        }

        public void Clear()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion(m_Safety);
#endif
            m_ListData->Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_ListData->GetEnumerator();
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
                throw new ArgumentOutOfRangeException(nameof(initialCapacity), $"Capacity * sizeof(T) cannot exceed {int.MaxValue} bytes");
        }
    }
    
    [NativeContainer]
    internal unsafe struct NativeCircularListDispose
    {
        [NativeDisableUnsafePtrRestriction]
        public void* m_ListData;
        public Allocator m_AllocatorLabel;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal AtomicSafetyHandle m_Safety;
#endif

        public void Dispose()
        {
            UnsafeUtility.FreeTracked(m_ListData, m_AllocatorLabel);
        }
    }

    [BurstCompile]
    internal unsafe struct NativeCircularListDisposeJob : IJob
    {
        internal NativeCircularListDispose Data;
        
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal AtomicSafetyHandle m_Safety;
#endif

        public void Execute()
        {
            Data.Dispose();
        }
    }
    
    public sealed class NativeCircularListDebugView<T> where T : unmanaged
    {
        NativeCircularList<T> m_List;

        public NativeCircularListDebugView(NativeCircularList<T> list)
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