using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LibBase.MathLite.FixMath;
using LibBase.OpenLibm;
using LibBase.Utils;
using Unity.Mathematics;
using UnityEngine;

public class TestDoubleDecimal : MonoBehaviour
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
        // 原生和unity都使用定点数，可以确保基础计算和三角函数计算都是准确的
        MathUtils.SRand(2000);
        
        StringBuilder str = new StringBuilder();
        double totalFloat = 0;

        // for (int i = 0; i < 5000; i++)
        // {
        //     int fand = OLMathUtils.Arc4Random();
        //     float f = fand / 100000000.0; // Convert to float by dividing by a large number
        //     totalFloat += f;
        //     double cos = OLMathUtils.Cos(totalFloat);
        //     double sin = OLMathUtils.Sin(totalFloat);
        //     double tan = OLMathUtils.Tan(totalFloat);
        //     double atan2 = OLMathUtils.Atan2(totalFloat + 100, totalFloat - 100);
        //
        //     // Use string interpolation for formatting
        //     str.AppendFormat("f:{0,-15} cos:{1,-15} sin:{2,-15} tan:{3,-15} atan2:{4,-15} \n",
        //         ConvertFloatToString(totalFloat), ConvertFloatToString(cos), ConvertFloatToString(sin), ConvertFloatToString(tan), ConvertFloatToString(atan2));
        // }
        
        // for (int i = 0; i < 5000; i++)
        // {
        //     int fand = MathUtils.Arc4Random();
        //     double f = fand / 100000000.0; // Convert to float by dividing by a large number
        //     totalFloat += f;
        //     double cos = Math.Cos(totalFloat);
        //     double sin = Math.Sin(totalFloat);
        //     double tan = Math.Tan(totalFloat);
        //     double atan2 = Math.Atan2(totalFloat + 100, totalFloat - 100);
        //
        //     // Use string interpolation for formatting
        //     str.AppendFormat("f:{0,-15} cos:{1,-15} sin:{2,-15} tan:{3,-15} atan2:{4,-15} \n",
        //         ConvertFloatToString(totalFloat), ConvertFloatToString(cos), ConvertFloatToString(sin), ConvertFloatToString(tan), ConvertFloatToString(atan2));
        // }
        
        // for (int i = 0; i < 5000; i++)
        // {
        //     int fand = MathUtils.Arc4Random();
        //     double f = fand / 100000000.0; // Convert to float by dividing by a large number
        //     totalFloat += f;
        //     double cos = math.cos(totalFloat);
        //     double sin = math.sin(totalFloat);
        //     double tan = math.tan(totalFloat);
        //     double atan2 = math.atan2(totalFloat + 100, totalFloat - 100);
        //
        //     // Use string interpolation for formatting
        //     str.AppendFormat("f:{0,-15} cos:{1,-15} sin:{2,-15} tan:{3,-15} atan2:{4,-15} \n",
        //         ConvertFloatToString(totalFloat), ConvertFloatToString(cos), ConvertFloatToString(sin), ConvertFloatToString(tan), ConvertFloatToString(atan2));
        // }
        
        // for (int i = 0; i < 5000; i++)
        // {
        //     int fand = MathUtils.Arc4Random();
        //     double f = fand / 100000000.0; // Convert to float by dividing by a large number
        //     totalFloat += f;
        //     double cos = math.cos(totalFloat);
        //     double sin = math.sin(totalFloat);
        //     double tan = math.tan(totalFloat);
        //     double atan2 = math.atan2(totalFloat + 100, totalFloat - 100);
        //
        //     // Use string interpolation for formatting
        //     str.AppendFormat("f:{0,-15} cos:{1,-15} sin:{2,-15} tan:{3,-15} atan2:{4,-15} \n",
        //         ConvertFloatToString(totalFloat), ConvertFloatToString(cos), ConvertFloatToString(sin), ConvertFloatToString(tan), ConvertFloatToString(atan2));
        // }
        
        // OpenLibmInterop 只能确保三角函数运算公式一样，不能确保 double 本身相乘的计算结果和原生一致
        // for (int i = 0; i < 5000; i++)
        // {
        //     int fand = MathUtils.Arc4Random();
        //     double f = fand / 100000000.0; // Convert to float by dividing by a large number
        //     totalFloat += f;
        //     double cos = OpenLibmInterop.cos(totalFloat);
        //     double sin = OpenLibmInterop.sin(totalFloat);
        //     double tan = OpenLibmInterop.tan(totalFloat);
        //     double atan2 = OpenLibmInterop.atan2(totalFloat + 100, totalFloat - 100);
        //
        //     if (i == 160)
        //     {
        //         int a = 0;
        //     }
        //     // Use string interpolation for formatting
        //     str.AppendFormat("f:{0,-15} cos:{1,-15} sin:{2,-15} tan:{3,-15} atan2:{4,-15} \n",
        //         ConvertFloatToString(totalFloat), ConvertFloatToString(cos), ConvertFloatToString(sin), ConvertFloatToString(tan), ConvertFloatToString(atan2));
        // }
        //
        FixFloat totalFixFloat = 0;
        for (int i = 0; i < 5000; i++)
        {
            int fand = MathUtils.Arc4Random();
            FixFloat f = fand / new FixFloat(100000000.0); // Convert to float by dividing by a large number
            totalFixFloat += f;
            FixFloat cos = FixFloat.Cos(totalFixFloat);
            FixFloat sin = FixFloat.Sin(totalFixFloat);
            FixFloat atan2 = FixFloat.Atan2(totalFixFloat + 100, totalFixFloat - 100);
        
            if (i == 4999)
            {
                int a = 0;
            }
            
            // Use string interpolation for formatting
            // str.AppendFormat("f:{0,-15} cos:{1,-15} sin:{2,-15} atan2:{3,-15} \n",
            //     ConvertFloatToString(totalFixFloat), ConvertFloatToString(cos), ConvertFloatToString(sin), ConvertFloatToString(atan2));
            
            str.AppendFormat("f:{0,-20} cos:{1,-20} sin:{2,-20} atan2:{3,-20} \n",
                totalFixFloat.RawValue, cos.RawValue, sin.RawValue, atan2.RawValue);
        }

        Debug.Log(str.ToString());
    }
    
    public static string ConvertFloatToString(double value, int decimalPlaces = 4)
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
        double fractionalPart = value - integralPart;
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
    
    
    public static string ConvertFloatToString(FixFloat value, int decimalPlaces = 4)
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
        FixFloat fractionalPart = value - integralPart;
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
// #import "FixFloatTable.h"
// #import "FixFloat.hpp"
// [FixFloatTable setup];
// [MathUtils srand:2000];
// NSMutableString *str = [NSMutableString string];
// FixFloat totalFloat = FixFloat(0);
// for (NSInteger i = 0; i < 5000; i++) {
//     int fand = [OLMathUtils arc4random];
//     FixFloat f = fand / FixFloat(100000000.0);
//     totalFloat += f;
//     FixFloat cos = FixFloat::Cos(totalFloat);
//     FixFloat sin = FixFloat::Sin(totalFloat);
//     FixFloat atan2 = FixFloat::Atan2(totalFloat + 100, totalFloat - 100);
//        
//     if (i == 160) {
//         int a = 0;
//     }
//             
// //            [str appendFormat:@"f:%@ cos:%@ sin:%@ atan2:%@ \n", [OLMathUtils convertFloatToString:totalFloat], [OLMathUtils convertFloatToString:cos], [OLMathUtils convertFloatToString:sin], [OLMathUtils convertFloatToString:atan2]];
//             
//     [str appendFormat:@"f:%-20lld cos:%-20lld sin:%-20lld atan2:%-20lld  \n", totalFloat.rawValue, cos.rawValue, sin.rawValue, atan2.rawValue];
// }
//         
// NSLog(@"%@", str);
