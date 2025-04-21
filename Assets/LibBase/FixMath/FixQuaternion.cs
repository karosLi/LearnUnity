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
    /// A Quaternion representing an orientation.
    /// </summary>
    [Serializable]
    public struct FixQuaternion
    {

        /// <summary>The X component of the quaternion.</summary>
        [SerializeField]
        public FixFloat x;
        /// <summary>The Y component of the quaternion.</summary>
        [SerializeField]
        public FixFloat y;
        /// <summary>The Z component of the quaternion.</summary>
        [SerializeField]
        public FixFloat z;
        /// <summary>The W component of the quaternion.</summary>
        [SerializeField]
        public FixFloat w;

        public static FixQuaternion identity;

        static FixQuaternion() {
            identity = new FixQuaternion(0, 0, 0, 1);
        }

        /// <summary>
        /// Initializes a new instance of the JQuaternion structure.
        /// </summary>
        /// <param name="x">The X component of the quaternion.</param>
        /// <param name="y">The Y component of the quaternion.</param>
        /// <param name="z">The Z component of the quaternion.</param>
        /// <param name="w">The W component of the quaternion.</param>
        public FixQuaternion(FixFloat x, FixFloat y, FixFloat z, FixFloat w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public void Set(FixFloat new_x, FixFloat new_y, FixFloat new_z, FixFloat new_w) {
            this.x = new_x;
            this.y = new_y;
            this.z = new_z;
            this.w = new_w;
        }

        public void SetFromToRotation(FVec3 fromDirection, FVec3 toDirection) {
            FixQuaternion targetRotation = FixQuaternion.FromToRotation(fromDirection, toDirection);
            this.Set(targetRotation.x, targetRotation.y, targetRotation.z, targetRotation.w);
        }

        public FVec3 eulerAngles {
            get {
                FVec3 result = new FVec3();

                FixFloat ysqr = y * y;
                FixFloat t0 = -2.0f * (ysqr + z * z) + 1.0f;
                FixFloat t1 = +2.0f * (x * y - w * z);
                FixFloat t2 = -2.0f * (x * z + w * y);
                FixFloat t3 = +2.0f * (y * z - w * x);
                FixFloat t4 = -2.0f * (x * x + ysqr) + 1.0f;

                t2 = t2 > 1.0f ? (FixFloat)1.0f : t2;
                t2 = t2 < -1.0f ? (FixFloat)(-1.0f) : t2;


                result.x = FixFloat.Atan2(t3, t4) * FixFloat.Rad2Deg;
                result.y = FixFloat.Asin(t2) * FixFloat.Rad2Deg;
                result.z = FixFloat.Atan2(t1, t0) * FixFloat.Rad2Deg;

                return result * -1;
            }
        }

        public static FixFloat Angle(FixQuaternion a, FixQuaternion b) {
            FixQuaternion aInv = FixQuaternion.Inverse(a);
            FixQuaternion f = b * aInv;

            FixFloat angle = FixFloat.Acos(f.w) * 2 * FixFloat.Rad2Deg;

            if (angle > 180) {
                angle = 360 - angle;
            }

            return angle;
        }

        /// <summary>
        /// Quaternions are added.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <returns>The sum of both quaternions.</returns>
        #region public static JQuaternion Add(JQuaternion quaternion1, JQuaternion quaternion2)
        public static FixQuaternion Add(FixQuaternion quaternion1, FixQuaternion quaternion2)
        {
            FixQuaternion result;
            FixQuaternion.Add(ref quaternion1, ref quaternion2, out result);
            return result;
        }

        public static FixQuaternion LookRotation(FVec3 forward) {
            return CreateFromMatrix(FixMatrix.LookAt(forward, FVec3.up));
        }

        public static FixQuaternion LookRotation(FVec3 forward, FVec3 upwards) {
            return CreateFromMatrix(FixMatrix.LookAt(forward, upwards));
        }

        public static FixQuaternion Slerp(FixQuaternion from, FixQuaternion to, FixFloat t) {
            t = FixMath.Clamp(t, 0, 1);

            FixFloat dot = Dot(from, to);

            if (dot < 0.0f) {
                to = Multiply(to, -1);
                dot = -dot;
            }

            FixFloat halfTheta = FixFloat.Acos(dot);

            return Multiply(Multiply(from, FixFloat.Sin((1 - t) * halfTheta)) + Multiply(to, FixFloat.Sin(t * halfTheta)), 1 / FixFloat.Sin(halfTheta));
        }

        public static FixQuaternion RotateTowards(FixQuaternion from, FixQuaternion to, FixFloat maxDegreesDelta) {
            FixFloat dot = Dot(from, to);

            if (dot < 0.0f) {
                to = Multiply(to, -1);
                dot = -dot;
            }

            FixFloat halfTheta = FixFloat.Acos(dot);
            FixFloat theta = halfTheta * 2;

            maxDegreesDelta *= FixFloat.Deg2Rad;

            if (maxDegreesDelta >= theta) {
                return to;
            }

            maxDegreesDelta /= theta;

            return Multiply(Multiply(from, FixFloat.Sin((1 - maxDegreesDelta) * halfTheta)) + Multiply(to, FixFloat.Sin(maxDegreesDelta * halfTheta)), 1 / FixFloat.Sin(halfTheta));
        }

        public static FixQuaternion Euler(FixFloat x, FixFloat y, FixFloat z) {
            x *= FixFloat.Deg2Rad;
            y *= FixFloat.Deg2Rad;
            z *= FixFloat.Deg2Rad;

            FixQuaternion rotation;
            FixQuaternion.CreateFromYawPitchRoll(y, x, z, out rotation);

            return rotation;
        }

        public static FixQuaternion Euler(FVec3 eulerAngles) {
            return Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
        }

        public static FixQuaternion AngleAxis(FixFloat angle, FVec3 axis) {
            axis = axis * FixFloat.Deg2Rad;
            axis.Normalize();

            FixFloat halfAngle = angle * FixFloat.Deg2Rad * FixFloat.Half;

            FixQuaternion rotation;
            FixFloat sin = FixFloat.Sin(halfAngle);

            rotation.x = axis.x * sin;
            rotation.y = axis.y * sin;
            rotation.z = axis.z * sin;
            rotation.w = FixFloat.Cos(halfAngle);

            return rotation;
        }

        public static void CreateFromYawPitchRoll(FixFloat yaw, FixFloat pitch, FixFloat roll, out FixQuaternion result)
        {
            FixFloat num9 = roll * FixFloat.Half;
            FixFloat num6 = FixFloat.Sin(num9);
            FixFloat num5 = FixFloat.Cos(num9);
            FixFloat num8 = pitch * FixFloat.Half;
            FixFloat num4 = FixFloat.Sin(num8);
            FixFloat num3 = FixFloat.Cos(num8);
            FixFloat num7 = yaw * FixFloat.Half;
            FixFloat num2 = FixFloat.Sin(num7);
            FixFloat num = FixFloat.Cos(num7);
            result.x = ((num * num4) * num5) + ((num2 * num3) * num6);
            result.y = ((num2 * num3) * num5) - ((num * num4) * num6);
            result.z = ((num * num3) * num6) - ((num2 * num4) * num5);
            result.w = ((num * num3) * num5) + ((num2 * num4) * num6);
        }

        /// <summary>
        /// Quaternions are added.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <param name="result">The sum of both quaternions.</param>
        public static void Add(ref FixQuaternion quaternion1, ref FixQuaternion quaternion2, out FixQuaternion result)
        {
            result.x = quaternion1.x + quaternion2.x;
            result.y = quaternion1.y + quaternion2.y;
            result.z = quaternion1.z + quaternion2.z;
            result.w = quaternion1.w + quaternion2.w;
        }
        #endregion

        public static FixQuaternion Conjugate(FixQuaternion value)
        {
            FixQuaternion quaternion;
            quaternion.x = -value.x;
            quaternion.y = -value.y;
            quaternion.z = -value.z;
            quaternion.w = value.w;
            return quaternion;
        }

        public static FixFloat Dot(FixQuaternion a, FixQuaternion b) {
            return a.w * b.w + a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static FixQuaternion Inverse(FixQuaternion rotation) {
            FixFloat invNorm = FixFloat.One / ((rotation.x * rotation.x) + (rotation.y * rotation.y) + (rotation.z * rotation.z) + (rotation.w * rotation.w));
            return FixQuaternion.Multiply(FixQuaternion.Conjugate(rotation), invNorm);
        }

        public static FixQuaternion FromToRotation(FVec3 fromVector, FVec3 toVector) {
            FVec3 w = FVec3.Cross(fromVector, toVector);
            FixQuaternion q = new FixQuaternion(w.x, w.y, w.z, FVec3.Dot(fromVector, toVector));
            q.w += FixFloat.Sqrt(fromVector.sqrMagnitude * toVector.sqrMagnitude);
            q.Normalize();

            return q;
        }

        public static FixQuaternion Lerp(FixQuaternion a, FixQuaternion b, FixFloat t) {
            t = FixMath.Clamp(t, FixFloat.Zero, FixFloat.One);

            return LerpUnclamped(a, b, t);
        }

        public static FixQuaternion LerpUnclamped(FixQuaternion a, FixQuaternion b, FixFloat t) {
            FixQuaternion result = FixQuaternion.Multiply(a, (1 - t)) + FixQuaternion.Multiply(b, t);
            result.Normalize();

            return result;
        }

        /// <summary>
        /// Quaternions are subtracted.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <returns>The difference of both quaternions.</returns>
        #region public static JQuaternion Subtract(JQuaternion quaternion1, JQuaternion quaternion2)
        public static FixQuaternion Subtract(FixQuaternion quaternion1, FixQuaternion quaternion2)
        {
            FixQuaternion result;
            FixQuaternion.Subtract(ref quaternion1, ref quaternion2, out result);
            return result;
        }

        /// <summary>
        /// Quaternions are subtracted.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <param name="result">The difference of both quaternions.</param>
        public static void Subtract(ref FixQuaternion quaternion1, ref FixQuaternion quaternion2, out FixQuaternion result)
        {
            result.x = quaternion1.x - quaternion2.x;
            result.y = quaternion1.y - quaternion2.y;
            result.z = quaternion1.z - quaternion2.z;
            result.w = quaternion1.w - quaternion2.w;
        }
        #endregion

        /// <summary>
        /// Multiply two quaternions.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <returns>The product of both quaternions.</returns>
        #region public static JQuaternion Multiply(JQuaternion quaternion1, JQuaternion quaternion2)
        public static FixQuaternion Multiply(FixQuaternion quaternion1, FixQuaternion quaternion2)
        {
            FixQuaternion result;
            FixQuaternion.Multiply(ref quaternion1, ref quaternion2, out result);
            return result;
        }

        /// <summary>
        /// Multiply two quaternions.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <param name="result">The product of both quaternions.</param>
        public static void Multiply(ref FixQuaternion quaternion1, ref FixQuaternion quaternion2, out FixQuaternion result)
        {
            FixFloat x = quaternion1.x;
            FixFloat y = quaternion1.y;
            FixFloat z = quaternion1.z;
            FixFloat w = quaternion1.w;
            FixFloat num4 = quaternion2.x;
            FixFloat num3 = quaternion2.y;
            FixFloat num2 = quaternion2.z;
            FixFloat num = quaternion2.w;
            FixFloat num12 = (y * num2) - (z * num3);
            FixFloat num11 = (z * num4) - (x * num2);
            FixFloat num10 = (x * num3) - (y * num4);
            FixFloat num9 = ((x * num4) + (y * num3)) + (z * num2);
            result.x = ((x * num) + (num4 * w)) + num12;
            result.y = ((y * num) + (num3 * w)) + num11;
            result.z = ((z * num) + (num2 * w)) + num10;
            result.w = (w * num) - num9;
        }
        #endregion

        /// <summary>
        /// Scale a quaternion
        /// </summary>
        /// <param name="quaternion1">The quaternion to scale.</param>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <returns>The scaled quaternion.</returns>
        #region public static JQuaternion Multiply(JQuaternion quaternion1, FixFloat scaleFactor)
        public static FixQuaternion Multiply(FixQuaternion quaternion1, FixFloat scaleFactor)
        {
            FixQuaternion result;
            FixQuaternion.Multiply(ref quaternion1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Scale a quaternion
        /// </summary>
        /// <param name="quaternion1">The quaternion to scale.</param>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <param name="result">The scaled quaternion.</param>
        public static void Multiply(ref FixQuaternion quaternion1, FixFloat scaleFactor, out FixQuaternion result)
        {
            result.x = quaternion1.x * scaleFactor;
            result.y = quaternion1.y * scaleFactor;
            result.z = quaternion1.z * scaleFactor;
            result.w = quaternion1.w * scaleFactor;
        }
        #endregion

        /// <summary>
        /// Sets the length of the quaternion to one.
        /// </summary>
        #region public void Normalize()
        public void Normalize()
        {
            FixFloat num2 = (((this.x * this.x) + (this.y * this.y)) + (this.z * this.z)) + (this.w * this.w);
            FixFloat num = 1 / (FixFloat.Sqrt(num2));
            this.x *= num;
            this.y *= num;
            this.z *= num;
            this.w *= num;
        }
        #endregion

        /// <summary>
        /// Creates a quaternion from a matrix.
        /// </summary>
        /// <param name="matrix">A matrix representing an orientation.</param>
        /// <returns>JQuaternion representing an orientation.</returns>
        #region public static JQuaternion CreateFromMatrix(JMatrix matrix)
        public static FixQuaternion CreateFromMatrix(FixMatrix matrix)
        {
            FixQuaternion result;
            FixQuaternion.CreateFromMatrix(ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Creates a quaternion from a matrix.
        /// </summary>
        /// <param name="matrix">A matrix representing an orientation.</param>
        /// <param name="result">JQuaternion representing an orientation.</param>
        public static void CreateFromMatrix(ref FixMatrix matrix, out FixQuaternion result)
        {
            FixFloat num8 = (matrix.M11 + matrix.M22) + matrix.M33;
            if (num8 > FixFloat.Zero)
            {
                FixFloat num = FixFloat.Sqrt((num8 + FixFloat.One));
                result.w = num * FixFloat.Half;
                num = FixFloat.Half / num;
                result.x = (matrix.M23 - matrix.M32) * num;
                result.y = (matrix.M31 - matrix.M13) * num;
                result.z = (matrix.M12 - matrix.M21) * num;
            }
            else if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
            {
                FixFloat num7 = FixFloat.Sqrt((((FixFloat.One + matrix.M11) - matrix.M22) - matrix.M33));
                FixFloat num4 = FixFloat.Half / num7;
                result.x = FixFloat.Half * num7;
                result.y = (matrix.M12 + matrix.M21) * num4;
                result.z = (matrix.M13 + matrix.M31) * num4;
                result.w = (matrix.M23 - matrix.M32) * num4;
            }
            else if (matrix.M22 > matrix.M33)
            {
                FixFloat num6 = FixFloat.Sqrt((((FixFloat.One + matrix.M22) - matrix.M11) - matrix.M33));
                FixFloat num3 = FixFloat.Half / num6;
                result.x = (matrix.M21 + matrix.M12) * num3;
                result.y = FixFloat.Half * num6;
                result.z = (matrix.M32 + matrix.M23) * num3;
                result.w = (matrix.M31 - matrix.M13) * num3;
            }
            else
            {
                FixFloat num5 = FixFloat.Sqrt((((FixFloat.One + matrix.M33) - matrix.M11) - matrix.M22));
                FixFloat num2 = FixFloat.Half / num5;
                result.x = (matrix.M31 + matrix.M13) * num2;
                result.y = (matrix.M32 + matrix.M23) * num2;
                result.z = FixFloat.Half * num5;
                result.w = (matrix.M12 - matrix.M21) * num2;
            }
        }
        #endregion

        /// <summary>
        /// Multiply two quaternions.
        /// </summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The product of both quaternions.</returns>
        #region public static FixFloat operator *(JQuaternion value1, JQuaternion value2)
        public static FixQuaternion operator *(FixQuaternion value1, FixQuaternion value2)
        {
            FixQuaternion result;
            FixQuaternion.Multiply(ref value1, ref value2,out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Add two quaternions.
        /// </summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The sum of both quaternions.</returns>
        #region public static FixFloat operator +(JQuaternion value1, JQuaternion value2)
        public static FixQuaternion operator +(FixQuaternion value1, FixQuaternion value2)
        {
            FixQuaternion result;
            FixQuaternion.Add(ref value1, ref value2, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Subtract two quaternions.
        /// </summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The difference of both quaternions.</returns>
        #region public static FixFloat operator -(JQuaternion value1, JQuaternion value2)
        public static FixQuaternion operator -(FixQuaternion value1, FixQuaternion value2)
        {
            FixQuaternion result;
            FixQuaternion.Subtract(ref value1, ref value2, out result);
            return result;
        }
        #endregion

        /**
         *  @brief Rotates a {@link FVec3} by the {@link TSQuanternion}.
         **/
        public static FVec3 operator *(FixQuaternion quat, FVec3 vec) {
            FixFloat num = quat.x * 2f;
            FixFloat num2 = quat.y * 2f;
            FixFloat num3 = quat.z * 2f;
            FixFloat num4 = quat.x * num;
            FixFloat num5 = quat.y * num2;
            FixFloat num6 = quat.z * num3;
            FixFloat num7 = quat.x * num2;
            FixFloat num8 = quat.x * num3;
            FixFloat num9 = quat.y * num3;
            FixFloat num10 = quat.w * num;
            FixFloat num11 = quat.w * num2;
            FixFloat num12 = quat.w * num3;

            FVec3 result;
            result.x = (1f - (num5 + num6)) * vec.x + (num7 - num12) * vec.y + (num8 + num11) * vec.z;
            result.y = (num7 + num12) * vec.x + (1f - (num4 + num6)) * vec.y + (num9 - num10) * vec.z;
            result.z = (num8 - num11) * vec.x + (num9 + num10) * vec.y + (1f - (num4 + num5)) * vec.z;

            return result;
        }

        public override string ToString() {
            return string.Format("({0:f1}, {1:f1}, {2:f1}, {3:f1})", x.AsFloat(), y.AsFloat(), z.AsFloat(), w.AsFloat());
        }

    }
}
