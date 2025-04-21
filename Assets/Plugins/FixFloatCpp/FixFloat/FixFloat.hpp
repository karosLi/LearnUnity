// FixFloat.hpp（更新版）
#ifndef FixFloat_hpp
#define FixFloat_hpp

#include <stdio.h>
#include <vector>
#include <cmath>

class FixFloat {
public:
    // 存储原始值
    int64_t rawValue;
    
    // 常量定义（保持不变）
    static const int64_t MAX_VALUE = INT64_MAX;
    static const int64_t MIN_VALUE = INT64_MIN;
    static const int NUM_BITS = 64;
    static const int FRACTIONAL_PLACES = 32;
    static const int64_t ONE = 1LL << FRACTIONAL_PLACES;
    static const int64_t TWO = 2LL << FRACTIONAL_PLACES;
    static const int64_t THREE = 3LL << FRACTIONAL_PLACES;
    static const int64_t TEN = 10LL << FRACTIONAL_PLACES;
    static const int64_t HALF = 1LL << (FRACTIONAL_PLACES - 1);
    static const int64_t PI_TIMES_2 = 0x6487ED511;
    static const int64_t PI = 0x3243F6A88;
    static const int64_t PI_OVER_2 = 0x1921FB544;
    static const int64_t LN2 = 0xB17217F7;
    static const int64_t LOG2MAX = 0x1F00000000;
    static const int64_t LOG2MIN = -0x2000000000;
    static const int LUT_SIZE = (int)(PI_OVER_2 >> 15);
    static const int64_t HUNDRED = 100LL << FRACTIONAL_PLACES;
    static const int64_t THOUSAND = 1000LL << FRACTIONAL_PLACES;
    
    // 构造函数
    FixFloat() : rawValue(0) {}
    FixFloat(int64_t raw) : rawValue(raw) {}
    FixFloat(int value) : rawValue((int64_t)value * ONE) {}
    FixFloat(float value) : rawValue((int64_t)(value * ONE)) {}
    FixFloat(double value) : rawValue((int64_t)(value * ONE)) {}
    
    // 静态常量实例（保持不变）
    static const FixFloat MaxValue;
    static const FixFloat MinValue;
    static const FixFloat One;
    static const FixFloat Two;
    static const FixFloat Three;
    static const FixFloat Ten;
    static const FixFloat Half;
    static const FixFloat Hundred;
    static const FixFloat Thousand;
    static const FixFloat Zero;
    static const FixFloat PositiveInfinity;
    static const FixFloat NegativeInfinity;
    static const FixFloat NaN;
    static const FixFloat Pi;
    static const FixFloat TwoPi;
    static const FixFloat PiOver2;
    static const FixFloat PiTimes2;
    static const FixFloat PiInv;
    static const FixFloat PiOver2Inv;
    static const FixFloat Deg2Rad;
    static const FixFloat Rad2Deg;
    static const FixFloat LutInterval;
    static const FixFloat Log2Max;
    static const FixFloat Log2Min;
    static const FixFloat Ln2;
    static const FixFloat EN1;
    static const FixFloat EN2;
    static const FixFloat EN3;
    static const FixFloat EN4;
    static const FixFloat EN5;
    static const FixFloat EN6;
    static const FixFloat EN7;
    static const FixFloat EN8;
    static const FixFloat Epsilon;
    
    // 工厂方法（保持不变）
    static FixFloat FromRaw(int64_t rawValue);
    static FixFloat FromInt(int value);
    static FixFloat FromFloat(float value);
    static FixFloat FromDouble(double value);
    static FixFloat FromFraction(int nominator, int denominator);

    // 三角函数表（保持不变）
    static void SetSinTable(const std::vector<FixFloat>& sinTable);
    static void SetCosTable(const std::vector<FixFloat>& cosTable);
    
    // 类型转换（保持不变）
    float AsFloat() const;
    int AsInt() const;
    int64_t AsLong() const;
    double AsDouble() const;
    
    // 运算符重载（FixFloat 与 FixFloat 的操作，保持不变）
    FixFloat operator+(const FixFloat& other) const;
    FixFloat operator-(const FixFloat& other) const;
    FixFloat operator*(const FixFloat& other) const;
    FixFloat operator/(const FixFloat& other) const;
    FixFloat operator%(const FixFloat& other) const;
    FixFloat operator-() const;
    
    FixFloat& operator+=(const FixFloat& other);
    FixFloat& operator-=(const FixFloat& other);
    FixFloat& operator*=(const FixFloat& other);
    FixFloat& operator/=(const FixFloat& other);
    FixFloat& operator%=(const FixFloat& other);
    
    bool operator==(const FixFloat& other) const;
    bool operator!=(const FixFloat& other) const;
    bool operator>(const FixFloat& other) const;
    bool operator<(const FixFloat& other) const;
    bool operator>=(const FixFloat& other) const;
    bool operator<=(const FixFloat& other) const;
    
    // 新增：与基本类型的操作
    // FixFloat 与 int
    FixFloat operator+(int other) const;
    FixFloat operator-(int other) const;
    FixFloat operator*(int other) const;
    FixFloat operator/(int other) const;
    FixFloat operator%(int other) const;
    FixFloat& operator+=(int other);
    FixFloat& operator-=(int other);
    FixFloat& operator*=(int other);
    FixFloat& operator/=(int other);
    FixFloat& operator%=(int other);
    
    bool operator==(int other) const;
    bool operator!=(int other) const;
    bool operator>(int other) const;
    bool operator<(int other) const;
    bool operator>=(int other) const;
    bool operator<=(int other) const;
    
    // FixFloat 与 float
    FixFloat operator+(float other) const;
    FixFloat operator-(float other) const;
    FixFloat operator*(float other) const;
    FixFloat operator/(float other) const;
    FixFloat& operator+=(float other);
    FixFloat& operator-=(float other);
    FixFloat& operator*=(float other);
    FixFloat& operator/=(float other);
    
    bool operator==(float other) const;
    bool operator!=(float other) const;
    bool operator>(float other) const;
    bool operator<(float other) const;
    bool operator>=(float other) const;
    bool operator<=(float other) const;
    
    // FixFloat 与 double
    FixFloat operator+(double other) const;
    FixFloat operator-(double other) const;
    FixFloat operator*(double other) const;
    FixFloat operator/(double other) const;
    FixFloat& operator+=(double other);
    FixFloat& operator-=(double other);
    FixFloat& operator*=(double other);
    FixFloat& operator/=(double other);
    
    // 位取反操作符
    FixFloat operator~() const; // 对 rawValue 进行位取反
    // 位运算操作符
    FixFloat operator&(const FixFloat& other) const; // 按位与
    FixFloat operator|(const FixFloat& other) const; // 按位或
    FixFloat operator^(const FixFloat& other) const; // 按位异或
    FixFloat operator<<(int shift) const; // 左移
    FixFloat operator>>(int shift) const; // 右移

    // 复合赋值位运算操作符
    FixFloat& operator&=(const FixFloat& other);
    FixFloat& operator|=(const FixFloat& other);
    FixFloat& operator^=(const FixFloat& other);
    FixFloat& operator<<=(int shift);
    FixFloat& operator>>=(int shift);
    
    bool operator==(double other) const;
    bool operator!=(double other) const;
    bool operator>(double other) const;
    bool operator<(double other) const;
    bool operator>=(double other) const;
    bool operator<=(double other) const;
    
    // 隐式转换操作符（保持不变）
    operator float() const;
    operator double() const;
    
    // 显式转换操作符（保持不变）
    explicit operator int() const;
    explicit operator int64_t() const;
    
    // 数学函数（保持不变）
    static int Sign(const FixFloat& value);
    static FixFloat Abs(const FixFloat& value);
    static FixFloat FastAbs(const FixFloat& value);
    static FixFloat Floor(const FixFloat& value);
    static FixFloat Ceiling(const FixFloat& value);
    static FixFloat Round(const FixFloat& value);
    static FixFloat Min(const FixFloat& a, const FixFloat& b);
    static FixFloat Max(const FixFloat& a, const FixFloat& b);
    static FixFloat Pow2(const FixFloat& value);
    static FixFloat Pow(const FixFloat& value, const FixFloat& exponent);
    static FixFloat OverflowAdd(const FixFloat& a, const FixFloat& b);
    static FixFloat FastAdd(const FixFloat& a, const FixFloat& b);
    static FixFloat OverflowSub(const FixFloat& a, const FixFloat& b);
    static FixFloat FastSub(const FixFloat& a, const FixFloat& b);
    static FixFloat OverflowMul(const FixFloat& a, const FixFloat& b);
    static FixFloat FastMul(const FixFloat& a, const FixFloat& b);
    static FixFloat FastMod(const FixFloat& a, const FixFloat& b);
    static FixFloat Sqrt(const FixFloat& value);
    static FixFloat Sin(const FixFloat& value);
    static FixFloat FastSin(const FixFloat& value);
    static FixFloat Cos(const FixFloat& value);
    static FixFloat FastCos(const FixFloat& value);
    static FixFloat Atan(const FixFloat& value);
    static FixFloat Atan2(const FixFloat& y, const FixFloat& x);
    static FixFloat Asin(const FixFloat& value);
    static FixFloat Acos(const FixFloat& value);
    
    // 实用函数（保持不变）
    bool IsZero() const;
    static bool IsInfinity(const FixFloat& value);
    static bool IsNaN(const FixFloat& value);
    
    // 辅助函数（保持不变）
    static int CountLeadingZeroes(uint64_t x);
    static int64_t ClampSinValue(int64_t angle, bool& flipHorizontal, bool& flipVertical);
    static int64_t AddOverflowHelper(int64_t x, int64_t y, bool& overflow);
    
    // 字符串转换（保持不变）
    const char* ToString() const;
    
private:
    static std::vector<FixFloat> _sinTable;
    static std::vector<FixFloat> _cosTable;
};

// 全局运算符重载（基本类型与 FixFloat）
// int 与 FixFloat
FixFloat operator+(int a, const FixFloat& b);
FixFloat operator-(int a, const FixFloat& b);
FixFloat operator*(int a, const FixFloat& b);
FixFloat operator/(int a, const FixFloat& b);
FixFloat operator%(int a, const FixFloat& b);

bool operator==(int a, const FixFloat& b);
bool operator!=(int a, const FixFloat& b);
bool operator>(int a, const FixFloat& b);
bool operator<(int a, const FixFloat& b);
bool operator>=(int a, const FixFloat& b);
bool operator<=(int a, const FixFloat& b);

// float 与 FixFloat
FixFloat operator+(float a, const FixFloat& b);
FixFloat operator-(float a, const FixFloat& b);
FixFloat operator*(float a, const FixFloat& b);
FixFloat operator/(float a, const FixFloat& b);

bool operator==(float a, const FixFloat& b);
bool operator!=(float a, const FixFloat& b);
bool operator>(float a, const FixFloat& b);
bool operator<(float a, const FixFloat& b);
bool operator>=(float a, const FixFloat& b);
bool operator<=(float a, const FixFloat& b);

// double 与 FixFloat
FixFloat operator+(double a, const FixFloat& b);
FixFloat operator-(double a, const FixFloat& b);
FixFloat operator*(double a, const FixFloat& b);
FixFloat operator/(double a, const FixFloat& b);

bool operator==(double a, const FixFloat& b);
bool operator!=(double a, const FixFloat& b);
bool operator>(double a, const FixFloat& b);
bool operator<(double a, const FixFloat& b);
bool operator>=(double a, const FixFloat& b);
bool operator<=(double a, const FixFloat& b);

#endif /* FixFloat_hpp */
