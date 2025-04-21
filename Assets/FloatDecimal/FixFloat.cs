using System;
using Unity.Mathematics;
using UnityEngine; // 如果你测试中还需要 Inspector 显示，保留这个和 SerializeField

namespace FloatDecimal
{
    /// <summary>
    /// TESTING VERSION of FixFloat using double internally.
    /// WARNING: Loses determinism and fixed-point properties. FOR TESTING ONLY.
    /// </summary>
    [Serializable]
    //[ProtoContract] // 可选，如果测试中不需要序列化，可以注释掉
    public partial struct FixFloat : IEquatable<FixFloat>, IComparable<FixFloat> // 保持原名 FixFloat
    {
        [SerializeField] // 可选
        //[ProtoMember(1)] // 可选
        public double _value; // ***核心改动：使用 double 存储***
        // --- 常量 (使用 double 值) ---
        public static readonly double Precision = double.Epsilon; // 标准 double 精度标记
        public static readonly FixFloat MaxValue = new FixFloat(double.MaxValue);
        public static readonly FixFloat MinValue = new FixFloat(double.MinValue);
        public static readonly FixFloat One = new FixFloat(1.0);
        public static readonly FixFloat Two = new FixFloat(2.0);
        public static readonly FixFloat Three = new FixFloat(3.0);
        public static readonly FixFloat Ten = new FixFloat(10.0);
        public static readonly FixFloat Half = new FixFloat(0.5);
        public static readonly FixFloat Hundred = new FixFloat(100.0);
        public static readonly FixFloat Thousand = new FixFloat(1000.0);
        public static readonly FixFloat Zero = new FixFloat(0.0);
        public static readonly FixFloat PositiveInfinity = new FixFloat(double.PositiveInfinity);
        public static readonly FixFloat NegativeInfinity = new FixFloat(double.NegativeInfinity);
        public static readonly FixFloat NaN = new FixFloat(double.NaN);
        // 小数常量
        public static readonly FixFloat EN1 = new FixFloat(1e-1);
        public static readonly FixFloat EN2 = new FixFloat(1e-2);
        public static readonly FixFloat EN3 = new FixFloat(1e-3);
        // ... (其他 ENX)
        public static readonly FixFloat Epsilon = new FixFloat(1e-7); // 选一个适合比较的 Epsilon
        // 数学常量 (使用 System.Math)
        public static readonly FixFloat Pi = new FixFloat(Math.PI);
        public static readonly FixFloat TwoPi = new FixFloat(Math.PI * 2.0);
        public static readonly FixFloat PiOver2 = new FixFloat(Math.PI / 2.0);
        public static readonly FixFloat PiTimes2 = TwoPi; // Same as TwoPi
        public static readonly FixFloat Deg2Rad = new FixFloat(Math.PI / 180.0);
        public static readonly FixFloat Rad2Deg = new FixFloat(180.0 / Math.PI);
        public static readonly FixFloat Ln2 = new FixFloat(Math.Log(2.0));
        // 移除了定点数特有的常量如 LOG2MAX, LUT_SIZE 等
        // --- 构造函数 (适配 double) ---
        public FixFloat(double value) { _value = value; }
        public FixFloat(float value) { _value = (double)value; }
        public FixFloat(int value) { _value = (double)value; }
        public FixFloat(long value) { _value = (double)value; } // 可能损失精度
        public FixFloat(int nom, int den)
        {
            if (den == 0) _value = double.NaN; // 或者无穷大，根据需要
            else _value = (double)nom / den;
        }
        // 移除了 FromRaw 构造
        // --- 静态数学函数---
        public static int Sign(FixFloat value) => (int)math.sign(value._value);
        public static FixFloat Abs(FixFloat value) => new FixFloat(math.abs(value._value));
        public static FixFloat Floor(FixFloat value) => new FixFloat(math.floor(value._value));
        public static FixFloat Ceiling(FixFloat value) => new FixFloat(math.ceil(value._value));
        public static FixFloat Round(FixFloat value) => new FixFloat(math.round(value._value));
        public static FixFloat Min(FixFloat t1, FixFloat t2) => new FixFloat(math.min(t1._value, t2._value));
        public static FixFloat Max(FixFloat t1, FixFloat t2) => new FixFloat(math.max(t1._value, t2._value));
        public static FixFloat Pow2(FixFloat value) => new FixFloat(value._value * value._value);
        public static FixFloat Pow(FixFloat value, FixFloat exponent) => new FixFloat(math.pow(value._value, exponent._value));
        public static FixFloat Sqrt(FixFloat x) => new FixFloat(math.sqrt(x._value)); // Math.Sqrt 对负数返回 NaN
        public static FixFloat Sin(FixFloat x) => new FixFloat(math.sin(x._value));
        public static FixFloat Cos(FixFloat x) => new FixFloat(math.cos(x._value));
        public static FixFloat Tan(FixFloat x) => new FixFloat(math.tan(x._value));
        public static FixFloat Atan(FixFloat z) => new FixFloat(math.atan(z._value));
        public static FixFloat Atan2(FixFloat y, FixFloat x) => new FixFloat(math.atan2(y._value, x._value));
        public static FixFloat Asin(FixFloat value) => new FixFloat(math.asin(value._value));
        public static FixFloat Acos(FixFloat x) => new FixFloat(math.acos(x._value));
        // 移除了 FastSin, FastCos, LUT 相关逻辑
        // --- 运算符 (使用 double 运算符) ---
        public static FixFloat operator +(FixFloat x, FixFloat y) => new FixFloat(x._value + y._value);
        public static FixFloat operator -(FixFloat x, FixFloat y) => new FixFloat(x._value - y._value);
        public static FixFloat operator *(FixFloat x, FixFloat y) => new FixFloat(x._value * y._value);
        public static FixFloat operator /(FixFloat x, FixFloat y) => new FixFloat(x._value / y._value); // double 除零会得 Infinity/NaN
        public static FixFloat operator %(FixFloat x, FixFloat y) => new FixFloat(x._value % y._value);
        public static FixFloat operator -(FixFloat x) => new FixFloat(-x._value);
        // 移除了 Overflow/Fast 版本，因为 double 有自己的溢出处理 (Infinity/NaN)
        // --- 比较运算符 ---
        public static bool operator ==(FixFloat x, FixFloat y) => x._value == y._value; // 注意 double 比较精度问题
        public static bool operator !=(FixFloat x, FixFloat y) => x._value != y._value;
        public static bool operator >(FixFloat x, FixFloat y) => x._value > y._value;
        public static bool operator <(FixFloat x, FixFloat y) => x._value < y._value;
        public static bool operator >=(FixFloat x, FixFloat y) => x._value >= y._value;
        public static bool operator <=(FixFloat x, FixFloat y) => x._value <= y._value;
        // --- 类型转换 (适配 double) ---
        public static implicit operator FixFloat(double value) => new FixFloat(value);
        public static implicit operator FixFloat(float value) => new FixFloat(value);
        public static implicit operator FixFloat(int value) => new FixFloat(value);
        public static explicit operator FixFloat(long value) => new FixFloat(value);
        public static explicit operator FixFloat(decimal value) => new FixFloat((double)value);
        public static explicit operator long(FixFloat value) => (long)value._value;
        public static explicit operator float(FixFloat value) => (float)value._value;
        public static explicit operator double(FixFloat value) => value._value;
        public static explicit operator decimal(FixFloat value) => (decimal)value._value; // 可能抛异常
        public static explicit operator int(FixFloat value) => (int)value._value;
        // --- 实用方法 ---
        public float AsFloat() => (float)_value;
        public int AsInt() => (int)_value;
        public long AsLong() => (long)_value;
        public double AsDouble() => _value;
        public decimal AsDecimal() => (decimal)_value;
        public static float ToFloat(FixFloat value) => (float)value;
        public static int ToInt(FixFloat value) => (int)value;
        public static double ToDouble(FixFloat value) => value._value;
        public static FixFloat FromFloat(float value) => new FixFloat(value);
        // FromRaw 移除
        public bool IsZero() => _value == 0.0;
        // 可以加一个 Epsilon 比较
        public bool IsNearlyZero(double tolerance = 1e-7) => math.abs(_value) < tolerance;
        public static bool IsInfinity(FixFloat value) => double.IsInfinity(value._value);
        public static bool IsNaN(FixFloat value) => double.IsNaN(value._value);
        // --- 接口实现 ---
        public override bool Equals(object obj) => obj is FixFloat other && Equals(other);
        public override int GetHashCode() => _value.GetHashCode();
        public bool Equals(FixFloat other) => _value.Equals(other._value); // 注意 double.NaN.Equals(double.NaN) 是 true
        public int CompareTo(FixFloat other) => _value.CompareTo(other._value);
        // --- ToString ---
        public override string ToString() => _value.ToString();
        public string ToString(IFormatProvider provider) => _value.ToString(provider);
        public string ToString(string format) => _value.ToString(format);
        // 移除了 RawValue 属性和 LUT 生成代码
    }
}