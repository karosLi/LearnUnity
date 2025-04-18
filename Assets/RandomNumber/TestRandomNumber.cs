using System;
using System.Collections;
using System.Collections.Generic;
using LibBase.MathLite;
using RandomNumber;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public unsafe class TestRandomNumber : MonoBehaviour
{
    private System.Random _random;
    private CRandom _cRandom;
    private LibBase.MathLite.FixMath.Random _random1;
    int x = 1000;
    private RandR _randR;// 可用
    private uint i = 0;
    private uint seed = 1000;
    private Unity.Mathematics.Random _MathRandom;
    
    // Start is called before the first frame update
    void Start()
    {
        _random = new Random(1000);
        _random1 = new LibBase.MathLite.FixMath.Random(1000);
        _cRandom = new CRandom();
        _cRandom.Seed = 1000;
        UnityEngine.Random.InitState(1000);

        _randR = new RandR(1000);
    }

    // Update is called once per frame
    void Update()
    {
        // var next = _random.Next(0, 0x7fffffff);
        // next = (int)(UnityEngine.Random.value * 0x7fffffff);
        // var next = _cRandom.Range(0, 0x7fffffff);
        // var next = _random1.Range(0, 0x7fffffff);
        
        // Generate the next x from the current one.
        // unchecked {
        //     // NOTE: x is an `int`
        //     x = ((int)0xadb4a92d * x) + 9999999;
        // }
        //
        
        // float next = BurstMath.RandomWithMinMax(ref seed, 0, 100000);
        // float next = BurstMath.RandomWithMinMax(0, 100000);
        // string str = string.Format("burstMath {0,-5}", next);
        // Debug.Log(str);
        
        var next = _randR.Next();
        Debug.Log(next + " " + i + "  TestRandR");
        i++;
    }

    [MenuItem("Test/Random/TestRandR")]
    public static void TestRandR()
    {
        // Editor 运行结果和场景里运行不一样，随机数还是要以运行时为准
        RandR randR = new RandR(1000);

        for (int i = 0; i < 100000; i++)
        {
            var next = randR.Arc4Random();
            Debug.Log(next + " " + i);
            i++;
        }
    }
}
