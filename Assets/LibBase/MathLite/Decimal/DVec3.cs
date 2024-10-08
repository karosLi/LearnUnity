using System;
// using ProtoBuf;
using UnityEngine;

namespace LibBase.MathLite.Decimal
{
    [Serializable]
    // [ProtoContract]
    public struct DVec3 : IEquatable<DVec3>
    {
        private static readonly DVec3 zeroVector = new DVec3(0.0f, 0.0f, 0.0f);
        private static readonly DVec3 oneVector = new DVec3(1f, 1f, 1f);
        private static readonly DVec3 upVector = new DVec3(0.0f, 1f, 0.0f);
        private static readonly DVec3 downVector = new DVec3(0.0f, -1f, 0.0f);
        private static readonly DVec3 leftVector = new DVec3(-1f, 0.0f, 0.0f);
        private static readonly DVec3 rightVector = new DVec3(1f, 0.0f, 0.0f);
        private static readonly DVec3 forwardVector = new DVec3(0.0f, 0.0f, 1f);
        private static readonly DVec3 backVector = new DVec3(0.0f, 0.0f, -1f);

        static public readonly DVec3 AxisX = new DVec3(1.0f, 0.0f, 0.0f);
        static public readonly DVec3 AxisY = new DVec3(0.0f, 1.0f, 0.0f);
        static public readonly DVec3 AxisZ = new DVec3(0.0f, 0.0f, 1.0f);
        // [ProtoMember(1)]
        public DFloat x;
        // [ProtoMember(2)]
        public DFloat y;
        // [ProtoMember(3)]
        public DFloat z;

        public DVec3(DFloat x, DFloat y, DFloat z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public DVec3(DVec2 vec2, DFloat z)
        {
            this.x = vec2.x;
            this.y = vec2.y;
            this.z = z;
        }

        public DVec3(DFloat x, DFloat y) : this(x, y, DFloat.Zero)
        {
        }


        public DVec3(DVec2 vec2) : this(vec2, DFloat.Zero)
        {
        }

        public DFloat this[int key]
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
        public static DVec3 zero
        {
            get { return DVec3.zeroVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3(1, 1, 1).</para>
        /// </summary>
        public static DVec3 one
        {
            get { return DVec3.oneVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3(0, 0, 1).</para>
        /// </summary>
        public static DVec3 forward
        {
            get { return DVec3.forwardVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3(0, 0, -1).</para>
        /// </summary>
        public static DVec3 back
        {
            get { return DVec3.backVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3(0, 1, 0).</para>
        /// </summary>
        public static DVec3 up
        {
            get { return DVec3.upVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3(0, -1, 0).</para>
        /// </summary>
        public static DVec3 down
        {
            get { return DVec3.downVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3(-1, 0, 0).</para>
        /// </summary>
        public static DVec3 left
        {
            get { return DVec3.leftVector; }
        }

        public DFloat Dot(DVec3 v2)
        {
            return x * v2.x + y * v2.y + z * v2.z;
        }

        public int AngleD(DVec2 v2)
        {
            DFloat angle = DMath.Clamp(-DFloat.One, DFloat.One, Dot(v2));
            return (int) (DFloat.ACos(angle) * DMath.Rad2Deg);
        }

        public DVec3 Cross(DVec3 v2)
        {
            return new DVec3(y * v2.z - z * v2.y, z * v2.x - x * v2.z, x * v2.y - y * v2.x);
        }

        public static DVec3 Cross(DVec3 v1, DVec3 v2)
        {
            return v1.Cross(v2);
        }

        public DVec3 UnitCross(DVec3 v2)
        {
            DVec3 n = new DVec3(y * v2.z - z * v2.y, z * v2.x - x * v2.z, x * v2.y - y * v2.x);
            n.Normalize();
            return n;
        }

        public DFloat sqrMagnitude
        {
            get { return x * x + y * y + z * z; }
        }

        public DFloat magnitude
        {
            get { return DFloat.Sqrt(sqrMagnitude); }
        }

        public DVec3 normalized
        {
            get
            {
                DFloat length = magnitude;
                if (length > DFloat.Zero)
                {
                    DFloat invLength = 1 / length;
                    return new DVec3(x * invLength, y * invLength, z * invLength);
                }
                else
                    return zero;
            }
        }

        public DFloat Normalize()
        {
            return Normalize(DFloat.Zero);
        }

        public DFloat Normalize(DFloat epsilon)
        {
            DFloat length = magnitude;
            if (length > epsilon)
            {
                DFloat invLength = DFloat.One / length;
                x *= invLength;
                y *= invLength;
                z *= invLength;
            }
            else
            {
                length = DFloat.Zero;
                x = y = z = DFloat.Zero;
            }

            return length;
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3(1, 0, 0).</para>
        /// </summary>
        public static DVec3 right
        {
            get { return DVec3.rightVector; }
        }

        public static DVec3 operator +(DVec3 a, DVec3 b)
        {
            return new DVec3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static DVec3 operator -(DVec3 a, DVec3 b)
        {
            return new DVec3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static DVec3 operator -(DVec3 a)
        {
            return new DVec3(-a.x, -a.y, -a.z);
        }

        public static DVec3 operator *(DVec3 a, float d)
        {
            return new DVec3(a.x * d, a.y * d, a.z * d);
        }

        public static DVec3 operator *(float d, DVec3 a)
        {
            return new DVec3(a.x * d, a.y * d, a.z * d);
        }

        public static DVec3 operator /(DVec3 a, float d)
        {
            return new DVec3(a.x / d, a.y / d, a.z / d);
        }

        public static bool operator ==(DVec3 lhs, DVec3 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        public static bool operator !=(DVec3 lhs, DVec3 rhs)
        {
            return !(lhs == rhs);
        }

        public Vector3 Vector3()
        {
            return new Vector3(x, y, z);
        }

        public DVec2 VecXZ()
        {
            return new DVec2(x, z);
        }

        public static Vector3 CovertVector3(DVec3 vec3)
        {
            return vec3.Vector3();
        }

        public bool Equals(DVec3 other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
        }

        public bool EpsilonEqual(DVec3 v2, DFloat epsilon)
        {
            return DFloat.Abs(x - v2.x) <= epsilon && DFloat.Abs(y - v2.y) <= epsilon &&
                   DFloat.Abs(z - v2.z) <= epsilon;
        }

        public static DVec3 Lerp(DVec3 a, DVec3 b, DFloat t)
        {
            DFloat s = 1 - t;
            return new DVec3(s * a.x + t * b.x, s * a.y + t * b.y, s * a.z + t * b.z);
        }

        public override bool Equals(object obj)
        {
            return obj is DVec3 && Equals((DVec3) obj);
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