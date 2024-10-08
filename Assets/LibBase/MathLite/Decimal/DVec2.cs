using System;
// using ProtoBuf;
using UnityEngine;

namespace LibBase.MathLite.Decimal
{
    [Serializable]
    // [ProtoContract]
    public struct DVec2 : IEquatable<DVec2>
    {
        private static readonly DVec2 zeroVector = new DVec2(0.0f, 0.0f);
        private static readonly DVec2 oneVector = new DVec2(1f, 1f);
        private static readonly DVec2 upVector = new DVec2(0.0f, 1f);
        private static readonly DVec2 downVector = new DVec2(0.0f, -1f);
        private static readonly DVec2 leftVector = new DVec2(-1f, 0.0f);
        private static readonly DVec2 rightVector = new DVec2(1f, 0.0f);

        // [ProtoMember(1)]
        public DFloat x;
        // [ProtoMember(2)]
        public DFloat y;

        public DVec2(DFloat x, DFloat y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2 Vector2()
        {
            return new Vector2(x, y);
        }

        public DFloat this[int key]
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

        public static Vector2 CovertVector2(DVec2 vec2)
        {
            return vec2.Vector2();
        }

        public static implicit operator DVec2(DVec3 v)
        {
            return new DVec2(v.x, v.y);
        }

        public static implicit operator DVec3(DVec2 v)
        {
            return new DVec3(v);
        }


        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, 0).</para>
        /// </summary>
        public static DVec2 zero
        {
            get { return DVec2.zeroVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2(1, 1).</para>
        /// </summary>
        public static DVec2 one
        {
            get { return DVec2.oneVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, 1).</para>
        /// </summary>
        public static DVec2 up
        {
            get { return DVec2.upVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, -1).</para>
        /// </summary>
        public static DVec2 down
        {
            get { return DVec2.downVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2(-1, 0).</para>
        /// </summary>
        public static DVec2 left
        {
            get { return DVec2.leftVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2(1, 0).</para>
        /// </summary>
        public static DVec2 right
        {
            get { return DVec2.rightVector; }
        }


        public DFloat Cross(Vector2 v2)
        {
            return x * v2.y - y * v2.x;
        }


        public DFloat sqrMagnitude
        {
            get { return x * x + y * y; }
        }

        public DFloat magnitude
        {
            get { return DFloat.Sqrt(sqrMagnitude); }
        }

        public DVec2 normalized
        {
            get
            {
                DFloat length = magnitude;
                if (length > DFloat.Zero)
                {
                    DFloat invLength = 1 / length;
                    return new DVec2(x * invLength, y * invLength);
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
            }
            else
            {
                length = 0;
                x = y = 0;
            }

            return length;
        }

        public DFloat Dot(DVec2 v2)
        {
            return x * v2.x + y * v2.y;
        }

        public int AngleD(DVec2 v2)
        {
            DFloat angle = DMath.Clamp(-DFloat.One, DFloat.One, Dot(v2));
            return (int) (DFloat.ACos(angle) * DMath.Rad2Deg);
        }

        public static DVec2 operator +(DVec2 a, DVec2 b)
        {
            return new DVec2(a.x + b.x, a.y + b.y);
        }

        public static DVec2 operator -(DVec2 a, DVec2 b)
        {
            return new DVec2(a.x - b.x, a.y - b.y);
        }

        public static DVec2 operator *(DVec2 a, DVec2 b)
        {
            return new DVec2(a.x * b.x, a.y * b.y);
        }

        public static DVec2 operator /(DVec2 a, DVec2 b)
        {
            return new DVec2(a.x / b.x, a.y / b.y);
        }

        public static DVec2 operator -(DVec2 a)
        {
            return new DVec2(-a.x, -a.y);
        }

        public static DVec2 operator *(DVec2 a, float d)
        {
            return new DVec2(a.x * d, a.y * d);
        }

        public static DVec2 operator *(float d, DVec2 a)
        {
            return new DVec2(a.x * d, a.y * d);
        }

        public static DVec2 operator /(DVec2 a, float d)
        {
            return new DVec2(a.x / d, a.y / d);
        }

        public static bool operator ==(DVec2 lhs, DVec2 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator !=(DVec2 lhs, DVec2 rhs)
        {
            return !(lhs == rhs);
        }

        public bool Equals(DVec2 other)
        {
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public override bool Equals(object obj)
        {
            return obj is DVec2 && Equals((DVec2) obj);
        }


        public bool EpsilonEqual(DVec2 v2, DFloat epsilon)
        {
            return DFloat.Abs(x - v2.x) <= epsilon && DFloat.Abs(y - v2.y) <= epsilon;
        }

        public static DVec2 Lerp(DVec2 a, DVec2 b, DFloat t)
        {
            DFloat s = 1 - t;
            return new DVec2(s * a.x + t * b.x, s * a.y + t * b.y);
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
        }

        public override string ToString()
        {
            return string.Format("({0:F1}, {1:F1})", this.x, this.y);
        }


        public string ToString(string fmt)
        {
            return string.Format("{0} {1}", x.ToString(fmt), y.ToString(fmt));
        }
    }
}