using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using JacksonDunstan.NativeCollections;
using NativeContainer;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

public struct MoveComponent : IComponentData
{
}

public struct Data
{
    public int A;
}

public unsafe class TestNatvieContainer : MonoBehaviour
{
    public NativeHashMap<int, IntPtr> HashMap;
    
    // Start is called before the first frame update
    void Start()
    {
        TestMultipleHashMap();
        return;
        
        // TestCircularList();
        // return;
        
        HashMap = new NativeHashMap<int, IntPtr>(0, Allocator.Persistent);
        Data data = new Data()
        {
            A = 10
        };
        Data data1 = new Data()
        {
            A = 20
        };
        
        ref Data structRef = ref data;
        AddValue(ref HashMap, 1, ref data);
        AddValue(ref HashMap, 1, ref data1);
        AddValue(ref HashMap, 2, ref data);
        AddValue(ref HashMap, 2, ref data1);

        // UnsafePtrList
        // NativeHashMap<int, IntPtr>* pointer = (NativeHashMap<int, IntPtr>*)UnsafeUtility.AddressOf(ref HashMap);
        // ref var HashMap1 = ref pointer;
        
        // NativeList<Data> list = new NativeList<Data>(Allocator.Persistent);
        // ref var a = ref list.ElementAt(0);

        foreach (var pair in HashMap)
        {
            NativeHashSet<IntPtr> set = UnsafeUtility.AsRef<NativeHashSet<IntPtr>>(pair.Value.ToPointer());
            foreach (var intPtr in set)
            {
                Data _data = UnsafeUtility.AsRef<Data>(intPtr.ToPointer());
                Debug.Log($"_data {_data.A}");
            }
            // 销毁 NativeHashSet
            if (set.IsCreated)
            {
                set.Dispose();
            }
            // 清理非托管内存
            Marshal.DestroyStructure<NativeHashSet<IntPtr>>(pair.Value);
            Marshal.FreeHGlobal(pair.Value);
            // set.Dispose();
        }
        HashMap.Dispose();

        // IntPtr pointer = new IntPtr(UnsafeUtility.AddressOf(ref data));
        // // UnsafeUtility.AsRef<Data>(pointer).A = 20;
        // Data* pointer1 = (Data*)pointer.ToPointer();
        // pointer1->A = 20;
        // Debug.Log($"data.A {data.A} pointer1->A {pointer1->A}");
        //
        // NativeList<Data> list = new NativeList<Data>(Allocator.Persistent);
        // IntPtr listPointer = new IntPtr(UnsafeUtility.AddressOf(ref list));
        // NativeList<Data> list1 = UnsafeUtility.AsRef<NativeList<Data>>(listPointer.ToPointer());
        // list1.Add(new Data()
        // {
        //     A = 20
        // });
        // Debug.Log($"list.data.A {list[0].A} list1.data->A {list1[0].A}");
        // list.Dispose();

        //
        // NativeCircularList<int> list = new NativeCircularList<int>(1, Allocator.Persistent);
        // for (int i = 0; i < 20; i++)
        // {
        //     list.Add(i);
        // }

        // list.AddHead(8);
        // list.AddHead(7);
        // list.AddHead(6);
        // list.AddHead(5);
        // list.RemoveHead();
        // list.RemoveTail();

        // StringBuilder sb = new StringBuilder();
        // foreach (var a in list)
        // {
        //     sb.Append($"{a} ");
        // }
        // Debug.Log($"==== {sb.ToString()}");
        //
        // for (int i = 0; i < 10; i++)
        // {
        //     sb.Clear();
        //     var tail = list[list.Length - 1];
        //     list.RemoveTail();
        //     var head = list[0];
        //     var tail1 = list[list.Length - 1];
        //     list.AddHead(tail);
        //     
        //     foreach (var a in list)
        //     {
        //         sb.Append($"{a} ");
        //     }
        //     Debug.Log($"==== {sb.ToString()}");
        // }
        // NativeHashMap<int, IntPtr> hashMap = new NativeHashMap<int, IntPtr>(0, Allocator.TempJob);
        // hashMap.Add(1, new IntPtr(UnsafeUtility.AddressOf(ref list)));
        //
        // TestNativeContainerJob job = new TestNativeContainerJob()
        // {
        //     HashMap = hashMap
        // };
        // var handle = job.Schedule();
        // handle.Complete();
        //
        // foreach (var a in list)
        // {
        //     Debug.Log($"==== {a}");
        // }

        // hashMap.Dispose();
        // list.Dispose();
    }

    private List<NativeHashSet<IntPtr>> list = new List<NativeHashSet<IntPtr>>();// 让结构体分配在堆上
    public void AddValue(ref NativeHashMap<int, IntPtr> hashMap, int key, ref Data data)
    {
        if (!hashMap.ContainsKey(key))
        {
            IntPtr dataPtr = new IntPtr(UnsafeUtility.AddressOf(ref data));
            NativeHashSet<IntPtr> set = new NativeHashSet<IntPtr>(0, Allocator.Persistent);
            // UnityAction action = () =>// 闭包的目的是让结构体分配在堆上
            // {
            //     ref NativeHashSet<IntPtr> refSet = ref set;
            // };
            // action();
            // set.Add(dataPtr);
            // hashMap.Add(key, new IntPtr(UnsafeUtility.AddressOf(ref set)));
            
            set.Add(dataPtr);
            // GCHandle handle = GCHandle.Alloc(set, GCHandleType.Pinned);
            // hashMap.Add(key, handle.AddrOfPinnedObject());
            IntPtr setPointer = Utils.StructToIntPtr(set);
            // NativeHashSet<IntPtr> setRef = UnsafeUtility.AsRef<NativeHashSet<IntPtr>>(setPointer.ToPointer());
            // setRef.Add(dataPtr);
            hashMap.Add(key, setPointer);
        }
        else
        {
            NativeHashSet<IntPtr> set = UnsafeUtility.AsRef<NativeHashSet<IntPtr>>(hashMap[key].ToPointer());
            set.Add(new IntPtr(UnsafeUtility.AddressOf(ref data)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TestChunkList()
    {
        NativeChunkedList<int> list = new NativeChunkedList<int>();
        // list.ad
    }

    void TestMultipleHashMap()
    {
        NativeParallelMultiHashMap<int, int> hashMap = new NativeParallelMultiHashMap<int, int>(10000, Allocator.Temp);
        for (int i = 1; i <= 10; i++)
        {
            hashMap.Add(0, i);
        }
        
        hashMap.RemoveValue(0, 4);
        hashMap.Add(0, 11);

        var values = hashMap.GetValuesForKey(0);
        foreach (var value in values)
        {
            Debug.Log("TestMultipleHashMap " + value);
        }
    }

    void TestCircularList()
    {
        NativeCircularList<int> list = new NativeCircularList<int>(20, Allocator.Persistent);
        for (int i = 0; i < 20; i++)
        {
            list.Add(i);
        }

        StringBuilder sb = new StringBuilder();
        foreach (var a in list)
        {
            sb.Append($"{a} ");
        }
        Debug.Log($"==== {sb.ToString()}");
        
        for (int i = 0; i < 5; i++)
        {
            sb.Clear();
            var tail = list[list.Length - 1];
            list.RemoveTail();
            var head = list[0];
            var tail1 = list[list.Length - 2];
            list.AddHead(tail);
            
            foreach (var a in list)
            {
                sb.Append($"{a} ");
            }
            Debug.Log($"==== {sb.ToString()}");
        }
        
        sb.Clear();
        for (int i = 20; i < 25; i++)
        {
            list.Add(i);
        }
        foreach (var a in list)
        {
            sb.Append($"{a} ");
        }
        Debug.Log($"==== {sb.ToString()}");
        list.Dispose();
    }
}

[BurstCompile(CompileSynchronously = true)]
public unsafe struct TestNativeContainerJob : IJob
{
    public NativeHashMap<int, IntPtr> HashMap;
    
    public void Execute()
    {
        NativeCircularList<int> list = UnsafeUtility.AsRef<NativeCircularList<int>>(HashMap[1].ToPointer());
        for (int i = 0; i < list.Length; i++)
        {
            int a = list.ElementAt(i);
            list[i] = a * 2;
        }
    }
}

public struct Utils
{
    public static IntPtr StructToIntPtr<T>(T structure) where T : struct
    {
        unsafe
        {
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
            Marshal.StructureToPtr(structure, ptr, false);
            return new IntPtr(ptr.ToPointer());
        }
    }

    public static T IntPtrToStruct<T>(IntPtr ptr) where T : struct
    {
        unsafe
        {
            return Marshal.PtrToStructure<T>(new IntPtr(ptr.ToPointer()));
        }
    }
}