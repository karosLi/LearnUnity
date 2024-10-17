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

namespace TestJobSystem
{
    
    public class TestNativeContainerPerformance
    {
        [MenuItem("Test/JobSystem/TestParallelFor")]
        public static void TestParallelFor()
        {
        }
    }
    
    public class ParallelFriendlyContainer
    {
        private NativeArray<int> data;
        private int length;

        public ParallelFriendlyContainer(int size)
        {
            data = new NativeArray<int>(size, Allocator.Persistent);
            length = size;
        }

        public void Dispose()
        {
            if (data.IsCreated)
            {
                data.Dispose();
            }
        }

        public JobHandle ScheduleFillJob(int value, JobHandle dependency = default)
        {
            var job = new FillJob
            {
                Data = data,
                Value = value
            };
            return job.Schedule(length, 64, dependency);
        }

        private struct FillJob : IJobParallelFor
        {
            public NativeArray<int> Data;
            public int Value;

            public void Execute(int index)
            {
                Data[index] = Value;
            }
        }
    }
}
