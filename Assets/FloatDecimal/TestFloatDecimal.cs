using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LibBase.Utils;
using Unity.Mathematics;
using UnityEngine;

public class TestFloatDecimal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Test();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Test()
    {
        MathUtils.SRand(2000);
        
        StringBuilder str = new StringBuilder();
        float totalFloat = 0;

        for (int i = 0; i < 5000; i++)
        {
            int fand = MathUtils.Arc4Random();
            float f = fand / 100000000.0f; // Convert to float by dividing by a large number
            totalFloat += f;
            float cos = (float)MathUtils.Cos(totalFloat);
            float sin = (float)MathUtils.Sin(totalFloat);
            float tan = (float)MathUtils.Tan(totalFloat);
            float atan2 = (float)MathUtils.Atan2(totalFloat + 100, totalFloat - 100);
        
            // Use string interpolation for formatting
            str.AppendFormat("f:{0,-15} cos:{1,-15} sin:{2,-15} tan:{3,-15} atan2:{4,-15} \n",
                ConvertFloatToString(totalFloat), ConvertFloatToString(cos), ConvertFloatToString(sin), ConvertFloatToString(tan), ConvertFloatToString(atan2));
        }
        
        // for (int i = 0; i < 5000; i++)
        // {
        //     int fand = MathUtils.Arc4Random();
        //     float f = fand / 100000000.0f; // Convert to float by dividing by a large number
        //     totalFloat += f;
        //     float cos = (float)Math.Cos(totalFloat);
        //     float sin = (float)Math.Sin(totalFloat);
        //     float tan = (float)Math.Tan(totalFloat);
        //     float atan2 = (float)Math.Atan2(totalFloat + 100, totalFloat - 100);
        //
        //     // Use string interpolation for formatting
        //     str.AppendFormat("f:{0,-15} cos:{1,-15} sin:{2,-15} tan:{3,-15} atan2:{4,-15} \n",
        //         ConvertFloatToString(totalFloat), ConvertFloatToString(cos), ConvertFloatToString(sin), ConvertFloatToString(tan), ConvertFloatToString(atan2));
        // }
        
        // for (int i = 0; i < 5000; i++)
        // {
        //     int fand = MathUtils.Arc4Random();
        //     float f = fand / 100000000.0f; // Convert to float by dividing by a large number
        //     totalFloat += f;
        //     float cos = (float)math.cos(totalFloat);
        //     float sin = (float)math.sin(totalFloat);
        //     float tan = (float)math.tan(totalFloat);
        //     float atan2 = (float)math.atan2(totalFloat + 100, totalFloat - 100);
        //
        //     // Use string interpolation for formatting
        //     str.AppendFormat("f:{0,-15} cos:{1,-15} sin:{2,-15} tan:{3,-15} atan2:{4,-15} \n",
        //         ConvertFloatToString(totalFloat), ConvertFloatToString(cos), ConvertFloatToString(sin), ConvertFloatToString(tan), ConvertFloatToString(atan2));
        // }

        Debug.Log(str.ToString());
    }
    
    public static string ConvertFloatToString(float value, int decimalPlaces = 8)
    {
        // 检查是否为负数，并且处理为正数以简化计算
        bool isNegative = value < 0;
        if (isNegative)
        {
            value = -value; // 转为正数处理
        }

        // 提取整数部分
        int integralPart = (int)value;
        // 提取小数部分
        float fractionalPart = value - integralPart;
        // 构建结果字符串，包括处理负号
        string result = (isNegative ? "-" : "") + integralPart.ToString() + ".";

        // 处理小数部分
        for (int i = 0; i < decimalPlaces; i++)
        {
            fractionalPart *= 10;
            int digit = (int)fractionalPart;
            result += digit.ToString();
            fractionalPart -= digit;
        }

        return result;
    }
}

// iOS 测试代码
// [MathUtils srand:2000];
// //        NSMutableString *str = [NSMutableString string];
// //        float totalFloat = 0;
// //        for (NSInteger i = 0; i < 5000; i++) {
// //            NSInteger fand = [MathUtils arc4random];
// //            float f = fand / 100000000.0f;
// //            totalFloat += f;
// //            float cos = cosf(totalFloat);
// //            float sin = sinf(totalFloat);
// //            float tan = tanf(totalFloat);
// //            float atan2 = atan2f(totalFloat + 100, totalFloat - 100);
// //            
// //            [str appendFormat:@"f:%@ cos:%@ sin:%@ tan:%@ atan2:%@ \n", [MathUtils convertFloatToString:totalFloat], [MathUtils convertFloatToString:cos], [MathUtils convertFloatToString:sin], [MathUtils convertFloatToString:tan], [MathUtils convertFloatToString:atan2]];
// //        }
//
// NSMutableString *str = [NSMutableString string];
// float totalFloat = 0;
// for (NSInteger i = 0; i < 5000; i++) {
//     NSInteger fand = [MathUtils arc4random];
//     float f = fand / 100000000.0f;
//     totalFloat += f;
//     float cos = [MathUtils cos:totalFloat];
//     float sin = [MathUtils sin:totalFloat];
//     float tan = [MathUtils tan:totalFloat];
//     float atan2 = [MathUtils atan2f:totalFloat + 100 x:totalFloat - 100];
//
//     [str appendFormat:@"f:%@ cos:%@ sin:%@ tan:%@ atan2:%@ \n", [MathUtils convertFloatToString:totalFloat], [MathUtils convertFloatToString:cos], [MathUtils convertFloatToString:sin], [MathUtils convertFloatToString:tan], [MathUtils convertFloatToString:atan2]];
// }
