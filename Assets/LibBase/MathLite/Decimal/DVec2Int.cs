using System;
// using ProtoBuf;
using UnityEngine;

namespace LibBase.MathLite.Decimal
{
    [Serializable]
    // [ProtoContract]
    public struct DVec2Int : IEquatable<DVec2Int>
    {
        // [ProtoMember(1)]
        private int m_X;
        // [ProtoMember(2)]
        private int m_Y;
        private static readonly DVec2Int s_Zero = new DVec2Int(0, 0);
        private static readonly DVec2Int s_One = new DVec2Int(1, 1);
        private static readonly DVec2Int s_Up = new DVec2Int(0, 1);
        private static readonly DVec2Int s_Down = new DVec2Int(0, -1);
        private static readonly DVec2Int s_Left = new DVec2Int(-1, 0);
        private static readonly DVec2Int s_Right = new DVec2Int(1, 0);

        public DVec2Int(int x, int y)
        {
            this.m_X = x;
            this.m_Y = y;
        }

        public DVec2Int(DFloat x, DFloat y)
        {
            this.m_X = (int) x;
            this.m_Y = (int) y;
        }

        /// <summary>
        ///   <para>X component of the vector.</para>
        /// </summary>
        public int x
        {
            get { return this.m_X; }
            set { this.m_X = value; }
        }

        /// <summary>
        ///   <para>Y component of the vector.</para>
        /// </summary>
        public int y
        {
            get { return this.m_Y; }
            set { this.m_Y = value; }
        }

        /// <summary>
        ///   <para>Set x and y components of an existing FVec2Int.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Set(int x, int y)
        {
            this.m_X = x;
            this.m_Y = y;
        }

        public int this[int index]
        {
            get
            {
                if (index == 0)
                    return this.x;
                if (index == 1)
                    return this.y;
                throw new IndexOutOfRangeException(string.Format("Invalid FVec2Int index addressed: {0}!",
                    (object) index));
            }
            set
            {
                if (index != 0)
                {
                    if (index != 1)
                        throw new IndexOutOfRangeException(string.Format("Invalid FVec2Int index addressed: {0}!",
                            (object) index));
                    this.y = value;
                }
                else
                    this.x = value;
            }
        }

        /// <summary>
        ///   <para>Returns the length of this vector (Read Only).</para>
        /// </summary>
        public float magnitude
        {
            get { return Mathf.Sqrt((float) (this.x * this.x + this.y * this.y)); }
        }

        /// <summary>
        ///   <para>Returns the squared length of this vector (Read Only).</para>
        /// </summary>
        public int sqrMagnitude
        {
            get { return this.x * this.x + this.y * this.y; }
        }

        /// <summary>
        ///   <para>Returns the distance between a and b.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static float Distance(DVec2Int a, DVec2Int b)
        {
            return (a - b).magnitude;
        }

        /// <summary>
        ///   <para>Returns a vector that is made from the smallest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static DVec2Int Min(DVec2Int lhs, DVec2Int rhs)
        {
            return new DVec2Int(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y));
        }

        /// <summary>
        ///   <para>Returns a vector that is made from the largest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static DVec2Int Max(DVec2Int lhs, DVec2Int rhs)
        {
            return new DVec2Int(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y));
        }

        /// <summary>
        ///   <para>Multiplies two vectors component-wise.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static DVec2Int Scale(DVec2Int a, DVec2Int b)
        {
            return new DVec2Int(a.x * b.x, a.y * b.y);
        }

        /// <summary>
        ///   <para>Multiplies every component of this vector by the same component of scale.</para>
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(DVec2Int scale)
        {
            this.x *= scale.x;
            this.y *= scale.y;
        }

        /// <summary>
        ///   <para>Clamps the FVec2Int to the bounds given by min and max.</para>
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void Clamp(DVec2Int min, DVec2Int max)
        {
            this.x = Math.Max(min.x, this.x);
            this.x = Math.Min(max.x, this.x);
            this.y = Math.Max(min.y, this.y);
            this.y = Math.Min(max.y, this.y);
        }

        public static implicit operator Vector2(DVec2Int v)
        {
            return new Vector2((float) v.x, (float) v.y);
        }

        public static implicit operator DVec3Int(DVec2Int v)
        {
            return new DVec3Int(v.x, v.y, 0);
        }

        /// <summary>
        ///   <para>Converts a Vector2 to a FVec2Int by doing a Floor to each value.</para>
        /// </summary>
        /// <param name="v"></param>
        public static DVec2Int FloorToInt(Vector2 v)
        {
            return new DVec2Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y));
        }
        
        public static DVec2Int ToInt(DVec2 v)
        {
            return new DVec2Int(v.x, v.y);
        }

        /// <summary>
        ///   <para>Converts a  Vector2 to a FVec2Int by doing a Ceiling to each value.</para>
        /// </summary>
        /// <param name="v"></param>
        public static DVec2Int CeilToInt(Vector2 v)
        {
            return new DVec2Int(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y));
        }
        
        /// <summary>
        ///   <para>Converts a  Vector2 to a FVec2Int by doing a Round to each value.</para>
        /// </summary>
        /// <param name="v"></param>
        public static DVec2Int RoundToInt(Vector2 v)
        {
            return new DVec2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
        }

        public static DVec2Int operator +(DVec2Int a, DVec2Int b)
        {
            return new DVec2Int(a.x + b.x, a.y + b.y);
        }

        public static DVec2Int operator -(DVec2Int a, DVec2Int b)
        {
            return new DVec2Int(a.x - b.x, a.y - b.y);
        }

        public static DVec2Int operator -(DVec2Int a)
        {
            return new DVec2Int(-a.x, -a.y);
        }

        public static DVec2Int operator *(DVec2Int a, DVec2Int b)
        {
            return new DVec2Int(a.x * b.x, a.y * b.y);
        }

        public static DVec2Int operator *(DVec2Int a, int b)
        {
            return new DVec2Int(a.x * b, a.y * b);
        }

        public static DVec2Int operator *(DVec2Int a, double b)
        {
            return new DVec2Int((int) (a.x * b), (int) (a.y * b));
        }

        public static DVec2Int operator /(DVec2Int a, DVec2Int b)
        {
            return new DVec2Int(a.x / b.x, a.y / b.y);
        }

        public static DVec2Int operator /(DVec2Int a, int b)
        {
            return new DVec2Int(a.x / b, a.y / b);
        }

        public static bool operator ==(DVec2Int lhs, DVec2Int rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator !=(DVec2Int lhs, DVec2Int rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        ///   <para>Returns true if the objects are equal.</para>
        /// </summary>
        /// <param name="other"></param>
        public override bool Equals(object other)
        {
            return other is DVec2Int && this.Equals(other);
        }

        public bool Equals(DVec2Int other)
        {
            return this.x.Equals(other.x) && this.y.Equals(other.y);
        }

        /// <summary>
        ///   <para>Gets the hash code for the FVec2Int.</para>
        /// </summary>
        /// <returns>
        ///   <para>The hash code of the FVec2Int.</para>
        /// </returns>
        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
        }

        /// <summary>
        ///   <para>Returns a nicely formatted string for this vector.</para>
        /// </summary>
        public override string ToString()
        {
            return String.Format("({0}, {1})", (object) this.x, (object) this.y);
        }

        /// <summary>
        ///   <para>Shorthand for writing FVec2Int (0, 0).</para>
        /// </summary>
        public static DVec2Int zero
        {
            get { return DVec2Int.s_Zero; }
        }

        /// <summary>
        ///   <para>Shorthand for writing FVec2Int (1, 1).</para>
        /// </summary>
        public static DVec2Int one
        {
            get { return DVec2Int.s_One; }
        }

        /// <summary>
        ///   <para>Shorthand for writing FVec2Int (0, 1).</para>
        /// </summary>
        public static DVec2Int up
        {
            get { return DVec2Int.s_Up; }
        }

        /// <summary>
        ///   <para>Shorthand for writing FVec2Int (0, -1).</para>
        /// </summary>
        public static DVec2Int down
        {
            get { return DVec2Int.s_Down; }
        }

        /// <summary>
        ///   <para>Shorthand for writing FVec2Int (-1, 0).</para>
        /// </summary>
        public static DVec2Int left
        {
            get { return DVec2Int.s_Left; }
        }

        /// <summary>
        ///   <para>Shorthand for writing FVec2Int (1, 0).</para>
        /// </summary>
        public static DVec2Int right
        {
            get { return DVec2Int.s_Right; }
        }
    }
}