using System;
using UnityEngine;

namespace LibBase.MathLite.FixMath
{
    public struct FVec2 : IEquatable<FVec2>
    {
        private static readonly FVec2 zeroVector = new FVec2(0.0f, 0.0f);
        private static readonly FVec2 oneVector = new FVec2(1f, 1f);
        private static readonly FVec2 upVector = new FVec2(0.0f, 1f);
        private static readonly FVec2 downVector = new FVec2(0.0f, -1f);
        private static readonly FVec2 leftVector = new FVec2(-1f, 0.0f);
        private static readonly FVec2 rightVector = new FVec2(1f, 0.0f);

        public FixFloat x;
        public FixFloat y;

        public FVec2(FixFloat x, FixFloat y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2 Vector2()
        {
            return new Vector2(x, y);
        }

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

        public static Vector2 CovertVector2(FVec2 vec2)
        {
            return vec2.Vector2();
        }

        public static implicit operator FVec2(FVec3 v)
        {
            return new FVec2(v.x, v.y);
        }

        public static implicit operator FVec3(FVec2 v)
        {
            return new FVec3(v);
        }


        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, 0).</para>
        /// </summary>
        public static FVec2 zero
        {
            get { return FVec2.zeroVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2(1, 1).</para>
        /// </summary>
        public static FVec2 one
        {
            get { return FVec2.oneVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, 1).</para>
        /// </summary>
        public static FVec2 up
        {
            get { return FVec2.upVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, -1).</para>
        /// </summary>
        public static FVec2 down
        {
            get { return FVec2.downVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2(-1, 0).</para>
        /// </summary>
        public static FVec2 left
        {
            get { return FVec2.leftVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2(1, 0).</para>
        /// </summary>
        public static FVec2 right
        {
            get { return FVec2.rightVector; }
        }


        public FixFloat Cross(Vector2 v2)
        {
            return x * v2.y - y * v2.x;
        }


        public FixFloat sqrMagnitude
        {
            get { return x * x + y * y; }
        }

        public FixFloat magnitude
        {
            get { return FixFloat.Sqrt(sqrMagnitude); }
        }

        public FVec2 normalized
        {
            get
            {
                FixFloat length = magnitude;
                if (length > FixFloat.Zero)
                {
                    FixFloat invLength = 1 / length;
                    return new FVec2(x * invLength, y * invLength);
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
            }
            else
            {
                length = 0;
                x = y = 0;
            }

            return length;
        }

        public FixFloat Dot(FVec2 v2)
        {
            return x * v2.x + y * v2.y;
        }

        public int AngleD(FVec2 v2)
        {
            FixFloat angle = FixMath.ForceRange(-FixFloat.One, FixFloat.One, Dot(v2));
            return (int) (FixFloat.ACos(angle) * FixMath.Rad2Deg);
        }

        public static FVec2 operator +(FVec2 a, FVec2 b)
        {
            return new FVec2(a.x + b.x, a.y + b.y);
        }

        public static FVec2 operator -(FVec2 a, FVec2 b)
        {
            return new FVec2(a.x - b.x, a.y - b.y);
        }

        public static FVec2 operator *(FVec2 a, FVec2 b)
        {
            return new FVec2(a.x * b.x, a.y * b.y);
        }

        public static FVec2 operator /(FVec2 a, FVec2 b)
        {
            return new FVec2(a.x / b.x, a.y / b.y);
        }

        public static FVec2 operator -(FVec2 a)
        {
            return new FVec2(-a.x, -a.y);
        }

        public static FVec2 operator *(FVec2 a, float d)
        {
            return new FVec2(a.x * d, a.y * d);
        }

        public static FVec2 operator *(float d, FVec2 a)
        {
            return new FVec2(a.x * d, a.y * d);
        }

        public static FVec2 operator /(FVec2 a, float d)
        {
            return new FVec2(a.x / d, a.y / d);
        }

        public static bool operator ==(FVec2 lhs, FVec2 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator !=(FVec2 lhs, FVec2 rhs)
        {
            return !(lhs == rhs);
        }

        public bool Equals(FVec2 other)
        {
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public override bool Equals(object obj)
        {
            return obj is FVec2 && Equals((FVec2) obj);
        }


        public bool EpsilonEqual(FVec2 v2, FixFloat epsilon)
        {
            return FixFloat.Abs(x - v2.x) <= epsilon && FixFloat.Abs(y - v2.y) <= epsilon;
        }

        public static FVec2 Lerp(FVec2 a, FVec2 b, FixFloat t)
        {
            FixFloat s = 1 - t;
            return new FVec2(s * a.x + t * b.x, s * a.y + t * b.y);
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