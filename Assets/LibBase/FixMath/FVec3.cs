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

using System;
using UnityEngine;

namespace LibBase.MathLite.FixMath
{
    /// <summary>
    /// A vector structure.
    /// </summary>
    [Serializable]
    public struct FVec3
    {

        private static FixFloat ZeroEpsilonSq = FixMath.Epsilon;
        internal static FVec3 InternalZero => new FVec3(0, 0, 0);
        internal static FVec3 Arbitrary => new FVec3(1, 1, 1);
        
        // public static FVec3 AxisX = new FVec3(1.0f, 0.0f, 0.0f);
        // public static FVec3 AxisY = new FVec3(0.0f, 1.0f, 0.0f);
        // public static FVec3 AxisZ = new FVec3(0.0f, 0.0f, 1.0f);
        
        /// <summary>The X component of the vector.</summary>
        [SerializeField]
        public FixFloat x;
        /// <summary>The Y component of the vector.</summary>
        [SerializeField]
        public FixFloat y;
        /// <summary>The Z component of the vector.</summary>
        [SerializeField]
        public FixFloat z;

        public FixFloat this[int key]
        {
            get { return (key == 0) ? x : (key == 1) ? y : z; }
            set
            {
                if (key == 0)
                    x = value;
                else if (key == 1)
                    y = value;
                else
                    z = value;
            }
        }
        
        public FVec2 xy
        {
            get { return new FVec2(x,y); }
        }
        
        #region Static readonly variables

        /// <summary>
        /// A vector with components (0,0,0);
        /// </summary>
        public static readonly FVec3 zero;
        /// <summary>
        /// A vector with components (-1,0,0);
        /// </summary>
        public static readonly FVec3 left = new FVec3(-1, 0, 0);

        /// <summary>
        /// A vector with components (1,0,0);
        /// </summary>
        public static readonly FVec3 right = new FVec3(1, 0, 0);
        /// <summary>
        /// A vector with components (0,1,0);
        /// </summary>
        public static readonly FVec3 up = new FVec3(0, 1, 0);

        /// <summary>
        /// A vector with components (0,-1,0);
        /// </summary>
        public static readonly FVec3 down = new FVec3(0, -1, 0);

        /// <summary>
        /// A vector with components (0,0,-1);
        /// </summary>
        public static readonly FVec3 back = new FVec3(0, 0, -1);
        /// <summary>
        /// A vector with components (0,0,1);
        /// </summary>
        public static readonly FVec3 forward = new FVec3(0, 0, 1);
        
        /// <summary>
        /// A vector with components (1,1,1);
        /// </summary>
        public static readonly FVec3 one = new FVec3(1, 1, 1);
        /// <summary>
        /// A vector with components 
        /// (FixFloat.MinValue,FixFloat.MinValue,FixFloat.MinValue);
        /// </summary>
        public static readonly FVec3 MinValue = new FVec3(FixFloat.MinValue);
        /// <summary>
        /// A vector with components 
        /// (FixFloat.MaxValue,FixFloat.MaxValue,FixFloat.MaxValue);
        /// </summary>
        public static readonly FVec3 MaxValue = new FVec3(FixFloat.MaxValue);
        #endregion

        public static FVec3 Abs(FVec3 other) {
            return new FVec3(FixFloat.Abs(other.x), FixFloat.Abs(other.y), FixFloat.Abs(other.z));
        }

        /// <summary>
        /// Gets the squared length of the vector.
        /// </summary>
        /// <returns>Returns the squared length of the vector.</returns>
        public FixFloat sqrMagnitude {
            get { 
                return (((this.x * this.x) + (this.y * this.y)) + (this.z * this.z));
            }
        }

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        /// <returns>Returns the length of the vector.</returns>
        public FixFloat magnitude {
            get {
                FixFloat num = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z);
                return FixFloat.Sqrt(num);
            }
        }

        public static FVec3 ClampMagnitude(FVec3 vector, FixFloat maxLength) {
            return Normalize(vector) * maxLength;
        }

        /// <summary>
        /// Gets a normalized version of the vector.
        /// </summary>
        /// <returns>Returns a normalized version of the vector.</returns>
        public FVec3 normalized {
            get {
                FVec3 result = new FVec3(this.x, this.y, this.z);
                result.Normalize();

                return result;
            }
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>

        public FVec3(int x,int y,int z)
		{
			this.x = (FixFloat)x;
			this.y = (FixFloat)y;
			this.z = (FixFloat)z;
		}

		public FVec3(FixFloat x, FixFloat y, FixFloat z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
        public void Scale(FVec3 other) {
            this.x = x * other.x;
            this.y = y * other.y;
            this.z = z * other.z;
        }

        /// <summary>
        /// Sets all vector component to specific values.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        public void Set(FixFloat x, FixFloat y, FixFloat z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="xyz">All components of the vector are set to xyz</param>
        public FVec3(FixFloat xyz)
        {
            this.x = xyz;
            this.y = xyz;
            this.z = xyz;
        }

		public static FVec3 Lerp(FVec3 from, FVec3 to, FixFloat percent) {
			return from + (to - from) * percent;
		}

        /// <summary>
        /// Builds a string from the JVector.
        /// </summary>
        /// <returns>A string containing all three components.</returns>
        #region public override string ToString()
        public override string ToString() {
            return string.Format("({0:f1}, {1:f1}, {2:f1})", x.AsFloat(), y.AsFloat(), z.AsFloat());
        }
        
        public string ToString(string fmt)
        {
            return string.Format("{0} {1} {2}", x.ToString(fmt), y.ToString(fmt), z.ToString(fmt));
        }
        #endregion

        /// <summary>
        /// Tests if an object is equal to this vector.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>Returns true if they are euqal, otherwise false.</returns>
        #region public override bool Equals(object obj)
        public override bool Equals(object obj)
        {
            if (!(obj is FVec3)) return false;
            FVec3 other = (FVec3)obj;

            return (((x == other.x) && (y == other.y)) && (z == other.z));
        }
        #endregion

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
        public static FVec3 Scale(FVec3 vecA, FVec3 vecB) {
            FVec3 result;
            result.x = vecA.x * vecB.x;
            result.y = vecA.y * vecB.y;
            result.z = vecA.z * vecB.z;

            return result;
        }

        /// <summary>
        /// Tests if two JVector are equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns true if both values are equal, otherwise false.</returns>
        #region public static bool operator ==(JVector value1, JVector value2)
        public static bool operator ==(FVec3 value1, FVec3 value2)
        {
            return (((value1.x == value2.x) && (value1.y == value2.y)) && (value1.z == value2.z));
        }
        #endregion

        /// <summary>
        /// Tests if two JVector are not equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns false if both values are equal, otherwise true.</returns>
        #region public static bool operator !=(JVector value1, JVector value2)
        public static bool operator !=(FVec3 value1, FVec3 value2)
        {
            if ((value1.x == value2.x) && (value1.y == value2.y))
            {
                return (value1.z != value2.z);
            }
            return true;
        }
        #endregion

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the minimum x,y and z values of both vectors.</returns>
        #region public static JVector Min(JVector value1, JVector value2)

        public static FVec3 Min(FVec3 value1, FVec3 value2)
        {
            FVec3 result;
            FVec3.Min(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the minimum x,y and z values of both vectors.</param>
        public static void Min(ref FVec3 value1, ref FVec3 value2, out FVec3 result)
        {
            result.x = (value1.x < value2.x) ? value1.x : value2.x;
            result.y = (value1.y < value2.y) ? value1.y : value2.y;
            result.z = (value1.z < value2.z) ? value1.z : value2.z;
        }
        #endregion

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the maximum x,y and z values of both vectors.</returns>
        #region public static JVector Max(JVector value1, JVector value2)
        public static FVec3 Max(FVec3 value1, FVec3 value2)
        {
            FVec3 result;
            FVec3.Max(ref value1, ref value2, out result);
            return result;
        }
		
		public static FixFloat Distance(FVec3 v1, FVec3 v2) {
			return FixFloat.Sqrt ((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z));
		}

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the maximum x,y and z values of both vectors.</param>
        public static void Max(ref FVec3 value1, ref FVec3 value2, out FVec3 result)
        {
            result.x = (value1.x > value2.x) ? value1.x : value2.x;
            result.y = (value1.y > value2.y) ? value1.y : value2.y;
            result.z = (value1.z > value2.z) ? value1.z : value2.z;
        }
        #endregion

        /// <summary>
        /// Sets the length of the vector to zero.
        /// </summary>
        #region public void MakeZero()
        public void MakeZero()
        {
            x = FixFloat.Zero;
            y = FixFloat.Zero;
            z = FixFloat.Zero;
        }
        #endregion

        /// <summary>
        /// Checks if the length of the vector is zero.
        /// </summary>
        /// <returns>Returns true if the vector is zero, otherwise false.</returns>
        #region public bool IsZero()
        public bool IsZero()
        {
            return (this.sqrMagnitude == FixFloat.Zero);
        }

        /// <summary>
        /// Checks if the length of the vector is nearly zero.
        /// </summary>
        /// <returns>Returns true if the vector is nearly zero, otherwise false.</returns>
        public bool IsNearlyZero()
        {
            return (this.sqrMagnitude < ZeroEpsilonSq);
        }
        #endregion

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <returns>The transformed vector.</returns>
        #region public static JVector Transform(JVector position, JMatrix matrix)
        public static FVec3 Transform(FVec3 position, FixMatrix matrix)
        {
            FVec3 result;
            FVec3.Transform(ref position, ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <param name="result">The transformed vector.</param>
        public static void Transform(ref FVec3 position, ref FixMatrix matrix, out FVec3 result)
        {
            FixFloat num0 = ((position.x * matrix.M11) + (position.y * matrix.M21)) + (position.z * matrix.M31);
            FixFloat num1 = ((position.x * matrix.M12) + (position.y * matrix.M22)) + (position.z * matrix.M32);
            FixFloat num2 = ((position.x * matrix.M13) + (position.y * matrix.M23)) + (position.z * matrix.M33);

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        /// <summary>
        /// Transforms a vector by the transposed of the given Matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <param name="result">The transformed vector.</param>
        public static void TransposedTransform(ref FVec3 position, ref FixMatrix matrix, out FVec3 result)
        {
            FixFloat num0 = ((position.x * matrix.M11) + (position.y * matrix.M12)) + (position.z * matrix.M13);
            FixFloat num1 = ((position.x * matrix.M21) + (position.y * matrix.M22)) + (position.z * matrix.M23);
            FixFloat num2 = ((position.x * matrix.M31) + (position.y * matrix.M32)) + (position.z * matrix.M33);

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>
        #region public static FixFloat Dot(JVector vector1, JVector vector2)
        public static FixFloat Dot(FVec3 vector1, FVec3 vector2)
        {
            return FVec3.Dot(ref vector1, ref vector2);
        }
        
        public FixFloat Dot(FVec3 v2)
        {
            return x * v2.x + y * v2.y + z * v2.z;
        }

        /// <summary>
        /// Calculates the dot product of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>
        public static FixFloat Dot(ref FVec3 vector1, ref FVec3 vector2)
        {
            return ((vector1.x * vector2.x) + (vector1.y * vector2.y)) + (vector1.z * vector2.z);
        }
        #endregion

        // Projects a vector onto another vector.
        public static FVec3 Project(FVec3 vector, FVec3 onNormal)
        {
            FixFloat sqrtMag = Dot(onNormal, onNormal);
            if (sqrtMag < FixMath.Epsilon)
                return zero;
            else
                return onNormal * Dot(vector, onNormal) / sqrtMag;
        }

        // Projects a vector onto a plane defined by a normal orthogonal to the plane.
        public static FVec3 ProjectOnPlane(FVec3 vector, FVec3 planeNormal)
        {
            return vector - Project(vector, planeNormal);
        }


        // Returns the angle in degrees between /from/ and /to/. This is always the smallest
        public static FixFloat Angle(FVec3 from, FVec3 to)
        {
            return FixMath.Acos(FixMath.Clamp(Dot(from.normalized, to.normalized), -FixFloat.ONE, FixFloat.ONE)) * FixMath.Rad2Deg;
        }

        // The smaller of the two possible angles between the two vectors is returned, therefore the result will never be greater than 180 degrees or smaller than -180 degrees.
        // If you imagine the from and to vectors as lines on a piece of paper, both originating from the same point, then the /axis/ vector would point up out of the paper.
        // The measured angle between the two vectors would be positive in a clockwise direction and negative in an anti-clockwise direction.
        public static FixFloat SignedAngle(FVec3 from, FVec3 to, FVec3 axis)
        {
            FVec3 fromNorm = from.normalized, toNorm = to.normalized;
            FixFloat unsignedAngle = FixMath.Acos(FixMath.Clamp(Dot(fromNorm, toNorm), -FixFloat.ONE, FixFloat.ONE)) * FixMath.Rad2Deg;
            FixFloat sign = FixMath.Sign(Dot(axis, Cross(fromNorm, toNorm)));
            return unsignedAngle * sign;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>
        #region public static void Add(JVector value1, JVector value2)
        public static FVec3 Add(FVec3 value1, FVec3 value2)
        {
            FVec3 result;
            FVec3.Add(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Adds to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The sum of both vectors.</param>
        public static void Add(ref FVec3 value1, ref FVec3 value2, out FVec3 result)
        {
            FixFloat num0 = value1.x + value2.x;
            FixFloat num1 = value1.y + value2.y;
            FixFloat num2 = value1.z + value2.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static FVec3 Divide(FVec3 value1, FixFloat scaleFactor) {
            FVec3 result;
            FVec3.Divide(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the scaled vector.</param>
        public static void Divide(ref FVec3 value1, FixFloat scaleFactor, out FVec3 result) {
            result.x = value1.x / scaleFactor;
            result.y = value1.y / scaleFactor;
            result.z = value1.z / scaleFactor;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>
        #region public static JVector Subtract(JVector value1, JVector value2)
        public static FVec3 Subtract(FVec3 value1, FVec3 value2)
        {
            FVec3 result;
            FVec3.Subtract(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Subtracts to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The difference of both vectors.</param>
        public static void Subtract(ref FVec3 value1, ref FVec3 value2, out FVec3 result)
        {
            FixFloat num0 = value1.x - value2.x;
            FixFloat num1 = value1.y - value2.y;
            FixFloat num2 = value1.z - value2.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// The cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The cross product of both vectors.</returns>
        #region public static JVector Cross(JVector vector1, JVector vector2)
        public static FVec3 Cross(FVec3 vector1, FVec3 vector2)
        {
            FVec3 result;
            FVec3.Cross(ref vector1, ref vector2, out result);
            return result;
        }
        
        public FVec3 Cross(FVec3 v2)
        {
            return new FVec3(y * v2.z - z * v2.y, z * v2.x - x * v2.z, x * v2.y - y * v2.x);
        }
        
        /// <summary>
        /// The cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="result">The cross product of both vectors.</param>
        public static void Cross(ref FVec3 vector1, ref FVec3 vector2, out FVec3 result)
        {
            FixFloat num3 = (vector1.y * vector2.z) - (vector1.z * vector2.y);
            FixFloat num2 = (vector1.z * vector2.x) - (vector1.x * vector2.z);
            FixFloat num = (vector1.x * vector2.y) - (vector1.y * vector2.x);
            result.x = num3;
            result.y = num2;
            result.z = num;
        }
        #endregion

        /// <summary>
        /// Gets the hashcode of the vector.
        /// </summary>
        /// <returns>Returns the hashcode of the vector.</returns>
        #region public override int GetHashCode()
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }
        #endregion

        /// <summary>
        /// Inverses the direction of the vector.
        /// </summary>
        #region public static JVector Negate(JVector value)
        public void Negate()
        {
            this.x = -this.x;
            this.y = -this.y;
            this.z = -this.z;
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <returns>The negated vector.</returns>
        public static FVec3 Negate(FVec3 value)
        {
            FVec3 result;
            FVec3.Negate(ref value,out result);
            return result;
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <param name="result">The negated vector.</param>
        public static void Negate(ref FVec3 value, out FVec3 result)
        {
            FixFloat num0 = -value.x;
            FixFloat num1 = -value.y;
            FixFloat num2 = -value.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <returns>A normalized vector.</returns>
        #region public static JVector Normalize(JVector value)
        public static FVec3 Normalize(FVec3 value)
        {
            FVec3 result;
            FVec3.Normalize(ref value, out result);
            return result;
        }

        /// <summary>
        /// Normalizes this vector.
        /// </summary>
        public void Normalize()
        {
            FixFloat num2 = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z);
            FixFloat num = FixFloat.One / FixFloat.Sqrt(num2);
            this.x *= num;
            this.y *= num;
            this.z *= num;
        }

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <param name="result">A normalized vector.</param>
        public static void Normalize(ref FVec3 value, out FVec3 result)
        {
            FixFloat num2 = ((value.x * value.x) + (value.y * value.y)) + (value.z * value.z);
            FixFloat num = FixFloat.One / FixFloat.Sqrt(num2);
            result.x = value.x * num;
            result.y = value.y * num;
            result.z = value.z * num;
        }
        #endregion

        #region public static void Swap(ref JVector vector1, ref JVector vector2)

        /// <summary>
        /// Swaps the components of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector to swap with the second.</param>
        /// <param name="vector2">The second vector to swap with the first.</param>
        public static void Swap(ref FVec3 vector1, ref FVec3 vector2)
        {
            FixFloat temp;

            temp = vector1.x;
            vector1.x = vector2.x;
            vector2.x = temp;

            temp = vector1.y;
            vector1.y = vector2.y;
            vector2.y = temp;

            temp = vector1.z;
            vector1.z = vector2.z;
            vector2.z = temp;
        }
        #endregion

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the multiplied vector.</returns>
        #region public static JVector Multiply(JVector value1, FixFloat scaleFactor)
        public static FVec3 Multiply(FVec3 value1, FixFloat scaleFactor)
        {
            FVec3 result;
            FVec3.Multiply(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the multiplied vector.</param>
        public static void Multiply(ref FVec3 value1, FixFloat scaleFactor, out FVec3 result)
        {
            result.x = value1.x * scaleFactor;
            result.y = value1.y * scaleFactor;
            result.z = value1.z * scaleFactor;
        }
        #endregion

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the cross product of both.</returns>
        #region public static JVector operator %(JVector value1, JVector value2)
        public static FVec3 operator %(FVec3 value1, FVec3 value2)
        {
            FVec3 result; FVec3.Cross(ref value1, ref value2, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the dot product of both.</returns>
        #region public static FixFloat operator *(JVector value1, JVector value2)
        public static FixFloat operator *(FVec3 value1, FVec3 value2)
        {
            return FVec3.Dot(ref value1, ref value2);
        }
        #endregion

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value1">The vector to scale.</param>
        /// <param name="value2">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        #region public static JVector operator *(JVector value1, FixFloat value2)
        public static FVec3 operator *(FVec3 value1, FixFloat value2)
        {
            FVec3 result;
            FVec3.Multiply(ref value1, value2,out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value2">The vector to scale.</param>
        /// <param name="value1">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        #region public static JVector operator *(FixFloat value1, JVector value2)
        public static FVec3 operator *(FixFloat value1, FVec3 value2)
        {
            FVec3 result;
            FVec3.Multiply(ref value2, value1, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>
        #region public static JVector operator -(JVector value1, JVector value2)
        public static FVec3 operator -(FVec3 value1, FVec3 value2)
        {
            FVec3 result; FVec3.Subtract(ref value1, ref value2, out result);
            return result;
        }
        
        public static FVec3 operator -(FVec3 a)
        {
            return new FVec3(-a.x, -a.y, -a.z);
        }
        #endregion

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>
        #region public static JVector operator +(JVector value1, JVector value2)
        public static FVec3 operator +(FVec3 value1, FVec3 value2)
        {
            FVec3 result; FVec3.Add(ref value1, ref value2, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static FVec3 operator /(FVec3 value1, FixFloat value2) {
            FVec3 result;
            FVec3.Divide(ref value1, value2, out result);
            return result;
        }

        public FVec2 ToFVec32() {
            return new FVec2(this.x, this.y);
        }

        public FVec4 ToFVec34()
        {
            return new FVec4(this.x, this.y, this.z, FixFloat.One);
        }

        public Vector3 Vector3()
        {
            return new Vector3(this.x, this.y, this.z);
        }
        
        public bool EpsilonEqual(FVec3 v2, FixFloat epsilon)
        {
            return FixFloat.Abs(x - v2.x) <= epsilon && FixFloat.Abs(y - v2.y) <= epsilon &&
                   FixFloat.Abs(z - v2.z) <= epsilon;
        }
        
        
        public FixFloat RightDir(FVec3 targetPos)
        {
            FixFloat x = targetPos.x - this.x;
            FixFloat y = targetPos.y - this.y;
            FixFloat angle = FixMath.Atan2(y, x) * FixMath.Rad2Deg;
            if (angle < 0)
            {
                angle = angle + 360;
            }
            return angle;
        }
    }

}