using System;
// using ProtoBuf;
using UnityEngine;

namespace LibBase.MathLite.Decimal
{
    [Serializable]
    // [ProtoContract]
    public struct DRectInt: IEquatable<DRectInt> {
        // [ProtoMember(1)]
        private int x;
        // [ProtoMember(2)]
        private int y;
        // [ProtoMember(3)]
        private int width;
        // [ProtoMember(4)]
        private int height;

        public DRectInt(int width, int height) : this(0, 0, width, height) {
        }


        public DRectInt(int x, int y, int width, int height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        ///   <para>Creates a rectangle given a size and position.</para>
        /// </summary>
        /// <param name="position">The position of the minimum corner of the rect.</param>
        /// <param name="size">The width and height of the rect.</param>
        public DRectInt(DVec2Int position, DVec2Int size) {
            this.x = position.x;
            this.y = position.y;
            this.width = size.x;
            this.height = size.y;
        }

        /// <summary>
        ///   <para></para>
        /// </summary>
        /// <param name="source"></param>
        public DRectInt(DRectInt source) {
            this.x = source.x;
            this.y = source.y;
            this.width = source.width;
            this.height = source.height;
        }

        public void SetRect(int x, int y, int width, int height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public DRectInt LeftBottomRect {
            get { return new DRectInt(Left, CenterY, width / 2, height / 2); }
        }

        public DRectInt RightBottomRect {
            get { return new DRectInt(CenterX, CenterY, width / 2, height / 2); }
        }

        public DRectInt LeftTopRect {
            get { return new DRectInt(Left, Top, width / 2, height / 2); }
        }

        public DRectInt RightTopRect {
            get { return new DRectInt(CenterX, Top, width / 2, height / 2); }
        }

        public Vector2Int Center {
            get { return new Vector2Int(x + width / 2, y + height / 2); }
        }

        public int CenterX {
            get { return x + width / 2; }
        }

        public int CenterY {
            get { return y + height / 2; }
        }

        public int X {
            get { return x; }
            set { x = value; }
        }

        public int Y {
            get { return y; }
            set { y = value; }
        }

        public int XMax {
            get { return x + width; }
        }

        public int YMax {
            get { return y + height; }
        }

        public int Left {
            get { return x; }
        }

        public int Right {
            get { return x + width; }
        }

        public int Top {
            get { return y; }
        }

        public int Bottom {
            get { return y + height; }
        }

        public int Width {
            get { return width; }
            set { width = value; }
        }

        public int Height {
            get { return height; }
            set { height = value; }
        }


        /// <summary>
        ///   <para>Returns true if the x and y components of point is a point inside this rectangle. If allowInverse is present and true, the width and height of the Rect are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.</para>
        /// </summary>
        /// <param name="point">Point to test.</param>
        /// <param name="allowInverse">Does the test allow the Rect's width and height to be negative?</param>
        /// <returns>
        ///   <para>True if the point lies within the specified rectangle.</para>
        /// </returns>
        public bool Contains(DVec2Int point) {
            return point.x >= this.x && point.x < this.XMax && point.y >= this.y && point.y < this.YMax;
        }

        /// <summary>
        ///   <para>Returns true if the x and y components of point is a point inside this rectangle. If allowInverse is present and true, the width and height of the Rect are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.</para>
        /// </summary>
        /// <param name="point">Point to test.</param>
        /// <param name="allowInverse">Does the test allow the Rect's width and height to be negative?</param>
        /// <returns>
        ///   <para>True if the point lies within the specified rectangle.</para>
        /// </returns>
        public bool Contains(DVec3Int point) {
            return point.x >= this.x && point.x < this.XMax && point.y >= this.y && point.y < this.YMax;
        }

        /// <summary>
        ///   <para>Returns true if the x and y components of point is a point inside this rectangle. If allowInverse is present and true, the width and height of the Rect are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.</para>
        /// </summary>
        /// <param name="point">Point to test.</param>
        /// <param name="allowInverse">Does the test allow the Rect's width and height to be negative?</param>
        /// <returns>
        ///   <para>True if the point lies within the specified rectangle.</para>
        /// </returns>
        public bool Contains(DVec3Int point, bool allowInverse) {
            if (!allowInverse) return this.Contains(point);
            bool flag = false;
            if (this.width < 0.0 && point.x <= this.x && point.x > this.XMax ||
                this.width >= 0.0 && point.x >= this.x && point.x < this.XMax)
                flag = true;
            return flag && ((double) this.height < 0.0 && point.y <= this.y && point.y > this.YMax ||
                            this.height >= 0.0 && point.y >= this.y && point.y < this.YMax);
        }

        public bool Contains(DRectInt rect) {
            return Left < Right && Bottom > Top && Left <= rect.Left && Top <= rect.Top && Right >= rect.Right &&
                   Bottom >= rect.Bottom;
        }

        public bool Overlaps(DRectInt other) {
            return other.Right > this.Left && other.Left < this.Right && other.Bottom > this.Top &&
                   other.Top < this.Bottom;
        }


        public static bool operator !=(DRectInt lhs, DRectInt rhs) {
            return !(lhs == rhs);
        }

        public static bool operator ==(DRectInt lhs, DRectInt rhs) {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
        }

        public override int GetHashCode() {
            return this.x.GetHashCode() ^ this.width.GetHashCode() << 2 ^ this.y.GetHashCode() >> 2 ^
                   this.height.GetHashCode() >> 1;
        }

        public override bool Equals(object other) {
            return other is DRectInt && this.Equals((DRectInt)other);
        }

        public bool Equals(DRectInt other) {
            return x.Equals(other.x) && y.Equals(other.y) && width.Equals(other.width) && height.Equals(other.height);
        }

        public override string ToString() {
            return string.Format("(x:{0:F2}, y:{1:F2}, width:{2:F2}, height:{3:F2})", (object) x, (object) y,
                (object) width, (object) height);
        }
    }
}