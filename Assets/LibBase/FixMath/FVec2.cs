#region License

/*
MIT License
Copyright © 2006 The Mono.Xna Team

All rights reserved.

Authors
 * Alan McGovern

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

#endregion License

using System;
using UnityEngine;

namespace LibBase.MathLite.FixMath {

    [Serializable]
    public struct FVec2 : IEquatable<FVec2>
    {
#region Private Fields

        private static FVec2 zeroVector => new FVec2(0, 0);
        private static FVec2 oneVector => new FVec2(1, 1);

        private static FVec2 rightVector => new FVec2(1, 0);
        private static FVec2 leftVector => new FVec2(-1, 0);

        private static FVec2 upVector => new FVec2(0, 1);
        private static FVec2 downVector => new FVec2(0, -1);

        #endregion Private Fields

        #region Public Fields

        [SerializeField]
        public FixFloat x;
        [SerializeField]
        public FixFloat y;

        public FixFloat this[int key]
        {
            get { return (key == 0) ? x : y; }
            set
            {
                if (key == 0)
                    x = value;
                else
                    y = value;
            }
        }
        
        #endregion Public Fields

#region Properties

        public static FVec2 zero
        {
            get { return zeroVector; }
        }

        public static FVec2 one
        {
            get { return oneVector; }
        }

        public static FVec2 right
        {
            get { return rightVector; }
        }

        public static FVec2 left {
            get { return leftVector; }
        }

        public static FVec2 up
        {
            get { return upVector; }
        }

        public static FVec2 down {
            get { return downVector; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Constructor foe standard 2D vector.
        /// </summary>
        /// <param name="x">
        /// A <see cref="System.Single"/>
        /// </param>
        /// <param name="y">
        /// A <see cref="System.Single"/>
        /// </param>
        public FVec2(FixFloat x, FixFloat y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Constructor for "square" vector.
        /// </summary>
        /// <param name="value">
        /// A <see cref="System.Single"/>
        /// </param>
        public FVec2(FixFloat value)
        {
            x = value;
            y = value;
        }

        public void Set(FixFloat x, FixFloat y) {
            this.x = x;
            this.y = y;
        }

        #endregion Constructors

        #region Public Methods

        public static void Reflect(ref FVec2 vector, ref FVec2 normal, out FVec2 result)
        {
            FixFloat dot = Dot(vector, normal);
            result.x = vector.x - ((2f*dot)*normal.x);
            result.y = vector.y - ((2f*dot)*normal.y);
        }

        public static FVec2 Reflect(FVec2 vector, FVec2 normal)
        {
            FVec2 result;
            Reflect(ref vector, ref normal, out result);
            return result;
        }

        public static FVec2 Add(FVec2 value1, FVec2 value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }

        public static void Add(ref FVec2 value1, ref FVec2 value2, out FVec2 result)
        {
            result.x = value1.x + value2.x;
            result.y = value1.y + value2.y;
        }

        public static FVec2 Barycentric(FVec2 value1, FVec2 value2, FVec2 value3, FixFloat amount1, FixFloat amount2)
        {
            return new FVec2(
                FixMath.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
                FixMath.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
        }

        public static void Barycentric(ref FVec2 value1, ref FVec2 value2, ref FVec2 value3, FixFloat amount1,
                                       FixFloat amount2, out FVec2 result)
        {
            result = new FVec2(
                FixMath.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
                FixMath.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
        }

        public static FVec2 CatmullRom(FVec2 value1, FVec2 value2, FVec2 value3, FVec2 value4, FixFloat amount)
        {
            return new FVec2(
                FixMath.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
                FixMath.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
        }

        public static void CatmullRom(ref FVec2 value1, ref FVec2 value2, ref FVec2 value3, ref FVec2 value4,
                                      FixFloat amount, out FVec2 result)
        {
            result = new FVec2(
                FixMath.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
                FixMath.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
        }

        public static FVec2 Clamp(FVec2 value1, FVec2 min, FVec2 max)
        {
            return new FVec2(
                FixMath.Clamp(value1.x, min.x, max.x),
                FixMath.Clamp(value1.y, min.y, max.y));
        }

        public static void Clamp(ref FVec2 value1, ref FVec2 min, ref FVec2 max, out FVec2 result)
        {
            result = new FVec2(
                FixMath.Clamp(value1.x, min.x, max.x),
                FixMath.Clamp(value1.y, min.y, max.y));
        }

        /// <summary>
        /// Returns FixFloat precison distanve between two vectors
        /// </summary>
        /// <param name="value1">
        /// A <see cref="FVec2"/>
        /// </param>
        /// <param name="value2">
        /// A <see cref="FVec2"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Single"/>
        /// </returns>
        public static FixFloat Distance(FVec2 value1, FVec2 value2)
        {
            FixFloat result;
            DistanceSquared(ref value1, ref value2, out result);
            return (FixFloat) FixFloat.Sqrt(result);
        }


        public static void Distance(ref FVec2 value1, ref FVec2 value2, out FixFloat result)
        {
            DistanceSquared(ref value1, ref value2, out result);
            result = (FixFloat) FixFloat.Sqrt(result);
        }

        public static FixFloat DistanceSquared(FVec2 value1, FVec2 value2)
        {
            FixFloat result;
            DistanceSquared(ref value1, ref value2, out result);
            return result;
        }

        public static void DistanceSquared(ref FVec2 value1, ref FVec2 value2, out FixFloat result)
        {
            result = (value1.x - value2.x)*(value1.x - value2.x) + (value1.y - value2.y)*(value1.y - value2.y);
        }

        public Vector2 Vector2()
        {
            return new Vector2(x, y);
        }
        
        /// <summary>
        /// Devide first vector with the secund vector
        /// </summary>
        /// <param name="value1">
        /// A <see cref="FVec2"/>
        /// </param>
        /// <param name="value2">
        /// A <see cref="FVec2"/>
        /// </param>
        /// <returns>
        /// A <see cref="FVec2"/>
        /// </returns>
        public static FVec2 Divide(FVec2 value1, FVec2 value2)
        {
            value1.x /= value2.x;
            value1.y /= value2.y;
            return value1;
        }

        public static void Divide(ref FVec2 value1, ref FVec2 value2, out FVec2 result)
        {
            result.x = value1.x/value2.x;
            result.y = value1.y/value2.y;
        }

        public static FVec2 Divide(FVec2 value1, FixFloat divider)
        {
            FixFloat factor = 1/divider;
            value1.x *= factor;
            value1.y *= factor;
            return value1;
        }

        public static void Divide(ref FVec2 value1, FixFloat divider, out FVec2 result)
        {
            FixFloat factor = 1/divider;
            result.x = value1.x*factor;
            result.y = value1.y*factor;
        }

        public static FixFloat Dot(FVec2 value1, FVec2 value2)
        {
            return value1.x*value2.x + value1.y*value2.y;
        }

        public static void Dot(ref FVec2 value1, ref FVec2 value2, out FixFloat result)
        {
            result = value1.x*value2.x + value1.y*value2.y;
        }

        public override bool Equals(object obj)
        {
            return (obj is FVec2) ? this == ((FVec2) obj) : false;
        }

        public bool Equals(FVec2 other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return (int) (x + y);
        }

        public static FVec2 Hermite(FVec2 value1, FVec2 tangent1, FVec2 value2, FVec2 tangent2, FixFloat amount)
        {
            FVec2 result = new FVec2();
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
            return result;
        }

        public static void Hermite(ref FVec2 value1, ref FVec2 tangent1, ref FVec2 value2, ref FVec2 tangent2,
                                   FixFloat amount, out FVec2 result)
        {
            result.x = FixMath.Hermite(value1.x, tangent1.x, value2.x, tangent2.x, amount);
            result.y = FixMath.Hermite(value1.y, tangent1.y, value2.y, tangent2.y, amount);
        }

        public FixFloat magnitude {
            get {
                FixFloat result;
                FVec2 zero = zeroVector;
                DistanceSquared(ref this, ref zero, out result);
                return FixFloat.Sqrt(result);
            }
        }

        public static FVec2 ClampMagnitude(FVec2 vector, FixFloat maxLength) {
            return Normalize(vector) * maxLength;
        }

        public FixFloat LengthSquared()
        {
            FixFloat result;
            FVec2 zero = zeroVector;
            DistanceSquared(ref this, ref zero, out result);
            return result;
        }

        public static FVec2 Lerp(FVec2 value1, FVec2 value2, FixFloat amount)
        {
            amount = FixMath.Clamp(amount, FixFloat.Zero, FixFloat.One);

            return new FVec2(
                FixMath.Lerp(value1.x, value2.x, amount),
                FixMath.Lerp(value1.y, value2.y, amount));
        }

        public static FVec2 LerpUnclamped(FVec2 value1, FVec2 value2, FixFloat amount)
        {
            return new FVec2(
                FixMath.Lerp(value1.x, value2.x, amount),
                FixMath.Lerp(value1.y, value2.y, amount));
        }

        public static void LerpUnclamped(ref FVec2 value1, ref FVec2 value2, FixFloat amount, out FVec2 result)
        {
            result = new FVec2(
                FixMath.Lerp(value1.x, value2.x, amount),
                FixMath.Lerp(value1.y, value2.y, amount));
        }

        public static FVec2 Max(FVec2 value1, FVec2 value2)
        {
            return new FVec2(
                FixMath.Max(value1.x, value2.x),
                FixMath.Max(value1.y, value2.y));
        }

        public static void Max(ref FVec2 value1, ref FVec2 value2, out FVec2 result)
        {
            result.x = FixMath.Max(value1.x, value2.x);
            result.y = FixMath.Max(value1.y, value2.y);
        }

        public static FVec2 Min(FVec2 value1, FVec2 value2)
        {
            return new FVec2(
                FixMath.Min(value1.x, value2.x),
                FixMath.Min(value1.y, value2.y));
        }

        public static void Min(ref FVec2 value1, ref FVec2 value2, out FVec2 result)
        {
            result.x = FixMath.Min(value1.x, value2.x);
            result.y = FixMath.Min(value1.y, value2.y);
        }

        public void Scale(FVec2 other) {
            this.x = x * other.x;
            this.y = y * other.y;
        }

        public static FVec2 Scale(FVec2 value1, FVec2 value2) {
            FVec2 result;
            result.x = value1.x * value2.x;
            result.y = value1.y * value2.y;

            return result;
        }

        public static FVec2 Multiply(FVec2 value1, FVec2 value2)
        {
            value1.x *= value2.x;
            value1.y *= value2.y;
            return value1;
        }

        public static FVec2 Multiply(FVec2 value1, FixFloat scaleFactor)
        {
            value1.x *= scaleFactor;
            value1.y *= scaleFactor;
            return value1;
        }

        public static void Multiply(ref FVec2 value1, FixFloat scaleFactor, out FVec2 result)
        {
            result.x = value1.x*scaleFactor;
            result.y = value1.y*scaleFactor;
        }

        public static void Multiply(ref FVec2 value1, ref FVec2 value2, out FVec2 result)
        {
            result.x = value1.x*value2.x;
            result.y = value1.y*value2.y;
        }

        public static FVec2 Negate(FVec2 value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }

        public static void Negate(ref FVec2 value, out FVec2 result)
        {
            result.x = -value.x;
            result.y = -value.y;
        }

        public void Normalize()
        {
            Normalize(ref this, out this);
        }

        public static FVec2 Normalize(FVec2 value)
        {
            Normalize(ref value, out value);
            return value;
        }

        public FVec2 normalized {
            get {
                FVec2 result;
                FVec2.Normalize(ref this, out result);

                return result;
            }
        }

        public static void Normalize(ref FVec2 value, out FVec2 result)
        {
            FixFloat factor;
            FVec2 zero = zeroVector;
            DistanceSquared(ref value, ref zero, out factor);
            factor = 1f/(FixFloat) FixFloat.Sqrt(factor);
            result.x = value.x*factor;
            result.y = value.y*factor;
        }

        public static FVec2 SmoothStep(FVec2 value1, FVec2 value2, FixFloat amount)
        {
            return new FVec2(
                FixMath.SmoothStep(value1.x, value2.x, amount),
                FixMath.SmoothStep(value1.y, value2.y, amount));
        }

        public static void SmoothStep(ref FVec2 value1, ref FVec2 value2, FixFloat amount, out FVec2 result)
        {
            result = new FVec2(
                FixMath.SmoothStep(value1.x, value2.x, amount),
                FixMath.SmoothStep(value1.y, value2.y, amount));
        }

        public static FVec2 Subtract(FVec2 value1, FVec2 value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }

        public static void Subtract(ref FVec2 value1, ref FVec2 value2, out FVec2 result)
        {
            result.x = value1.x - value2.x;
            result.y = value1.y - value2.y;
        }

        public static FixFloat Angle(FVec2 a, FVec2 b) {
            return FixFloat.Acos(a.normalized * b.normalized) * FixFloat.Rad2Deg;
        }

        public FVec3 ToTSVector() {
            return new FVec3(this.x, this.y, FixFloat.Zero);
        }

        public override string ToString() {
            return string.Format("({0:f1}, {1:f1})", x.AsFloat(), y.AsFloat());
        }

        #endregion Public Methods

#region Operators

        public static FVec2 operator -(FVec2 value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }


        public static bool operator ==(FVec2 value1, FVec2 value2)
        {
            return value1.x == value2.x && value1.y == value2.y;
        }


        public static bool operator !=(FVec2 value1, FVec2 value2)
        {
            return value1.x != value2.x || value1.y != value2.y;
        }


        public static FVec2 operator +(FVec2 value1, FVec2 value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }


        public static FVec2 operator -(FVec2 value1, FVec2 value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }


        public static FixFloat operator *(FVec2 value1, FVec2 value2)
        {
            return FVec2.Dot(value1, value2);
        }


        public static FVec2 operator *(FVec2 value, FixFloat scaleFactor)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }


        public static FVec2 operator *(FixFloat scaleFactor, FVec2 value)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }


        public static FVec2 operator /(FVec2 value1, FVec2 value2)
        {
            value1.x /= value2.x;
            value1.y /= value2.y;
            return value1;
        }


        public static FVec2 operator /(FVec2 value1, FixFloat divider)
        {
            FixFloat factor = 1/divider;
            value1.x *= factor;
            value1.y *= factor;
            return value1;
        }
        
        public static implicit operator FVec2(FVec3 v)
        {
            return new FVec2(v.x, v.y);
        }

        public static implicit operator FVec3(FVec2 v)
        {
            return new FVec3(v.x, v.y, FixFloat.Zero);
        }
        
        #endregion Operators

        public FixFloat rawSqrMagnitude
        {
            get
            {
                FixFloat x = this.x;
                FixFloat y = this.y;
                return (x * x + y * y);
            }
        }
    }
}