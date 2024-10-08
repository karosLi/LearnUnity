using System;

namespace LibBase.MathLite.FixMath {
    public struct FRect : IEquatable<FRect> {
        private FixFloat x;
        private FixFloat y;
        private FixFloat width;
        private FixFloat height;

        public FRect(FixFloat width, FixFloat height) : this(0, 0, width, height) {
        }


        public FRect(FixFloat x, FixFloat y, FixFloat width, FixFloat height) {
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
        public FRect(FVec2 position, FVec2 size) {
            this.x = position.x;
            this.y = position.y;
            this.width = size.x;
            this.height = size.y;
        }

        /// <summary>
        ///   <para></para>
        /// </summary>
        /// <param name="source"></param>
        public FRect(FRect source) {
            this.x = source.x;
            this.y = source.y;
            this.width = source.width;
            this.height = source.height;
        }

        public void SetRect(FixFloat x, FixFloat y, FixFloat width, FixFloat height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public FRect LeftBottomRect {
            get { return new FRect(Left, CenterY, width / 2, height / 2); }
        }

        public FRect RightBottomRect {
            get { return new FRect(CenterX, CenterY, width / 2, height / 2); }
        }

        public FRect LeftTopRect {
            get { return new FRect(Left, Top, width / 2, height / 2); }
        }

        public FRect RightTopRect {
            get { return new FRect(CenterX, Top, width / 2, height / 2); }
        }

        public FVec2 Center {
            get { return new FVec2(x + width / 2, y + height / 2); }
        }

        public FixFloat CenterX {
            get { return x + width / 2; }
        }

        public FixFloat CenterY {
            get { return y + height / 2; }
        }

        public FixFloat X {
            get { return x; }
            set { x = value; }
        }

        public FixFloat Y {
            get { return y; }
            set { y = value; }
        }

        public FixFloat XMax {
            get { return x + width; }
        }

        public FixFloat YMax {
            get { return y + height; }
        }

        public FixFloat Left {
            get { return x; }
        }

        public FixFloat Right {
            get { return x + width; }
        }

        public FixFloat Top {
            get { return y; }
        }

        public FixFloat Bottom {
            get { return y + height; }
        }

        public FixFloat Width {
            get { return width; }
            set { width = value; }
        }

        public FixFloat Height {
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
        public bool Contains(FVec2 point) {
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
        public bool Contains(FVec3 point) {
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
        public bool Contains(FVec3 point, bool allowInverse) {
            if (!allowInverse) return this.Contains(point);
            bool flag = false;
            if (this.width < 0.0 && point.x <= this.x && point.x > this.XMax ||
                this.width >= 0.0 && point.x >= this.x && point.x < this.XMax)
                flag = true;
            return flag && ((double) this.height < 0.0 && point.y <= this.y && point.y > this.YMax ||
                            this.height >= 0.0 && point.y >= this.y && point.y < this.YMax);
        }

        public bool Contains(FRect rect) {
            return Left < Right && Bottom > Top && Left <= rect.Left && Top <= rect.Top && Right >= rect.Right &&
                   Bottom >= rect.Bottom;
        }

        public bool Overlaps(FRect other) {
            return other.Right > this.Left && other.Left < this.Right && other.Bottom > this.Top &&
                   other.Top < this.Bottom;
        }


        public static bool operator !=(FRect lhs, FRect rhs) {
            return !(lhs == rhs);
        }

        public static bool operator ==(FRect lhs, FRect rhs) {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
        }

        public override int GetHashCode() {
            return this.x.GetHashCode() ^ this.width.GetHashCode() << 2 ^ this.y.GetHashCode() >> 2 ^
                   this.height.GetHashCode() >> 1;
        }

        public override bool Equals(object other) {
            return other is FRect && this.Equals((FRect)other);
        }

        public bool Equals(FRect other) {
            return x.Equals(other.x) && y.Equals(other.y) && width.Equals(other.width) && height.Equals(other.height);
        }

        public override string ToString() {
            return string.Format("(x:{0:F2}, y:{1:F2}, width:{2:F2}, height:{3:F2})", (object) x, (object) y,
                (object) width, (object) height);
        }
    }
}