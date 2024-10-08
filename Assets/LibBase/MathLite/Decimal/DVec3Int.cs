using System;
// using ProtoBuf;
using UnityEngine;

namespace LibBase.MathLite.Decimal
{
    [Serializable]
    // [ProtoContract]
    public struct DVec3Int : IEquatable<DVec3Int>
    {
        // [ProtoMember(1)]
        private int m_X;
        // [ProtoMember(2)]
        private int m_Y;
        // [ProtoMember(3)]
        private int m_Z;
        private static readonly DVec3Int s_Zero = new DVec3Int(0, 0, 0);
        private static readonly DVec3Int s_One = new DVec3Int(1, 1, 1);
        private static readonly DVec3Int s_Up = new DVec3Int(0, 1, 0);
        private static readonly DVec3Int s_Down = new DVec3Int(0, -1, 0);
        private static readonly DVec3Int s_Left = new DVec3Int(-1, 0, 0);
        private static readonly DVec3Int s_Right = new DVec3Int(1, 0, 0);

        public DVec3Int(int x, int y, int z)
        {
            this.m_X = x;
            this.m_Y = y;
            this.m_Z = z;
        }

        public DVec3Int(DFloat x, DFloat y, DFloat z)
        {
            this.m_X = (int) x;
            this.m_Y = (int) y;
            this.m_Z = (int) z;
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
        ///   <para>Z component of the vector.</para>
        /// </summary>
        public int z
        {
            get { return this.m_Z; }
            set { this.m_Z = value; }
        }

        /// <summary>
        ///   <para>Set x, y and z components of an existing FVec3Int.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Set(int x, int y, int z)
        {
            this.m_X = x;
            this.m_Y = y;
            this.m_Z = z;
        }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.x;
                    case 1:
                        return this.y;
                    case 2:
                        return this.z;
                    default:
                        throw new IndexOutOfRangeException(string.Format("Invalid FVec3Int index addressed: {0}!",
                            (object) index));
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.x = value;
                        break;
                    case 1:
                        this.y = value;
                        break;
                    case 2:
                        this.z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException(string.Format("Invalid FVec3Int index addressed: {0}!",
                            (object) index));
                }
            }
        }

        /// <summary>
        ///   <para>Returns the length of this vector (Read Only).</para>
        /// </summary>
        public float magnitude
        {
            get { return Mathf.Sqrt((float) (this.x * this.x + this.y * this.y + this.z * this.z)); }
        }

        /// <summary>
        ///   <para>Returns the squared length of this vector (Read Only).</para>
        /// </summary>
        public int sqrMagnitude
        {
            get { return this.x * this.x + this.y * this.y + this.z * this.z; }
        }

        /// <summary>
        ///   <para>Returns the distance between a and b.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static float Distance(DVec3Int a, DVec3Int b)
        {
            return (a - b).magnitude;
        }

        /// <summary>
        ///   <para>Returns a vector that is made from the smallest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static DVec3Int Min(DVec3Int lhs, DVec3Int rhs)
        {
            return new DVec3Int(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y), Mathf.Min(lhs.z, rhs.z));
        }

        /// <summary>
        ///   <para>Returns a vector that is made from the largest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static DVec3Int Max(DVec3Int lhs, DVec3Int rhs)
        {
            return new DVec3Int(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y), Mathf.Max(lhs.z, rhs.z));
        }

        /// <summary>
        ///   <para>Multiplies two vectors component-wise.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static DVec3Int Scale(DVec3Int a, DVec3Int b)
        {
            return new DVec3Int(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        /// <summary>
        ///   <para>Multiplies every component of this vector by the same component of scale.</para>
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(DVec3Int scale)
        {
            this.x *= scale.x;
            this.y *= scale.y;
            this.z *= scale.z;
        }

        /// <summary>
        ///   <para>Clamps the FVec3Int to the bounds given by min and max.</para>
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void Clamp(DVec3Int min, DVec3Int max)
        {
            this.x = Math.Max(min.x, this.x);
            this.x = Math.Min(max.x, this.x);
            this.y = Math.Max(min.y, this.y);
            this.y = Math.Min(max.y, this.y);
            this.z = Math.Max(min.z, this.z);
            this.z = Math.Min(max.z, this.z);
        }

        public static implicit operator Vector3(DVec3Int v)
        {
            return new Vector3((float) v.x, (float) v.y, (float) v.z);
        }

        public static implicit operator DVec2Int(DVec3Int v)
        {
            return new DVec2Int(v.x, v.y);
        }

        /// <summary>
        ///   <para>Converts a  Vector3 to a FVec3Int by doing a Floor to each value.</para>
        /// </summary>
        /// <param name="v"></param>
        public static DVec3Int FloorToInt(Vector3 v)
        {
            return new DVec3Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y), Mathf.FloorToInt(v.z));
        }

        /// <summary>
        ///   <para>Converts a  Vector3 to a FVec3Int by doing a Ceiling to each value.</para>
        /// </summary>
        /// <param name="v"></param>
        public static DVec3Int CeilToInt(Vector3 v)
        {
            return new DVec3Int(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y), Mathf.CeilToInt(v.z));
        }

        /// <summary>
        ///   <para>Converts a  Vector3 to a FVec3Int by doing a Round to each value.</para>
        /// </summary>
        /// <param name="v"></param>
        public static DVec3Int RoundToInt(Vector3 v)
        {
            return new DVec3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
        }

        public static DVec3Int operator +(DVec3Int a, DVec3Int b)
        {
            return new DVec3Int(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static DVec3Int operator -(DVec3Int a, DVec3Int b)
        {
            return new DVec3Int(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static DVec3Int operator -(DVec3Int a)
        {
            return new DVec3Int(-a.x, -a.y, -a.z);
        }

        public static DVec3Int operator *(DVec3Int a, DVec3Int b)
        {
            return new DVec3Int(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static DVec3Int operator *(DVec3Int a, int b)
        {
            return new DVec3Int(a.x * b, a.y * b, a.z * b);
        }

        public static DVec3Int operator *(DVec3Int a, double b)
        {
            return new DVec3Int((int) (a.x * b), (int) (a.y * b), (int) (a.z * b));
        }

        public static DVec3Int operator /(DVec3Int a, DVec3Int b)
        {
            return new DVec3Int(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static DVec3Int operator /(DVec3Int a, int b)
        {
            return new DVec3Int(a.x / b, a.y / b, a.z / b);
        }

        public static bool operator ==(DVec3Int lhs, DVec3Int rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        public static bool operator !=(DVec3Int lhs, DVec3Int rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        ///   <para>Returns true if the objects are equal.</para>
        /// </summary>
        /// <param name="other"></param>
        public override bool Equals(object other)
        {
            return other is DVec3Int && this.Equals(other);
        }

        public bool Equals(DVec3Int other)
        {
            return this == other;
        }

        /// <summary>
        ///   <para>Gets the hash code for the FVec3Int.</para>
        /// </summary>
        /// <returns>
        ///   <para>The hash code of the FVec3Int.</para>
        /// </returns>
        public override int GetHashCode()
        {
            int hashCode1 = this.y.GetHashCode();
            int hashCode2 = this.z.GetHashCode();
            return this.x.GetHashCode() ^ hashCode1 << 4 ^ hashCode1 >> 28 ^ hashCode2 >> 4 ^ hashCode2 << 28;
        }

        /// <summary>
        ///   <para>Returns a nicely formatted string for this vector.</para>
        /// </summary>
        /// <param name="format"></param>
        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", (object) this.x, (object) this.y, (object) this.z);
        }

        /// <summary>
        ///   <para>Returns a nicely formatted string for this vector.</para>
        /// </summary>
        /// <param name="format"></param>
        public string ToString(string format)
        {
            return string.Format("({0}, {1}, {2})", (object) this.x.ToString(format),
                (object) this.y.ToString(format), (object) this.z.ToString(format));
        }

        /// <summary>
        ///   <para>Shorthand for writing FVec3Int (0, 0, 0).</para>
        /// </summary>
        public static DVec3Int zero
        {
            get { return DVec3Int.s_Zero; }
        }

        /// <summary>
        ///   <para>Shorthand for writing FVec3Int (1, 1, 1).</para>
        /// </summary>
        public static DVec3Int one
        {
            get { return DVec3Int.s_One; }
        }

        /// <summary>
        ///   <para>Shorthand for writing FVec3Int (0, 1, 0).</para>
        /// </summary>
        public static DVec3Int up
        {
            get { return DVec3Int.s_Up; }
        }

        /// <summary>
        ///   <para>Shorthand for writing FVec3Int (0, -1, 0).</para>
        /// </summary>
        public static DVec3Int down
        {
            get { return DVec3Int.s_Down; }
        }

        /// <summary>
        ///   <para>Shorthand for writing FVec3Int (-1, 0, 0).</para>
        /// </summary>
        public static DVec3Int left
        {
            get { return DVec3Int.s_Left; }
        }

        /// <summary>
        ///   <para>Shorthand for writing FVec3Int (1, 0, 0).</para>
        /// </summary>
        public static DVec3Int right
        {
            get { return DVec3Int.s_Right; }
        }
    }
}