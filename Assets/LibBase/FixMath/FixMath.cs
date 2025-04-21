using System;
using Unity.Collections;

/* Copyright (C) <2009-2011> <Thorben Linneweber, Jitter Physics>
* 
*  This software is provided 'as-is', without any express or implied
*  warranty.  In no event will the authors be held liable for any damages
*  arising from the use of this software.
*
*  Permission is granted to anyone to use this software for any purpose,
*  including commercial applications, and to alter it and redistribute it
*  freely, subject to the following restrictions:
*
*  1. The origin of this software must not be misrepresented; you must not
*      claim that you wrote the original software. If you use this software
*      in a product, an acknowledgment in the product documentation would be
*      appreciated but is not required.
*  2. Altered source versions must be plainly marked as such, and must not be
*      misrepresented as being the original software.
*  3. This notice may not be removed or altered from any source distribution. 
*/

namespace LibBase.MathLite.FixMath
{

    /// <summary>
    /// Contains common math operations.
    /// </summary>
    public static partial class FixMath
    {

        /// <summary>
        /// PI constant.
        /// </summary>
        public static FixFloat PI => FixFloat.Pi;

        /**
        *  @brief PI over 2 constant.
        **/
        public static FixFloat PiOver2 => FixFloat.PiOver2;

        /// <summary>
        /// A small value often used to decide if numeric 
        /// results are zero.
        /// </summary>
		public static FixFloat Epsilon => FixFloat.Epsilon;

        /**
        *  @brief Degree to radians constant.
        **/
        public static FixFloat Deg2Rad => FixFloat.Deg2Rad;

        /**
        *  @brief Radians to degree constant.
        **/
        public static FixFloat Rad2Deg => FixFloat.Rad2Deg;


        /**
         * @brief FixFloat infinity.
         * */
        public static FixFloat Infinity => FixFloat.MaxValue;
        

        /// <summary>
        /// Gets the square root.
        /// </summary>
        /// <param name="number">The number to get the square root from.</param>
        /// <returns></returns>
        #region public static FixFloat Sqrt(FixFloat number)
        public static FixFloat Sqrt(FixFloat number)
        {
            return FixFloat.Sqrt(number);
        }
        #endregion

        /// <summary>
        /// Gets the maximum number of two values.
        /// </summary>
        /// <param name="val1">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <returns>Returns the largest value.</returns>
        #region public static FixFloat Max(FixFloat val1, FixFloat val2)
        public static FixFloat Max(FixFloat val1, FixFloat val2)
        {
            return (val1 > val2) ? val1 : val2;
        }
        #endregion

        /// <summary>
        /// Gets the minimum number of two values.
        /// </summary>
        /// <param name="val1">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <returns>Returns the smallest value.</returns>
        #region public static FixFloat Min(FixFloat val1, FixFloat val2)
        public static FixFloat Min(FixFloat val1, FixFloat val2)
        {
            return (val1 < val2) ? val1 : val2;
        }
        public static FixFloat Min(FixFloat val1, FixFloat val2, FixFloat val3)
        {
            FixFloat min12 = (val1 < val2) ? val1 : val2;
            return (min12 < val3) ? min12 : val3;
        }
        #endregion

        /// <summary>
        /// Gets the maximum number of three values.
        /// </summary>
        /// <param name="val1">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <param name="val3">The third value.</param>
        /// <returns>Returns the largest value.</returns>
        #region public static FixFloat Max(FixFloat val1, FixFloat val2,FixFloat val3)
        public static FixFloat Max(FixFloat val1, FixFloat val2, FixFloat val3)
        {
            FixFloat max12 = (val1 > val2) ? val1 : val2;
            return (max12 > val3) ? max12 : val3;
        }
        #endregion

        /// <summary>
        /// Returns a number which is within [min,max]
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        #region public static FixFloat Clamp(FixFloat value, FixFloat min, FixFloat max)
        public static FixFloat Clamp(FixFloat value, FixFloat min, FixFloat max)
        {
            if (value < min)
            {
                value = min;
                return value;
            }
            if (value > max)
            {
                value = max;
            }
            return value;
        }
        #endregion

        /// <summary>
        /// Returns a number which is within [FixFloat.Zero, FixFloat.One]
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The clamped value.</returns>
        public static FixFloat Clamp01(FixFloat value)
        {
            if (value < FixFloat.Zero)
                return FixFloat.Zero;

            if (value > FixFloat.One)
                return FixFloat.One;

            return value;
        }

        /// <summary>
        /// Changes every sign of the matrix entry to '+'
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="result">The absolute matrix.</param>
        #region public static void Absolute(ref JMatrix matrix,out JMatrix result)
        public static void Absolute(ref FixMatrix matrix, out FixMatrix result)
        {
            result.M11 = FixFloat.Abs(matrix.M11);
            result.M12 = FixFloat.Abs(matrix.M12);
            result.M13 = FixFloat.Abs(matrix.M13);
            result.M21 = FixFloat.Abs(matrix.M21);
            result.M22 = FixFloat.Abs(matrix.M22);
            result.M23 = FixFloat.Abs(matrix.M23);
            result.M31 = FixFloat.Abs(matrix.M31);
            result.M32 = FixFloat.Abs(matrix.M32);
            result.M33 = FixFloat.Abs(matrix.M33);
        }
        #endregion
        
        
        public static FixFloat Sin(FixFloat tAngle, NativeList<FixFloat> sinTable)
        {
            int angleInt = (tAngle * FixFloat.Hundred).AsInt();
            angleInt %= 360 * 100;
            if (angleInt > 180 * 100)
            {
                angleInt -= 360 * 100;
            }
            else if (angleInt < -180 * 100)
            {
                angleInt += 360 * 100;
            }

            // -180
            if (angleInt == -180 * 100 || angleInt == 180 * 100) return FixFloat.Zero;
            bool isOverZero = true;
            if (angleInt < 0)
            {
                isOverZero = false;
                angleInt = -angleInt;
            }

            if (angleInt <= 90 * 100) return isOverZero ? sinTable[angleInt] : -sinTable[angleInt];
            angleInt = 180 * 100 - angleInt;
            return isOverZero ? sinTable[angleInt] : -sinTable[angleInt];
        }
        
        public static FixFloat Cos(FixFloat tAngle, NativeList<FixFloat> cosTable)
        {
            int angleInt = (tAngle * FixFloat.Hundred).AsInt();
            angleInt %= 360 * 100;
            if (angleInt > 180 * 100)
            {
                angleInt -= 360 * 100;
            }
            else if (angleInt < -180 * 100)
            {
                angleInt += 360 * 100;
            }
            
            // -180
            if (angleInt == 180 * 100 || angleInt == -180 * 100) return -FixFloat.One;
            if (angleInt < 0) angleInt = -angleInt;
            if (angleInt <= 90 * 100) return cosTable[angleInt];
            angleInt = 180 * 100 - angleInt;
            return -cosTable[angleInt];
        }

        // /// <summary>
        // /// Returns the tan of value.
        // /// </summary>
        // public static FixFloat Tan(FixFloat value)
        // {
        //     return FixFloat.Tan(value * Deg2Rad);
        // }

        /// <summary>
        /// Returns the arc sine of value.
        /// </summary>
        public static FixFloat Asin(FixFloat value)
        {
            return FixFloat.Asin(value * Deg2Rad);
        }

        /// <summary>
        /// Returns the arc cosine of value.
        /// </summary>
        public static FixFloat Acos(FixFloat value)
        {
            return FixFloat.Acos(value * Deg2Rad);
        }

        /// <summary>
        /// Returns the arc tan of value.
        /// </summary>
        public static FixFloat Atan(FixFloat value)
        {
            return FixFloat.Atan(value * Deg2Rad);
        }

        /// <summary>
        /// Returns the arc tan of coordinates x-y.
        /// </summary>
        public static FixFloat Atan2(FixFloat y, FixFloat x)
        {
            return FixFloat.Atan2(y, x);
        }

        /// <summary>
        /// Returns the largest integer less than or equal to the specified number.
        /// </summary>
        public static FixFloat Floor(FixFloat value)
        {
            return FixFloat.Floor(value);
        }

        /// <summary>
        /// Returns the smallest integral value that is greater than or equal to the specified number.
        /// </summary>
        public static FixFloat Ceiling(FixFloat value)
        {
            return value;
        }

        /// <summary>
        /// Rounds a value to the nearest integral value.
        /// If the value is halfway between an even and an uneven value, returns the even value.
        /// </summary>
        public static FixFloat Round(FixFloat value)
        {
            return FixFloat.Round(value);
        }

        /// <summary>
        /// Returns a number indicating the sign of a Fix64 number.
        /// Returns 1 if the value is positive, 0 if is 0, and -1 if it is negative.
        /// </summary>
        public static int Sign(FixFloat value)
        {
            return FixFloat.Sign(value);
        }

        /// <summary>
        /// Returns the absolute value of a Fix64 number.
        /// Note: Abs(Fix64.MinValue) == Fix64.MaxValue.
        /// </summary>
        public static FixFloat Abs(FixFloat value)
        {
            return FixFloat.Abs(value);
        }

        public static FixFloat Barycentric(FixFloat value1, FixFloat value2, FixFloat value3, FixFloat amount1, FixFloat amount2)
        {
            return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
        }

        public static FixFloat CatmullRom(FixFloat value1, FixFloat value2, FixFloat value3, FixFloat value4, FixFloat amount)
        {
            // Using formula from http://www.mvps.org/directx/articles/catmull/
            // Internally using FixFloats not to lose precission
            FixFloat amountSquared = amount * amount;
            FixFloat amountCubed = amountSquared * amount;
            return (FixFloat)(0.5 * (2.0 * value2 +
                                 (value3 - value1) * amount +
                                 (2.0 * value1 - 5.0 * value2 + 4.0 * value3 - value4) * amountSquared +
                                 (3.0 * value2 - value1 - 3.0 * value3 + value4) * amountCubed));
        }

        public static FixFloat Distance(FixFloat value1, FixFloat value2)
        {
            return FixFloat.Abs(value1 - value2);
        }

        public static FixFloat Hermite(FixFloat value1, FixFloat tangent1, FixFloat value2, FixFloat tangent2, FixFloat amount)
        {
            // All transformed to FixFloat not to lose precission
            // Otherwise, for high numbers of param:amount the result is NaN instead of Infinity
            FixFloat v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount, result;
            FixFloat sCubed = s * s * s;
            FixFloat sSquared = s * s;

            if (amount == 0f)
                result = value1;
            else if (amount == 1f)
                result = value2;
            else
                result = (2 * v1 - 2 * v2 + t2 + t1) * sCubed +
                         (3 * v2 - 3 * v1 - 2 * t1 - t2) * sSquared +
                         t1 * s +
                         v1;
            return (FixFloat)result;
        }

        public static FixFloat Lerp(FixFloat value1, FixFloat value2, FixFloat amount)
        {
            return value1 + (value2 - value1) * Clamp01(amount);
        }
        
        public static FVec3 Lerp(FVec3 a, FVec3 b, FixFloat f)
        {
            return new FVec3(Lerp(a.x, b.x, f), Lerp(a.y, b.y, f), Lerp(a.z, b.z, f));
        }

        public static FixFloat InverseLerp(FixFloat value1, FixFloat value2, FixFloat amount)
        {
            if (value1 != value2)
                return Clamp01((amount - value1) / (value2 - value1));
            return FixFloat.Zero;
        }

        public static FixFloat SmoothStep(FixFloat value1, FixFloat value2, FixFloat amount)
        {
            // It is expected that 0 < amount < 1
            // If amount < 0, return value1
            // If amount > 1, return value2
            FixFloat result = Clamp(amount, 0f, 1f);
            result = Hermite(value1, 0f, value2, 0f, result);
            return result;
        }


        /// <summary>
        /// Returns 2 raised to the specified power.
        /// Provides at least 6 decimals of accuracy.
        /// </summary>
        public static FixFloat Pow2(FixFloat x)
        {
            if (x.RawValue == 0)
            {
                return FixFloat.One;
            }

            // Avoid negative arguments by exploiting that exp(-x) = 1/exp(x).
            bool neg = x.RawValue < 0;
            if (neg)
            {
                x = -x;
            }

            if (x == FixFloat.One)
            {
                return neg ? FixFloat.One / (FixFloat)2 : (FixFloat)2;
            }
            if (x >= FixFloat.Log2Max)
            {
                return neg ? FixFloat.One / FixFloat.MaxValue : FixFloat.MaxValue;
            }
            if (x <= FixFloat.Log2Min)
            {
                return neg ? FixFloat.MaxValue : FixFloat.Zero;
            }

            /* The algorithm is based on the power series for exp(x):
             * http://en.wikipedia.org/wiki/Exponential_function#Formal_definition
             * 
             * From term n, we get term n+1 by multiplying with x/n.
             * When the sum term drops to zero, we can stop summing.
             */

            int integerPart = (int)Floor(x);
            // Take fractional part of exponent
            x = FixFloat.FromRaw(x.RawValue & 0x00000000FFFFFFFF);

            var result = FixFloat.One;
            var term = FixFloat.One;
            int i = 1;
            while (term.RawValue != 0)
            {
                term = FixFloat.FastMul(FixFloat.FastMul(x, term), FixFloat.Ln2) / (FixFloat)i;
                result += term;
                i++;
            }

            result = FixFloat.FromRaw(result.RawValue << integerPart);
            if (neg)
            {
                result = FixFloat.One / result;
            }

            return result;
        }

        /// <summary>
        /// Returns the base-2 logarithm of a specified number.
        /// Provides at least 9 decimals of accuracy.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The argument was non-positive
        /// </exception>
        internal static FixFloat Log2(FixFloat x)
        {
            if (x.RawValue <= 0)
            {
                throw new ArgumentOutOfRangeException("Non-positive value passed to Ln", "x");
            }

            // This implementation is based on Clay. S. Turner's fast binary logarithm
            // algorithm (C. S. Turner,  "A Fast Binary Logarithm Algorithm", IEEE Signal
            //     Processing Mag., pp. 124,140, Sep. 2010.)

            long b = 1U << (FixFloat.FRACTIONAL_PLACES - 1);
            long y = 0;

            long rawX = x.RawValue;
            while (rawX < FixFloat.ONE)
            {
                rawX <<= 1;
                y -= FixFloat.ONE;
            }

            while (rawX >= (FixFloat.ONE << 1))
            {
                rawX >>= 1;
                y += FixFloat.ONE;
            }

            var z = FixFloat.FromRaw(rawX);

            for (int i = 0; i < FixFloat.FRACTIONAL_PLACES; i++)
            {
                z = FixFloat.FastMul(z, z);
                if (z.RawValue >= (FixFloat.ONE << 1))
                {
                    z = FixFloat.FromRaw(z.RawValue >> 1);
                    y += b;
                }
                b >>= 1;
            }

            return FixFloat.FromRaw(y);
        }

        /// <summary>
        /// Returns the natural logarithm of a specified number.
        /// Provides at least 7 decimals of accuracy.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The argument was non-positive
        /// </exception>
        public static FixFloat Ln(FixFloat x)
        {
            return FixFloat.FastMul(Log2(x), FixFloat.Ln2);
        }

        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// Provides about 5 digits of accuracy for the result.
        /// </summary>
        /// <exception cref="DivideByZeroException">
        /// The base was zero, with a negative exponent
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The base was negative, with a non-zero exponent
        /// </exception>
        public static FixFloat Pow(FixFloat b, FixFloat exp)
        {
            if (b == FixFloat.One)
            {
                return FixFloat.One;
            }

            if (exp.RawValue == 0)
            {
                return FixFloat.One;
            }

            if (b.RawValue == 0)
            {
                if (exp.RawValue < 0)
                {
                    //throw new DivideByZeroException();
                    return FixFloat.MaxValue;
                }
                return FixFloat.Zero;
            }

            FixFloat log2 = Log2(b);
            return Pow2(exp * log2);
        }

        public static FixFloat MoveTowards(FixFloat current, FixFloat target, FixFloat maxDelta)
        {
            if (Abs(target - current) <= maxDelta)
                return target;
            return (current + (Sign(target - current)) * maxDelta);
        }

        public static FixFloat Repeat(FixFloat t, FixFloat length)
        {
            return (t - (Floor(t / length) * length));
        }

        public static FixFloat DeltaAngle(FixFloat current, FixFloat target)
        {
            FixFloat num = Repeat(target - current, (FixFloat)360f);
            if (num > (FixFloat)180f)
            {
                num -= (FixFloat)360f;
            }
            return num;
        }

        public static FixFloat MoveTowardsAngle(FixFloat current, FixFloat target, float maxDelta)
        {
            target = current + DeltaAngle(current, target);
            return MoveTowards(current, target, maxDelta);
        }

        public static FixFloat SmoothDamp(FixFloat current, FixFloat target, ref FixFloat currentVelocity, FixFloat smoothTime, FixFloat maxSpeed)
        {
            FixFloat deltaTime = FixFloat.EN2;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static FixFloat SmoothDamp(FixFloat current, FixFloat target, ref FixFloat currentVelocity, FixFloat smoothTime)
        {
            FixFloat deltaTime = FixFloat.EN2;
            FixFloat positiveInfinity = -FixFloat.MaxValue;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, positiveInfinity, deltaTime);
        }

        public static FixFloat SmoothDamp(FixFloat current, FixFloat target, ref FixFloat currentVelocity, FixFloat smoothTime, FixFloat maxSpeed, FixFloat deltaTime)
        {
            smoothTime = Max(FixFloat.EN4, smoothTime);
            FixFloat num = (FixFloat)2f / smoothTime;
            FixFloat num2 = num * deltaTime;
            FixFloat num3 = FixFloat.One / (((FixFloat.One + num2) + (((FixFloat)0.48f * num2) * num2)) + ((((FixFloat)0.235f * num2) * num2) * num2));
            FixFloat num4 = current - target;
            FixFloat num5 = target;
            FixFloat max = maxSpeed * smoothTime;
            num4 = Clamp(num4, -max, max);
            target = current - num4;
            FixFloat num7 = (currentVelocity + (num * num4)) * deltaTime;
            currentVelocity = (currentVelocity - (num * num7)) * num3;
            FixFloat num8 = target + ((num4 + num7) * num3);
            if (((num5 - current) > FixFloat.Zero) == (num8 > num5))
            {
                num8 = num5;
                currentVelocity = (num8 - num5) / deltaTime;
            }
            return num8;
        }

        public static FixFloat ForceRange(FixFloat min, FixFloat max, FixFloat value)
        {
            if (value < min) value = min;
            if (value > max) value = max;
            return value;
        }
    }
}