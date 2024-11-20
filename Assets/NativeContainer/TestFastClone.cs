using System;
using System.Collections;
using System.Collections.Generic;
using NativeContainer;
using Unity.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestFastClone : MonoBehaviour
{
    [MenuItem("Test/NativeContainer/TestCircularResize")]
    static void Test()
    {
        var logicBodyData = new NativeCircularList<BodyPointStruct>(10000, Allocator.Persistent);
        // var renderBodyData = new NativeCircularList<BodyPointStruct>(10, Allocator.Persistent);
        try
        {
            // for (int i = 0; i < 10000; i++)
            // {
            //     var bps = new BodyPointStruct()
            //     {
            //         Pos = new float3(Random.value, Random.value, 0f),
            //         Width = Random.value * 100,
            //         SmoothRSin = Random.value,
            //     };
            //     logicBodyData.Add(bps);
            // }
            // logicBodyData.Capacity = 20000;
            //
            // for (int i = 0; i < 2000; i++)
            // {
            //     var bps = new BodyPointStruct()
            //     {
            //         Pos = new float3(Random.value, Random.value, 0f),
            //         Width = Random.value * 100,
            //         SmoothRSin = Random.value,
            //     };
            //     logicBodyData.Add(bps);
            // }



            // var nativeList = new NativeList<BodyPointStruct>(10000, Allocator.Persistent);
            // var b = nativeList;
            //
            // // var logicBodyData = new NativeCircularList<BodyPointStruct>(10000, Allocator.Persistent);
            var s = logicBodyData;
            for (int i = 0; i < 12000; i++)
            {
                var bps = new BodyPointStruct()
                {
                    Pos = new float3(Random.value, Random.value, 0f),
                    Width = Random.value * 100,
                    SmoothRSin = Random.value,
                };
                s.Add(bps);
            }
            
            // RandomInit(logicBodyData, 12000);
            // RandomInit(renderBodyData, 8000);

            // RandomInit(renderBodyList, 12000);

            Debug.Log($"logicBodyData={logicBodyData.Length}");
            // Debug.Log($"logicBodyData={logicBodyData.Length}, renderBodyData={renderBodyData.Length}");
            // renderBodyData.FastClone(logicBodyData);

            // for (int i = 0; i < logicBodyData.Length; i++)
            // {
            //     var lbps = logicBodyData[i];
            //     var rbps = renderBodyData[i];
            //     if (lbps.Pos.x != rbps.Pos.x || lbps.Pos.y != rbps.Pos.y || lbps.Width != rbps.Width || lbps.SmoothRSin != rbps.SmoothRSin)
            //     {
            //         Debug.Log($"not {i}: {lbps.Pos.x}, {lbps.Pos.y}, {lbps.Width}, {lbps.SmoothRSin}. {rbps.Pos.x}, {rbps.Pos.y}, {rbps.Width}, {rbps.SmoothRSin}");
            //     }
            // }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            logicBodyData.Dispose();
            // renderBodyData.Dispose();
        }
    }

    void RandomInit(NativeCircularList<BodyPointStruct> bodyPointStructs, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var bps = new BodyPointStruct()
            {
                Pos = new float3(Random.value, Random.value, 0f),
                Width = Random.value * 100,
                SmoothRSin = Random.value,
            };
            bodyPointStructs.Add(bps);
        }

        // for (int i = 0; i < count; i++)
        // {
        //     var tail = bodyPointStructs.ElementAt(bodyPointStructs.Length - 1);
        //     if (Random.value > 0.5f)
        //     {
        //         bodyPointStructs.RemoveTail();
        //     }
        //     if (Random.value > 0.5f)
        //     {
        //         tail.Width = Random.value * 50;
        //         bodyPointStructs.AddHead(tail);
        //     }
        // }
    }

    void RandomInit(NativeList<BodyPointStruct> bodyPointStructs, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var bps = new BodyPointStruct()
            {
                Pos = new float3(Random.value, Random.value, 0f),
                Width = Random.value * 100,
                SmoothRSin = Random.value,
            };
            bodyPointStructs.Add(bps);
        }

        // for (int i = 0; i < count; i++)
        // {
        //     var tail = bodyPointStructs.ElementAt(bodyPointStructs.Length - 1);
        //     if (Random.value > 0.5f)
        //     {
        //         bodyPointStructs.RemoveTail();
        //     }
        //     if (Random.value > 0.5f)
        //     {
        //         tail.Width = Random.value * 50;
        //         bodyPointStructs.AddHead(tail);
        //     }
        // }
    }
}