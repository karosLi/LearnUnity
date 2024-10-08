using System;
using UnityEngine;

namespace LibBase.MathLite.FixMath
{
    public struct FVec3 : IEquatable<FVec3>
    {
        private static readonly FVec3 zeroVector = new FVec3(0.0f, 0.0f, 0.0f);
        private static readonly FVec3 oneVector = new FVec3(1f, 1f, 1f);
        private static readonly FVec3 upVector = new FVec3(0.0f, 1f, 0.0f);
        private static readonly FVec3 downVector = new FVec3(0.0f, -1f, 0.0f);
        private static readonly FVec3 leftVector = new FVec3(-1f, 0.0f, 0.0f);
        private static readonly FVec3 rightVector = new FVec3(1f, 0.0f, 0.0f);
        private static readonly FVec3 forwardVector = new FVec3(0.0f, 0.0f, 1f);
        private static readonly FVec3 backVector = new FVec3(0.0f, 0.0f, -1f);

        static public readonly FVec3 AxisX = new FVec3(1.0f, 0.0f, 0.0f);
        static public readonly FVec3 AxisY = new FVec3(0.0f, 1.0f, 0.0f);
        static public readonly FVec3 AxisZ = new FVec3(0.0f, 0.0f, 1.0f);

        public FixFloat x;
        public FixFloat y;
        public FixFloat z;

        public FVec3(FixFloat x, FixFloat y, FixFloat z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public FVec3(FVec2 vec2, FixFloat z)
        {
            this.x = vec2.x;
            this.y = vec2.y;
            this.z = z;
        }

        public FVec3(FixFloat x, FixFloat y) : this(x, y, FixFloat.Zero)
        {
        }


        public FVec3(FVec2 vec2) : this(vec2, FixFloat.Zero)
        {
        }

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

        /// <summary>
        ///   <para>Shorthand for writing Vector3(0, 0, 0).</para>
        /// </summary>
        public static FVec3 zero
        {
            get { return FVec3.zeroVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3(1, 1, 1).</para>
        /// </summary>
        public static FVec3 one
        {
            get { return FVec3.oneVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3(0, 0, 1).</para>
        /// </summary>
        public static FVec3 forward
        {
            get { return FVec3.forwardVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3(0, 0, -1).</para>
        /// </summary>
        public static FVec3 back
        {
            get { return FVec3.backVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3(0, 1, 0).</para>
        /// </summary>
        public static FVec3 up
        {
            get { return FVec3.upVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3(0, -1, 0).</para>
        /// </summary>
        public static FVec3 down
        {
            get { return FVec3.downVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3(-1, 0, 0).</para>
        /// </summary>
        public static FVec3 left
        {
            get { return FVec3.leftVector; }
        }

        public FixFloat Dot(FVec3 v2)
        {
            return x * v2.x + y * v2.y + z * v2.z;
        }

        public int AngleD(FVec2 v2)
        {
            FixFloat angle = FixMath.ForceRange(-FixFloat.One, FixFloat.One, Dot(v2));
            return (int) (FixFloat.ACos(angle) * FixMath.Rad2Deg);
        }

        public FVec3 Cross(FVec3 v2)
        {
            return new FVec3(y * v2.z - z * v2.y, z * v2.x - x * v2.z, x * v2.y - y * v2.x);
        }

        public static FVec3 Cross(FVec3 v1, FVec3 v2)
        {
            return v1.Cross(v2);
        }

        public FVec3 UnitCross(FVec3 v2)
        {
            FVec3 n = new FVec3(y * v2.z - z * v2.y, z * v2.x - x * v2.z, x * v2.y - y * v2.x);
            n.Normalize();
            return n;
        }

        public FixFloat sqrMagnitude
        {
            get { return x * x + y * y + z * z; }
        }

        public FixFloat magnitude
        {
            get { return FixFloat.Sqrt(sqrMagnitude); }
        }

        public FVec3 normalized
        {
            get
            {
                FixFloat length = magnitude;
                if (length > FixFloat.Zero)
                {
                    FixFloat invLength = 1 / length;
                    return new FVec3(x * invLength, y * invLength, z * invLength);
                }
                else
                    return zero;
            }
        }

        public FixFloat Normalize()
        {
            return Normalize(FixFloat.Zero);
        }

        public FixFloat Normalize(FixFloat epsilon)
        {
            FixFloat length = magnitude;
            if (length > epsilon)
            {
                FixFloat invLength = FixFloat.One / length;
                x *= invLength;
                y *= invLength;
                z *= invLength;
            }
            else
            {
                length = 0;
                x = y = z = 0;
            }

            return length;
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3(1, 0, 0).</para>
        /// </summary>
        public static FVec3 right
        {
            get { return FVec3.rightVector; }
        }

        public static FVec3 operator +(FVec3 a, FVec3 b)
        {
            return new FVec3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static FVec3 operator -(FVec3 a, FVec3 b)
        {
            return new FVec3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static FVec3 operator -(FVec3 a)
        {
            return new FVec3(-a.x, -a.y, -a.z);
        }

        public static FVec3 operator *(FVec3 a, float d)
        {
            return new FVec3(a.x * d, a.y * d, a.z * d);
        }

        public static FVec3 operator *(float d, FVec3 a)
        {
            return new FVec3(a.x * d, a.y * d, a.z * d);
        }

        public static FVec3 operator /(FVec3 a, float d)
        {
            return new FVec3(a.x / d, a.y / d, a.z / d);
        }

        public static bool operator ==(FVec3 lhs, FVec3 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        public static bool operator !=(FVec3 lhs, FVec3 rhs)
        {
            return !(lhs == rhs);
        }

        public Vector3 Vector3()
        {
            return new Vector3(x, y, z);
        }

        public FVec2 VecXZ()
        {
            return new FVec2(x, z);
        }

        public static Vector3 CovertVector3(FVec3 vec3)
        {
            return vec3.Vector3();
        }

        public bool Equals(FVec3 other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
        }

        public bool EpsilonEqual(FVec3 v2, FixFloat epsilon)
        {
            return FixFloat.Abs(x - v2.x) <= epsilon && FixFloat.Abs(y - v2.y) <= epsilon &&
                   FixFloat.Abs(z - v2.z) <= epsilon;
        }

        public static FVec3 Lerp(FVec3 a, FVec3 b, FixFloat t)
        {
            FixFloat s = 1 - t;
            return new FVec3(s * a.x + t * b.x, s * a.y + t * b.y, s * a.z + t * b.z);
        }

        public override bool Equals(object obj)
        {
            return obj is FVec3 && Equals((FVec3) obj);
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2;
        }

        public override string ToString()
        {
            return string.Format("({0:F1}, {1:F1}, {2:F1})", this.x, this.y, this.z);
        }

        public string ToString(string fmt)
        {
            return string.Format("{0} {1} {2}", x.ToString(fmt), y.ToString(fmt), z.ToString(fmt));
        }
    }
}