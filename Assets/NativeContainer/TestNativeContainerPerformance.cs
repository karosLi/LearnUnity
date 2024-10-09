using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace NativeContainer
{
    public struct Node
    {
        public int Value;
    }
    
    public unsafe struct NodePtr : IDisposable
    {
        public Node* Ptr;
        public Allocator Allocator;

        public static NodePtr CreateNodePtr(Allocator allocator, int value)
        {
            NodePtr nodePtr = new NodePtr();
            nodePtr.Ptr = (Node *)UnsafeUtility.Malloc(sizeof(Node), UnsafeUtility.AlignOf<Node>(), allocator);
            nodePtr.Allocator = allocator;
            nodePtr.Ptr->Value = value;
            return nodePtr;
        }

        public void Dispose()
        {
            if (Ptr != null)
            {
                UnsafeUtility.Free(Ptr, Allocator);
                Ptr = null;
            }
        }
    }
    
    public unsafe struct NodePoolPtr : IDisposable
    {
        public Node* Ptr;
        public MemoryPool<Node>* MemoryPool;

        public static NodePoolPtr CreateNodePtr(ref MemoryPool<Node> memoryPool, int value)
        {
            NodePoolPtr nodePtr = new NodePoolPtr();
            nodePtr.Ptr = (Node *)memoryPool.Get();
            nodePtr.Ptr->Value = value;
            nodePtr.MemoryPool = (MemoryPool<Node>*)UnsafeUtility.AddressOf(ref memoryPool);
            return nodePtr;
        }

        public void Dispose()
        {
            if (Ptr != null)
            {
                MemoryPool->Release(new IntPtr(Ptr));
                Ptr = null;
            }
        }
    }

    public class TestNativeContainerPerformance
    {
        [MenuItem("Test/NativeContainer/TestCircularDisposeListPerformance")]
        public static void TestCircularDisposeListPerformance()
        {
            NativeCircularDisposeList<NodePtr> nativeCircularDisposeList = new NativeCircularDisposeList<NodePtr>(20, Allocator.Persistent);
            NativeCircularList<Node> nativeCircularList = new NativeCircularList<Node>(20, Allocator.Persistent);
        
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 200000; i++)
            {
                nativeCircularDisposeList.Add(NodePtr.CreateNodePtr(Allocator.Persistent, i));
            }
            stopwatch.Stop();
            Debug.Log($"NativeCircularDisposeList add {stopwatch.ElapsedMilliseconds}ms");
        
            stopwatch.Start();
            for (int i = 0; i < 200000; i++)
            {
                nativeCircularList.Add(new Node()
                {
                    Value = i
                });
            }
            stopwatch.Stop();
            Debug.Log($"NativeCircularList add {stopwatch.ElapsedMilliseconds}ms");
            
            stopwatch.Start();
            for (int i = 0; i < 20000; i++)
            {
                var tail = nativeCircularDisposeList[nativeCircularDisposeList.Length - 1];
                nativeCircularDisposeList.RemoveTail(false);
                var head = nativeCircularDisposeList[0];
                var tail1 = nativeCircularDisposeList[nativeCircularDisposeList.Length - 2];
                nativeCircularDisposeList.AddHead(tail);
            }
            stopwatch.Stop();
            Debug.Log($"NativeCircularDisposeList move {stopwatch.ElapsedMilliseconds}ms");
            
            stopwatch.Start();
            for (int i = 0; i < 20000; i++)
            {
                var tail = nativeCircularList[nativeCircularList.Length - 1];
                nativeCircularList.RemoveTail();
                var head = nativeCircularList[0];
                var tail1 = nativeCircularList[nativeCircularList.Length - 2];
                nativeCircularList.AddHead(tail);
            }
            stopwatch.Stop();
            Debug.Log($"NativeCircularList move {stopwatch.ElapsedMilliseconds}ms");
        
            stopwatch.Start();
            nativeCircularDisposeList.Clear();
            stopwatch.Stop();
            Debug.Log($"NativeCircularDisposeList clear {stopwatch.ElapsedMilliseconds}ms");
        
            stopwatch.Start();
            nativeCircularList.Clear();
            stopwatch.Stop();
            Debug.Log($"NativeCircularList clear {stopwatch.ElapsedMilliseconds}ms");
        }

        [MenuItem("Test/NativeContainer/TestCircularDisposeListPerformanceBurst")]
        [BurstCompile]
        public static void TestCircularDisposeListPerformanceBurst()
        {
            NativeCircularDisposeList<NodePtr> nativeCircularDisposeList = new NativeCircularDisposeList<NodePtr>(20, Allocator.Persistent);
            NativeCircularList<Node> nativeCircularList = new NativeCircularList<Node>(20, Allocator.Persistent);
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            NativeContainerBurst.TestCircularDisposeListPerformanceAdd(ref nativeCircularDisposeList);
            stopwatch.Stop();
            Debug.Log($"NativeCircularDisposeList Burst add {stopwatch.ElapsedMilliseconds}ms");
            
            stopwatch.Start();
            NativeContainerBurst.TestCircularListPerformanceAdd(ref nativeCircularList);
            stopwatch.Stop();
            Debug.Log($"NativeCircularList Burst add {stopwatch.ElapsedMilliseconds}ms");
            
            stopwatch.Start();
            NativeContainerBurst.TestCircularDisposeListPerformanceMove(ref nativeCircularDisposeList);
            stopwatch.Stop();
            Debug.Log($"NativeCircularDisposeList Burst move {stopwatch.ElapsedMilliseconds}ms");
            
            stopwatch.Start();
            NativeContainerBurst.TestCircularListPerformanceMove(ref nativeCircularList);
            stopwatch.Stop();
            Debug.Log($"NativeCircularList Burst move {stopwatch.ElapsedMilliseconds}ms");
            
            stopwatch.Start();
            NativeContainerBurst.TestCircularDisposeListPerformanceClear(ref nativeCircularDisposeList);
            stopwatch.Stop();
            Debug.Log($"NativeCircularDisposeList Burst clear {stopwatch.ElapsedMilliseconds}ms");
            
            stopwatch.Start();
            NativeContainerBurst.TestCircularListPerformanceClear(ref nativeCircularList);
            stopwatch.Stop();
            Debug.Log($"NativeCircularList Burst clear {stopwatch.ElapsedMilliseconds}ms");
            
            
        }
        
        [MenuItem("Test/NativeContainer/TestCircularDisposeListPoolPerformanceBurst")]
        [BurstCompile]
        public static void TestCircularDisposeListPoolPerformanceBurst()
        {
            MemoryPool<Node> memoryPool = new MemoryPool<Node>(100, Allocator.Persistent);
            NativeCircularDisposeList<NodePoolPtr> nativeCircularDisposeList = new NativeCircularDisposeList<NodePoolPtr>(20, Allocator.Persistent);
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            NativeContainerBurst.TestCircularDisposeListPoolPerformanceAdd(ref nativeCircularDisposeList, ref memoryPool);
            stopwatch.Stop();
            Debug.Log($"NativeCircularDisposeListPool Burst add {stopwatch.ElapsedMilliseconds}ms");
            
            stopwatch.Start();
            NativeContainerBurst.TestCircularDisposeListPoolPerformanceMove(ref nativeCircularDisposeList);
            stopwatch.Stop();
            Debug.Log($"NativeCircularDisposeListPool Burst move {stopwatch.ElapsedMilliseconds}ms");
            
            stopwatch.Start();
            NativeContainerBurst.TestCircularDisposeListPoolPerformanceClear(ref nativeCircularDisposeList);
            stopwatch.Stop();
            Debug.Log($"NativeCircularDisposeListPool Burst clear {stopwatch.ElapsedMilliseconds}ms");
            
            memoryPool.Dispose();
        }
    }
}

[BurstCompile]
public struct NativeContainerBurst
{
    #region CircularDisposeList

    [BurstCompile]
    public static void TestCircularDisposeListPerformanceAdd(ref NativeCircularDisposeList<NodePtr> nativeCircularDisposeList)
    {
        for (int i = 0; i < 200000; i++)
        {
            nativeCircularDisposeList.Add(NodePtr.CreateNodePtr(Allocator.Persistent, i));
        }
    }
    
    [BurstCompile]
    public static void TestCircularDisposeListPerformanceMove(ref NativeCircularDisposeList<NodePtr> nativeCircularDisposeList)
    {
        for (int i = 0; i < 20000; i++)
        {
            var tail = nativeCircularDisposeList[nativeCircularDisposeList.Length - 1];
            nativeCircularDisposeList.RemoveTail(false);
            var head = nativeCircularDisposeList[0];
            var tail1 = nativeCircularDisposeList[nativeCircularDisposeList.Length - 2];
            nativeCircularDisposeList.AddHead(tail);
        }
    }
    
    [BurstCompile]
    public static void TestCircularDisposeListPerformanceClear(ref NativeCircularDisposeList<NodePtr> nativeCircularDisposeList)
    {
        nativeCircularDisposeList.Clear();
    }

    #endregion


    #region CircularDisposeList Pool

    [BurstCompile]
    public static void TestCircularDisposeListPoolPerformanceAdd(ref NativeCircularDisposeList<NodePoolPtr> nativeCircularDisposeList, ref MemoryPool<Node> memoryPool)
    {
        for (int i = 0; i < 200000; i++)
        {
            nativeCircularDisposeList.Add(NodePoolPtr.CreateNodePtr(ref memoryPool, i));
        }
    }
    
    [BurstCompile]
    public static void TestCircularDisposeListPoolPerformanceMove(ref NativeCircularDisposeList<NodePoolPtr> nativeCircularDisposeList)
    {
        for (int i = 0; i < 20000; i++)
        {
            var tail = nativeCircularDisposeList[nativeCircularDisposeList.Length - 1];
            nativeCircularDisposeList.RemoveTail(false);
            var head = nativeCircularDisposeList[0];
            var tail1 = nativeCircularDisposeList[nativeCircularDisposeList.Length - 2];
            nativeCircularDisposeList.AddHead(tail);
        }
    }
    
    [BurstCompile]
    public static void TestCircularDisposeListPoolPerformanceClear(ref NativeCircularDisposeList<NodePoolPtr> nativeCircularDisposeList)
    {
        nativeCircularDisposeList.Clear();
    }

    #endregion

    #region CircularList

    [BurstCompile]
    public static void TestCircularListPerformanceAdd(ref NativeCircularList<Node> nativeCircularList)
    {
        for (int i = 0; i < 200000; i++)
        {
            nativeCircularList.Add(new Node()
            {
                Value = i
            });
        }
    }
    
    [BurstCompile]
    public static void TestCircularListPerformanceMove(ref NativeCircularList<Node> nativeCircularList)
    {
        for (int i = 0; i < 20000; i++)
        {
            var tail = nativeCircularList[nativeCircularList.Length - 1];
            nativeCircularList.RemoveTail();
            var head = nativeCircularList[0];
            var tail1 = nativeCircularList[nativeCircularList.Length - 2];
            nativeCircularList.AddHead(tail);
        }
    }
    
    [BurstCompile]
    public static void TestCircularListPerformanceClear(ref NativeCircularList<Node> nativeCircularList)
    {
        nativeCircularList.Clear();
    }

    #endregion
}