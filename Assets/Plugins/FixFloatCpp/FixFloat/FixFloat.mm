// FixFloat.mm
#include "FixFloat.hpp"
#include <sstream>
#include <iostream>

// 静态成员初始化
std::vector<FixFloat> FixFloat::_sinTable;
std::vector<FixFloat> FixFloat::_cosTable;

// 静态常量实例初始化
const FixFloat FixFloat::MaxValue = FixFloat(MAX_VALUE - 1);
const FixFloat FixFloat::MinValue = FixFloat(MIN_VALUE + 2);
const FixFloat FixFloat::One = FixFloat(ONE);
const FixFloat FixFloat::Two = FixFloat(TWO);
const FixFloat FixFloat::Three = FixFloat(THREE);
const FixFloat FixFloat::Ten = FixFloat(TEN);
const FixFloat FixFloat::Half = FixFloat(HALF);
const FixFloat FixFloat::Hundred = FixFloat(HUNDRED);
const FixFloat FixFloat::Thousand = FixFloat(THOUSAND);
const FixFloat FixFloat::Zero = FixFloat(0);
const FixFloat FixFloat::PositiveInfinity = FixFloat(MAX_VALUE);
const FixFloat FixFloat::NegativeInfinity = FixFloat(MIN_VALUE + 1);
const FixFloat FixFloat::NaN = FixFloat(MIN_VALUE);
const FixFloat FixFloat::Pi = FixFloat(PI);
const FixFloat FixFloat::TwoPi = FixFloat(PI * 2);
const FixFloat FixFloat::PiOver2 = FixFloat(PI_OVER_2);
const FixFloat FixFloat::PiTimes2 = FixFloat(PI_TIMES_2);
const FixFloat FixFloat::PiInv = FixFloat((int64_t)(0.3183098861837906715377675267 * ONE));
const FixFloat FixFloat::PiOver2Inv = FixFloat((int64_t)(0.6366197723675813430755350535 * ONE));
const FixFloat FixFloat::EN1 = FixFloat::One / FixFloat(10);
const FixFloat FixFloat::EN2 = FixFloat::One / FixFloat(100);
const FixFloat FixFloat::EN3 = FixFloat::One / FixFloat(1000);
const FixFloat FixFloat::EN4 = FixFloat::One / FixFloat(10000);
const FixFloat FixFloat::EN5 = FixFloat::One / FixFloat(100000);
const FixFloat FixFloat::EN6 = FixFloat::One / FixFloat(1000000);
const FixFloat FixFloat::EN7 = FixFloat::One / FixFloat(10000000);
const FixFloat FixFloat::EN8 = FixFloat::One / FixFloat(100000000);
const FixFloat FixFloat::Epsilon = FixFloat::EN3;
const FixFloat FixFloat::Deg2Rad = FixFloat::Pi / FixFloat(180);
const FixFloat FixFloat::Rad2Deg = FixFloat(180) / FixFloat::Pi;
const FixFloat FixFloat::LutInterval = FixFloat(LUT_SIZE - 1) / FixFloat::PiOver2;
const FixFloat FixFloat::Log2Max = FixFloat(LOG2MAX);
const FixFloat FixFloat::Log2Min = FixFloat(LOG2MIN);
const FixFloat FixFloat::Ln2 = FixFloat(LN2);

// 工厂方法
FixFloat FixFloat::FromRaw(int64_t rawValue) {
    return FixFloat(rawValue);
}

FixFloat FixFloat::FromInt(int value) {
    return FixFloat(value);
}

FixFloat FixFloat::FromFloat(float value) {
    return FixFloat(value);
}

FixFloat FixFloat::FromDouble(double value) {
    return FixFloat(value);
}

FixFloat FixFloat::FromFraction(int nominator, int denominator) {
    return FixFloat((int64_t)nominator * ONE / denominator);
}

// 三角函数表
void FixFloat::SetSinTable(const std::vector<FixFloat>& sinTable) {
    _sinTable = sinTable;
}

void FixFloat::SetCosTable(const std::vector<FixFloat>& cosTable) {
    _cosTable = cosTable;
}

// 类型转换
float FixFloat::AsFloat() const {
    return (float)rawValue / ONE;
}

int FixFloat::AsInt() const {
    return (int)(rawValue / ONE);
}

int64_t FixFloat::AsLong() const {
    return rawValue >> FRACTIONAL_PLACES;
}

double FixFloat::AsDouble() const {
    return (double)rawValue / ONE;
}

// 隐式转换操作符
FixFloat::operator float() const {
    return AsFloat();
}

FixFloat::operator double() const {
    return AsDouble();
}

// 显式转换操作符
FixFloat::operator int() const {
    return AsInt();
}

FixFloat::operator int64_t() const {
    return AsLong();
}

// 数学函数
int FixFloat::Sign(const FixFloat& value) {
    if (value.rawValue < 0) return -1;
    if (value.rawValue > 0) return 1;
    return 0;
}

FixFloat FixFloat::Abs(const FixFloat& value) {
    if (value.rawValue == MIN_VALUE) {
        return MaxValue;
    }
    
    // branchless implementation
    int64_t mask = value.rawValue >> 63;
    return FixFloat((value.rawValue + mask) ^ mask);
}

FixFloat FixFloat::FastAbs(const FixFloat& value) {
    // branchless implementation
    int64_t mask = value.rawValue >> 63;
    return FixFloat((value.rawValue + mask) ^ mask);
}

FixFloat FixFloat::Floor(const FixFloat& value) {
    // Just zero out the fractional part and convert back to signed int64_t
    return FixFloat((int64_t)((uint64_t)value.rawValue & 0xFFFFFFFF00000000));
}

FixFloat FixFloat::Ceiling(const FixFloat& value) {
    bool hasFractionalPart = (value.rawValue & 0x00000000FFFFFFFF) != 0;
    if (hasFractionalPart) {
        return Floor(value) + One;
    } else {
        return value;
    }
}

FixFloat FixFloat::Round(const FixFloat& value) {
    int64_t fractionalPart = value.rawValue & 0x00000000FFFFFFFF;
    FixFloat integralPart = Floor(value);
    
    if (fractionalPart < 0x80000000) {
        return integralPart;
    }
    
    if (fractionalPart > 0x80000000) {
        return integralPart + One;
    }
    
    // if number is halfway between two values, round to the nearest even number
    if ((integralPart.rawValue & ONE) == 0) {
        return integralPart;
    } else {
        return integralPart + One;
    }
}

FixFloat FixFloat::Min(const FixFloat& a, const FixFloat& b) {
    return (a.rawValue >= b.rawValue) ? b : a;
}

FixFloat FixFloat::Max(const FixFloat& a, const FixFloat& b) {
    return (a.rawValue >= b.rawValue) ? a : b;
}

FixFloat FixFloat::Pow2(const FixFloat& value) {
    return value * value;
}

FixFloat FixFloat::Pow(const FixFloat& value, const FixFloat& exponent) {
    // 使用双精度浮点数近似
    double baseVal = value.AsDouble();
    double expVal = exponent.AsDouble();
    double result = pow(baseVal, expVal);
    return FixFloat(result);
}

FixFloat FixFloat::OverflowAdd(const FixFloat& a, const FixFloat& b) {
    int64_t xl = a.rawValue;
    int64_t yl = b.rawValue;
    int64_t sum = xl + yl;
    
    // if signs of operands are equal and signs of sum and x are different
    if (((~(xl ^ yl) & (xl ^ sum)) & MIN_VALUE) != 0) {
        sum = xl > 0 ? MAX_VALUE : MIN_VALUE;
    }
    
    return FixFloat(sum);
}

FixFloat FixFloat::FastAdd(const FixFloat& a, const FixFloat& b) {
    return FixFloat(a.rawValue + b.rawValue);
}

FixFloat FixFloat::OverflowSub(const FixFloat& a, const FixFloat& b) {
    int64_t xl = a.rawValue;
    int64_t yl = b.rawValue;
    int64_t diff = xl - yl;
    
    // if signs of operands are different and signs of diff and x are different
    if ((((xl ^ yl) & (xl ^ diff)) & MIN_VALUE) != 0) {
        diff = xl < 0 ? MIN_VALUE : MAX_VALUE;
    }
    
    return FixFloat(diff);
}

FixFloat FixFloat::FastSub(const FixFloat& a, const FixFloat& b) {
    return FixFloat(a.rawValue - b.rawValue);
}

int64_t FixFloat::AddOverflowHelper(int64_t x, int64_t y, bool& overflow) {
    int64_t sum = x + y;
    // x + y overflows if sign(x) ^ sign(y) != sign(sum)
    overflow |= ((x ^ y ^ sum) & MIN_VALUE) != 0;
    return sum;
}

FixFloat FixFloat::OverflowMul(const FixFloat& a, const FixFloat& b) {
    int64_t xl = a.rawValue;
    int64_t yl = b.rawValue;
    
    uint64_t xlo = (uint64_t)(xl & 0x00000000FFFFFFFF);
    int64_t xhi = xl >> FRACTIONAL_PLACES;
    uint64_t ylo = (uint64_t)(yl & 0x00000000FFFFFFFF);
    int64_t yhi = yl >> FRACTIONAL_PLACES;
    
    uint64_t lolo = xlo * ylo;
    int64_t lohi = (int64_t)xlo * yhi;
    int64_t hilo = xhi * (int64_t)ylo;
    int64_t hihi = xhi * yhi;
    
    uint64_t loResult = lolo >> FRACTIONAL_PLACES;
    int64_t midResult1 = lohi;
    int64_t midResult2 = hilo;
    int64_t hiResult = hihi << FRACTIONAL_PLACES;
    
    bool overflow = false;
    int64_t sum = AddOverflowHelper((int64_t)loResult, midResult1, overflow);
    sum = AddOverflowHelper(sum, midResult2, overflow);
    sum = AddOverflowHelper(sum, hiResult, overflow);
    
    bool opSignsEqual = ((xl ^ yl) & MIN_VALUE) == 0;
    
    // If signs of operands are equal and sign of result is negative,
    // then multiplication overflowed positively
    if (opSignsEqual) {
        if (sum < 0 || (overflow && xl > 0)) {
            return MaxValue;
        }
    } else {
        if (sum > 0) {
            return MinValue;
        }
    }
    
    // If the top 32 bits of hihi (unused in the result) are neither all 0s or 1s,
    // then this means the result overflowed.
    int64_t topCarry = hihi >> FRACTIONAL_PLACES;
    if (topCarry != 0 && topCarry != -1) {
        return opSignsEqual ? MaxValue : MinValue;
    }
    
    // If signs differ, both operands' magnitudes are greater than 1,
    // and the result is greater than the negative operand, then there was negative overflow.
    if (!opSignsEqual) {
        int64_t posOp, negOp;
        if (xl > yl) {
            posOp = xl;
            negOp = yl;
        } else {
            posOp = yl;
            negOp = xl;
        }
        
        if (sum > negOp && negOp < -ONE && posOp > ONE) {
            return MinValue;
        }
    }
    
    return FixFloat(sum);
}

FixFloat FixFloat::FastMul(const FixFloat& a, const FixFloat& b) {
    int64_t xl = a.rawValue;
    int64_t yl = b.rawValue;
    
    uint64_t xlo = (uint64_t)(xl & 0x00000000FFFFFFFF);
    int64_t xhi = xl >> FRACTIONAL_PLACES;
    uint64_t ylo = (uint64_t)(yl & 0x00000000FFFFFFFF);
    int64_t yhi = yl >> FRACTIONAL_PLACES;
    
    uint64_t lolo = xlo * ylo;
    int64_t lohi = (int64_t)xlo * yhi;
    int64_t hilo = xhi * (int64_t)ylo;
    int64_t hihi = xhi * yhi;
    
    uint64_t loResult = lolo >> FRACTIONAL_PLACES;
    int64_t midResult1 = lohi;
    int64_t midResult2 = hilo;
    int64_t hiResult = hihi << FRACTIONAL_PLACES;
    
    int64_t sum = (int64_t)loResult + midResult1 + midResult2 + hiResult;
    return FixFloat(sum);
}

int FixFloat::CountLeadingZeroes(uint64_t x) {
    int result = 0;
    while ((x & 0xF000000000000000) == 0) {
        result += 4;
        x <<= 4;
    }
    
    while ((x & 0x8000000000000000) == 0) {
        result += 1;
        x <<= 1;
    }
    
    return result;
}

FixFloat FixFloat::FastMod(const FixFloat& a, const FixFloat& b) {
    return FixFloat(a.rawValue % b.rawValue);
}

FixFloat FixFloat::Sqrt(const FixFloat& value) {
    int64_t xl = value.rawValue;
    if (xl < 0) {
        throw std::invalid_argument("Negative value passed to Sqrt");
    }
    
    uint64_t num = (uint64_t)xl;
    uint64_t result = 0;
    
    // second-to-top bit
    uint64_t bit = 1ULL << (NUM_BITS - 2);
    
    while (bit > num) {
        bit >>= 2;
    }
    
    // The main part is executed twice, in order to avoid
    // using 128 bit values in computations.
    for (int i = 0; i < 2; ++i) {
        // First we get the top 48 bits of the answer.
        while (bit != 0) {
            if (num >= result + bit) {
                num -= result + bit;
                result = (result >> 1) + bit;
            } else {
                result = result >> 1;
            }
            bit >>= 2;
        }
        
        if (i == 0) {
            // Then process it again to get the lowest 16 bits.
            if (num > (1ULL << (NUM_BITS / 2)) - 1) {
                // The remainder 'num' is too large to be shifted left
                // by 32, so we have to add 1 to result manually and
                // adjust 'num' accordingly.
                num -= result;
                num = (num << (NUM_BITS / 2)) - 0x80000000ULL;
                result = (result << (NUM_BITS / 2)) + 0x80000000ULL;
            } else {
                num <<= (NUM_BITS / 2);
                result <<= (NUM_BITS / 2);
            }
            
            bit = 1ULL << (NUM_BITS / 2 - 2);
        }
    }
    
    // Finally, if next bit would have been 1, round the result upwards.
    if (num > result) {
        ++result;
    }
    
    return FixFloat((int64_t)result);
}

FixFloat FixFloat::Sin(const FixFloat& value) {
    // 确保表已经初始化
    if (_sinTable.empty()) {
        return Zero;
    }
    
    int angleInt = (value * Hundred).AsInt();
    angleInt = angleInt % (360 * 100);
    
    if (angleInt > 180 * 100) {
        angleInt -= 360 * 100;
    } else if (angleInt < -180 * 100) {
        angleInt += 360 * 100;
    }
    
    // -180
    if (angleInt == -180 * 100 || angleInt == 180 * 100) return Zero;
    
    bool isOverZero = true;
    if (angleInt < 0) {
        isOverZero = false;
        angleInt = -angleInt;
    }
    
    if (angleInt <= 90 * 100) {
        return isOverZero ? _sinTable[angleInt] : -_sinTable[angleInt];
    }
    
    angleInt = 180 * 100 - angleInt;
    return isOverZero ? _sinTable[angleInt] : -_sinTable[angleInt];
}

// 传入弧度，计算正弦值
FixFloat FixFloat::SinRad(const FixFloat& radians) {
    FixFloat degrees = radians * Rad2Deg;  // 先转换为角度
    return Sin(degrees);
}

FixFloat FixFloat::FastSin(const FixFloat& value) {
    return Zero; // 暂时返回零，可以根据需要实现
}

int64_t FixFloat::ClampSinValue(int64_t angle, bool& flipHorizontal, bool& flipVertical) {
    // Clamp value to 0 - 2*PI using modulo
    int64_t clamped2Pi = angle % PI_TIMES_2;
    if (angle < 0) {
        clamped2Pi += PI_TIMES_2;
    }
    
    // The LUT contains values for 0 - PiOver2; every other value must be obtained by
    // vertical or horizontal mirroring
    flipVertical = clamped2Pi >= PI;
    
    // obtain (angle % PI) from (angle % 2PI)
    int64_t clampedPi = clamped2Pi;
    while (clampedPi >= PI) {
        clampedPi -= PI;
    }
    
    flipHorizontal = clampedPi >= PI_OVER_2;
    
    // obtain (angle % PI_OVER_2) from (angle % PI)
    int64_t clampedPiOver2 = clampedPi;
    if (clampedPiOver2 >= PI_OVER_2) {
        clampedPiOver2 -= PI_OVER_2;
    }
    
    return clampedPiOver2;
}

FixFloat FixFloat::Cos(const FixFloat& value) {
    // 确保表已经初始化
    if (_cosTable.empty()) {
        return Zero;
    }
    
    int angleInt = (value * Hundred).AsInt();
    angleInt = angleInt % (360 * 100);
    
    if (angleInt > 180 * 100) {
        angleInt -= 360 * 100;
    } else if (angleInt < -180 * 100) {
        angleInt += 360 * 100;
    }
    
    // -180
    if (angleInt == 180 * 100 || angleInt == -180 * 100) return -One;
    
    if (angleInt < 0) angleInt = -angleInt;
    
    if (angleInt <= 90 * 100) {
        return _cosTable[angleInt];
    }
    
    angleInt = 180 * 100 - angleInt;
    return -_cosTable[angleInt];
}

// 传入弧度，计算余弦值
FixFloat FixFloat::CosRad(const FixFloat& radians) {
    FixFloat degrees = radians * Rad2Deg;  // 先转换为角度
    return Cos(degrees);
}

FixFloat FixFloat::FastCos(const FixFloat& value) {
    int64_t xl = value.rawValue;
    int64_t rawAngle = xl + (xl > 0 ? -PI - PI_OVER_2 : PI_OVER_2);
    return FastSin(FixFloat(rawAngle));
}

FixFloat FixFloat::Atan(const FixFloat& z) {
    if (z.rawValue == 0) return Zero;
    
    // Force positive values for argument
    // Atan(-z) = -Atan(z).
    bool neg = z.rawValue < 0;
    FixFloat zValue = neg ? -z : z;
    
    FixFloat result;
    
    bool invert = zValue > One;
    if (invert) zValue = One / zValue;
    
    result = One;
    FixFloat term = One;
    
    FixFloat zSq = zValue * zValue;
    FixFloat zSq2 = zSq * Two;
    FixFloat zSqPlusOne = zSq + One;
    FixFloat zSq12 = zSqPlusOne * Two;
    FixFloat dividend = zSq2;
    FixFloat divisor = zSqPlusOne * Three;
    
    for (int i = 2; i < 30; ++i) {
        term = term * (dividend / divisor);
        result = result + term;
        
        dividend = dividend + zSq2;
        divisor = divisor + zSq12;
        
        if (term.rawValue == 0) break;
    }
    
    result = result * zValue / zSqPlusOne;
    
    if (invert) {
        result = PiOver2 - result;
    }
    
    if (neg) {
        result = -result;
    }
    
    return result;
}

FixFloat FixFloat::Atan2(const FixFloat& y, const FixFloat& x) {
    int64_t yl = y.rawValue;
    int64_t xl = x.rawValue;
    
    if (xl == 0) {
        if (yl > 0) {
            return PiOver2;
        }
        if (yl == 0) {
            return Zero;
        }
        return -PiOver2;
    }
    
    FixFloat atan;
    FixFloat z = y / x;
    
    FixFloat sm = EN2 * FixFloat(28);
    
    // Deal with overflow
    if (One + sm * z * z == MaxValue) {
        return y < Zero ? -PiOver2 : PiOver2;
    }
    
    if (Abs(z) < One) {
        atan = z / (One + sm * z * z);
        
        if (xl < 0) {
            if (yl < 0) {
                return atan - Pi;
            }
            return atan + Pi;
        }
    } else {
        atan = PiOver2 - z / (z * z + sm);
        
        if (yl < 0) {
            return atan - Pi;
        }
    }
    
    return atan;
}

FixFloat FixFloat::Asin(const FixFloat& value) {
    return PiOver2 - Acos(value);
}

FixFloat FixFloat::Acos(const FixFloat& x) {
    if (x < -One || x > One) {
        throw std::invalid_argument("Must between -FixFloat.One and FixFloat.One");
    }
    
    if (x.rawValue == 0) return PiOver2;
    
    FixFloat result = Atan(Sqrt(One - x * x) / x);
    return x.rawValue < 0 ? result + Pi : result;
}

// 角度转弧度
FixFloat FixFloat::Degree2Rad(const FixFloat& degrees) {
    return degrees * Deg2Rad;
}

// 弧度转角度
FixFloat FixFloat::Rad2Degree(const FixFloat& radians) {
    return radians * Rad2Deg;
}

// 实用函数
bool FixFloat::IsZero() const {
    return *this == Zero;
}

bool FixFloat::IsInfinity(const FixFloat& value) {
    return value == NegativeInfinity || value == PositiveInfinity;
}

bool FixFloat::IsNaN(const FixFloat& value) {
    return value == NaN;
}

// 运算符重载
FixFloat FixFloat::operator+(const FixFloat& other) const {
    return FixFloat(rawValue + other.rawValue);
}

FixFloat FixFloat::operator-(const FixFloat& other) const {
    return FixFloat(rawValue - other.rawValue);
}

FixFloat FixFloat::operator*(const FixFloat& other) const {
    int64_t xl = rawValue;
    int64_t yl = other.rawValue;
    
    uint64_t xlo = (uint64_t)(xl & 0x00000000FFFFFFFF);
    int64_t xhi = xl >> FRACTIONAL_PLACES;
    uint64_t ylo = (uint64_t)(yl & 0x00000000FFFFFFFF);
    int64_t yhi = yl >> FRACTIONAL_PLACES;
    
    uint64_t lolo = xlo * ylo;
    int64_t lohi = (int64_t)xlo * yhi;
    int64_t hilo = xhi * (int64_t)ylo;
    int64_t hihi = xhi * yhi;
    
    uint64_t loResult = lolo >> FRACTIONAL_PLACES;
    int64_t midResult1 = lohi;
    int64_t midResult2 = hilo;
    int64_t hiResult = hihi << FRACTIONAL_PLACES;
    
    int64_t sum = (int64_t)loResult + midResult1 + midResult2 + hiResult;
    return FixFloat(sum);
}

FixFloat FixFloat::operator/(const FixFloat& other) const {
    int64_t xl = rawValue;
    int64_t yl = other.rawValue;
    
    if (yl == 0) {
        return MAX_VALUE; // 避免除以零
    }
    
    uint64_t remainder = (uint64_t)(xl >= 0 ? xl : -xl);
    uint64_t divider = (uint64_t)(yl >= 0 ? yl : -yl);
    uint64_t quotient = 0;
    int bitPos = NUM_BITS / 2 + 1;
    
    // If the divider is divisible by 2^n, take advantage of it.
    while ((divider & 0xF) == 0 && bitPos >= 4) {
        divider >>= 4;
        bitPos -= 4;
    }
    
    while (remainder != 0 && bitPos >= 0) {
        int shift = CountLeadingZeroes(remainder);
        if (shift > bitPos) {
            shift = bitPos;
        }
        
        remainder <<= shift;
        bitPos -= shift;
        
        uint64_t div = remainder / divider;
        remainder = remainder % divider;
        quotient += div << bitPos;
        
        // Detect overflow
        if ((div & ~(0xFFFFFFFFFFFFFFFF >> bitPos)) != 0) {
            return ((xl ^ yl) & MIN_VALUE) == 0 ? MaxValue : MinValue;
        }
        
        remainder <<= 1;
        --bitPos;
    }
    
    // rounding
    ++quotient;
    int64_t result = (int64_t)(quotient >> 1);
    if (((xl ^ yl) & MIN_VALUE) != 0) {
        result = -result;
    }
    
    return FixFloat(result);
}

FixFloat FixFloat::operator%(const FixFloat& other) const {
    if (rawValue == MIN_VALUE && other.rawValue == -1) {
        return FixFloat(0);
    } else {
        return FixFloat(rawValue % other.rawValue);
    }
}

FixFloat FixFloat::operator-() const {
    if (rawValue == MIN_VALUE) {
        return MaxValue;
    } else {
        return FixFloat(-rawValue);
    }
}

FixFloat& FixFloat::operator+=(const FixFloat& other) {
    *this = *this + other;
    return *this;
}

FixFloat& FixFloat::operator-=(const FixFloat& other) {
    *this = *this - other;
    return *this;
}

FixFloat& FixFloat::operator*=(const FixFloat& other) {
    *this = *this * other;
    return *this;
}

FixFloat& FixFloat::operator/=(const FixFloat& other) {
    *this = *this / other;
    return *this;
}

FixFloat& FixFloat::operator%=(const FixFloat& other) {
    *this = *this % other;
    return *this;
}

bool FixFloat::operator==(const FixFloat& other) const {
    return rawValue == other.rawValue;
}

bool FixFloat::operator!=(const FixFloat& other) const {
    return rawValue != other.rawValue;
}

bool FixFloat::operator>(const FixFloat& other) const {
    return rawValue > other.rawValue;
}

bool FixFloat::operator<(const FixFloat& other) const {
    return rawValue < other.rawValue;
}

bool FixFloat::operator>=(const FixFloat& other) const {
    return rawValue >= other.rawValue;
}

bool FixFloat::operator<=(const FixFloat& other) const {
    return rawValue <= other.rawValue;
}

// FixFloat 与 int 的操作

FixFloat FixFloat::operator+(int other) const {
    return *this + FixFloat(other);
}

FixFloat FixFloat::operator-(int other) const {
    return *this - FixFloat(other);
}

FixFloat FixFloat::operator*(int other) const {
    // 对于整数乘法，我们可以优化性能
    return FixFloat(rawValue) * FixFloat(other);
}

FixFloat FixFloat::operator/(int other) const {
    // 对于整数除法，我们可以优化性能
    return FixFloat(rawValue) / FixFloat(other);
}

FixFloat FixFloat::operator%(int other) const {
    return *this % FixFloat(other);
}

FixFloat& FixFloat::operator+=(int other) {
    *this = *this + FixFloat(other);
    return *this;
}

FixFloat& FixFloat::operator-=(int other) {
    *this = *this - FixFloat(other);
    return *this;
}

FixFloat& FixFloat::operator*=(int other) {
    *this = *this * FixFloat(other);
    return *this;
}

FixFloat& FixFloat::operator/=(int other) {
    *this = *this / FixFloat(other);
    return *this;
}

FixFloat& FixFloat::operator%=(int other) {
    *this = *this % FixFloat(other);
    return *this;
}

bool FixFloat::operator==(int other) const {
    return *this == FixFloat(other);
}

bool FixFloat::operator!=(int other) const {
    return *this != FixFloat(other);
}

bool FixFloat::operator>(int other) const {
    return *this > FixFloat(other);
}

bool FixFloat::operator<(int other) const {
    return *this < FixFloat(other);
}

bool FixFloat::operator>=(int other) const {
    return *this >= FixFloat(other);
}

bool FixFloat::operator<=(int other) const {
    return *this <= FixFloat(other);
}

// FixFloat 与 float 的操作

FixFloat FixFloat::operator+(float other) const {
    return *this + FixFloat(other);
}

FixFloat FixFloat::operator-(float other) const {
    return *this - FixFloat(other);
}

FixFloat FixFloat::operator*(float other) const {
    return *this * FixFloat(other);
}

FixFloat FixFloat::operator/(float other) const {
    return *this / FixFloat(other);
}

FixFloat& FixFloat::operator+=(float other) {
    *this = *this + FixFloat(other);
    return *this;
}

FixFloat& FixFloat::operator-=(float other) {
    *this = *this - FixFloat(other);
    return *this;
}

FixFloat& FixFloat::operator*=(float other) {
    *this = *this * FixFloat(other);
    return *this;
}

FixFloat& FixFloat::operator/=(float other) {
    *this = *this / FixFloat(other);
    return *this;
}

bool FixFloat::operator==(float other) const {
    return *this == FixFloat(other);
}

bool FixFloat::operator!=(float other) const {
    return *this != FixFloat(other);
}

bool FixFloat::operator>(float other) const {
    return *this > FixFloat(other);
}

bool FixFloat::operator<(float other) const {
    return *this < FixFloat(other);
}

bool FixFloat::operator>=(float other) const {
    return *this >= FixFloat(other);
}

bool FixFloat::operator<=(float other) const {
    return *this <= FixFloat(other);
}

// FixFloat 与 double 的操作

FixFloat FixFloat::operator+(double other) const {
    return *this + FixFloat(other);
}

FixFloat FixFloat::operator-(double other) const {
    return *this - FixFloat(other);
}

FixFloat FixFloat::operator*(double other) const {
    return *this * FixFloat(other);
}

FixFloat FixFloat::operator/(double other) const {
    return *this / FixFloat(other);
}

FixFloat& FixFloat::operator+=(double other) {
    *this = *this + FixFloat(other);
    return *this;
}

FixFloat& FixFloat::operator-=(double other) {
    *this = *this - FixFloat(other);
    return *this;
}

FixFloat& FixFloat::operator*=(double other) {
    *this = *this * FixFloat(other);
    return *this;
}

FixFloat& FixFloat::operator/=(double other) {
    *this = *this / FixFloat(other);
    return *this;
}

bool FixFloat::operator==(double other) const {
    return *this == FixFloat(other);
}

bool FixFloat::operator!=(double other) const {
    return *this != FixFloat(other);
}

bool FixFloat::operator>(double other) const {
    return *this > FixFloat(other);
}

bool FixFloat::operator<(double other) const {
    return *this < FixFloat(other);
}

bool FixFloat::operator>=(double other) const {
    return *this >= FixFloat(other);
}

bool FixFloat::operator<=(double other) const {
    return *this <= FixFloat(other);
}

// 全局运算符重载实现

// int 与 FixFloat
FixFloat operator+(int a, const FixFloat& b) {
    return FixFloat(a) + b;
}

FixFloat operator-(int a, const FixFloat& b) {
    return FixFloat(a) - b;
}

FixFloat operator*(int a, const FixFloat& b) {
    return FixFloat(a) * b;
}

FixFloat operator/(int a, const FixFloat& b) {
    return FixFloat(a) / b;
}

FixFloat operator%(int a, const FixFloat& b) {
    return FixFloat(a) % b;
}

bool operator==(int a, const FixFloat& b) {
    return FixFloat(a) == b;
}

bool operator!=(int a, const FixFloat& b) {
    return FixFloat(a) != b;
}

bool operator>(int a, const FixFloat& b) {
    return FixFloat(a) > b;
}

bool operator<(int a, const FixFloat& b) {
    return FixFloat(a) < b;
}

bool operator>=(int a, const FixFloat& b) {
    return FixFloat(a) >= b;
}

bool operator<=(int a, const FixFloat& b) {
    return FixFloat(a) <= b;
}

// float 与 FixFloat
FixFloat operator+(float a, const FixFloat& b) {
    return FixFloat(a) + b;
}

FixFloat operator-(float a, const FixFloat& b) {
    return FixFloat(a) - b;
}

FixFloat operator*(float a, const FixFloat& b) {
    return FixFloat(a) * b;
}

FixFloat operator/(float a, const FixFloat& b) {
    return FixFloat(a) / b;
}

bool operator==(float a, const FixFloat& b) {
    return FixFloat(a) == b;
}

bool operator!=(float a, const FixFloat& b) {
    return FixFloat(a) != b;
}

bool operator>(float a, const FixFloat& b) {
    return FixFloat(a) > b;
}

bool operator<(float a, const FixFloat& b) {
    return FixFloat(a) < b;
}

bool operator>=(float a, const FixFloat& b) {
    return FixFloat(a) >= b;
}

bool operator<=(float a, const FixFloat& b) {
    return FixFloat(a) <= b;
}

// double 与 FixFloat
FixFloat operator+(double a, const FixFloat& b) {
    return FixFloat(a) + b;
}

FixFloat operator-(double a, const FixFloat& b) {
    return FixFloat(a) - b;
}

FixFloat operator*(double a, const FixFloat& b) {
    return FixFloat(a) * b;
}

FixFloat operator/(double a, const FixFloat& b) {
    return FixFloat(a) / b;
}

bool operator==(double a, const FixFloat& b) {
    return FixFloat(a) == b;
}

bool operator!=(double a, const FixFloat& b) {
    return FixFloat(a) != b;
}

bool operator>(double a, const FixFloat& b) {
    return FixFloat(a) > b;
}

bool operator<(double a, const FixFloat& b) {
    return FixFloat(a) < b;
}

bool operator>=(double a, const FixFloat& b) {
    return FixFloat(a) >= b;
}

bool operator<=(double a, const FixFloat& b) {
    return FixFloat(a) <= b;
}

// 位运算操作符实现
FixFloat FixFloat::operator~() const {
    return FixFloat(~rawValue);
}

FixFloat FixFloat::operator&(const FixFloat& other) const {
    return FixFloat(rawValue & other.rawValue);
}

FixFloat FixFloat::operator|(const FixFloat& other) const {
    return FixFloat(rawValue | other.rawValue);
}

FixFloat FixFloat::operator^(const FixFloat& other) const {
    return FixFloat(rawValue ^ other.rawValue);
}

FixFloat FixFloat::operator<<(int shift) const {
    return FixFloat(rawValue << shift);
}

FixFloat FixFloat::operator>>(int shift) const {
    return FixFloat(rawValue >> shift);
}

// 复合赋值位运算操作符实现
FixFloat& FixFloat::operator&=(const FixFloat& other) {
    rawValue &= other.rawValue;
    return *this;
}

FixFloat& FixFloat::operator|=(const FixFloat& other) {
    rawValue |= other.rawValue;
    return *this;
}

FixFloat& FixFloat::operator^=(const FixFloat& other) {
    rawValue ^= other.rawValue;
    return *this;
}

FixFloat& FixFloat::operator<<=(int shift) {
    rawValue <<= shift;
    return *this;
}

FixFloat& FixFloat::operator>>=(int shift) {
    rawValue >>= shift;
    return *this;
}

// 字符串转换
const char* FixFloat::ToString() const {
    static char buffer[32];
    printf(buffer, "%f", AsFloat());
    return buffer;
}

