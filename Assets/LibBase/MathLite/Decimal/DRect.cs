using System;
// using ProtoBuf;

namespace LibBase.MathLite.Decimal {
    [Serializable]
    // [ProtoContract]
    public struct DRect : IEquatable<DRect> {
        // [ProtoMember(1)]
        private DFloat x;
        // [ProtoMember(2)]
        private DFloat y;
        // [ProtoMember(3)]
        private DFloat width;
        // [ProtoMember(4)]
        private DFloat height;

        public DRect(DFloat width, DFloat height) : this(0, 0, width, height) {
        }


        public DRect(DFloat x, DFloat y, DFloat width, DFloat height) {
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
        public DRect(DVec2 position, DVec2 size) {
            this.x = position.x;
            this.y = position.y;
            this.width = size.x;
            this.height = size.y;
        }

        /// <summary>
        ///   <para></para>
        /// </summary>
        /// <param name="source"></param>
        public DRect(DRect source) {
            this.x = source.x;
            this.y = source.y;
            this.width = source.width;
            this.height = source.height;
        }

        public void SetRect(DFloat x, DFloat y, DFloat width, DFloat height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public DRect LeftBottomRect {
            get { return new DRect(Left, CenterY, width / 2, height / 2); }
        }

        public DRect RightBottomRect {
            get { return new DRect(CenterX, CenterY, width / 2, height / 2); }
        }

        public DRect LeftTopRect {
            get { return new DRect(Left, Top, width / 2, height / 2); }
        }

        public DRect RightTopRect {
            get { return new DRect(CenterX, Top, width / 2, height / 2); }
        }

        public DVec2 Center {
            get { return new DVec2(x + width / 2, y + height / 2); }
        }

        public DFloat CenterX {
            get { return x + width / 2; }
        }

        public DFloat CenterY {
            get { return y + height / 2; }
        }

        public DFloat X {
            get { return x; }
            set { x = value; }
        }

        public DFloat Y {
            get { return y; }
            set { y = value; }
        }

        public DFloat XMax {
            get { return x + width; }
        }

        public DFloat YMax {
            get { return y + height; }
        }

        public DFloat Left {
            get { return x; }
        }

        public DFloat Right {
            get { return x + width; }
        }

        public DFloat Top {
            get { return y; }
        }

        public DFloat Bottom {
            get { return y + height; }
        }

        public DFloat Width {
            get { return width; }
            set { width = value; }
        }

        public DFloat Height {
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
        public bool Contains(DVec2 point) {
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
        public bool Contains(DVec3 point) {
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
        public bool Contains(DVec3 point, bool allowInverse) {
            if (!allowInverse) return this.Contains(point);
            bool flag = false;
            if (this.width < 0.0 && point.x <= this.x && point.x > this.XMax ||
                this.width >= 0.0 && point.x >= this.x && point.x < this.XMax)
                flag = true;
            return flag && ((double) this.height < 0.0 && point.y <= this.y && point.y > this.YMax ||
                            this.height >= 0.0 && point.y >= this.y && point.y < this.YMax);
        }

        public bool Contains(DRect rect) {
            return Left < Right && Bottom > Top && Left <= rect.Left && Top <= rect.Top && Right >= rect.Right &&
                   Bottom >= rect.Bottom;
        }

        public bool Overlaps(DRect other) {
            return other.Right > this.Left && other.Left < this.Right && other.Bottom > this.Top &&
                   other.Top < this.Bottom;
        }


        public static bool operator !=(DRect lhs, DRect rhs) {
            return !(lhs == rhs);
        }

        public static bool operator ==(DRect lhs, DRect rhs) {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
        }

        public override int GetHashCode() {
            return this.x.GetHashCode() ^ this.width.GetHashCode() << 2 ^ this.y.GetHashCode() >> 2 ^
                   this.height.GetHashCode() >> 1;
        }

        public override bool Equals(object other) {
            return other is DRect && this.Equals((DRect)other);
        }

        public bool Equals(DRect other) {
            return x.Equals(other.x) && y.Equals(other.y) && width.Equals(other.width) && height.Equals(other.height);
        }

        public override string ToString() {
            return string.Format("(x:{0:F2}, y:{1:F2}, width:{2:F2}, height:{3:F2})", (object) x, (object) y,
                (object) width, (object) height);
        }
    }
}