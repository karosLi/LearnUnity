using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using JacksonDunstan.NativeCollections;
using LibBase.Utils;
using LitJson;
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

namespace TestJson
{
    
    public class TestJsonPerformance
    {
        [MenuItem("Test/Json/TestJson")]
        public static void Json()
        {
            try
            {
                JsonData jsonData = JsonMapper.ToObject("{\"callback\":\"getUserInfo_success_1729135553401_2\"}");
                var callback = jsonData.GetString("callback");
                Debug.Log(callback);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
            
        }
    }
}
