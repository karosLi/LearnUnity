
namespace LibBase.MathLite.FixMath
{
    public class FixMath
    {
        public static readonly FixFloat PI = FixFloat.Pi;
        public static readonly FixFloat Deg2Rad = PI / 180;
        public static readonly FixFloat Rad2Deg = 180 / PI;
        public static readonly FixFloat PI2 = PI * 2;
        public static readonly FixFloat PI_Half = PI * 0.5;

        /// <summary>
        /// Returns the cosine of angle f
        /// </summary>
        /// <param name="radians">The input angle, in radians 弧度数</param>
        /// <returns>The return value between -1 and +1.</returns>
        public static FixFloat Cos(FixFloat radians)
        {
            return Cos((int) (Rad2Deg * radians));
        }

        /// <summary>
        /// Returns the sine of angle f
        /// </summary>
        /// <param name="radians">The input angle, in radians 弧度数</param>
        /// <returns>The return value between -1 and +1.</returns>
        public static FixFloat Sin(FixFloat radians)
        {
            return Sin((int) (Rad2Deg * radians));
        }

        /// <summary>
        /// Returns the cosine of angle f
        /// </summary>
        /// <param name="tAngle">The input angle, in degree.度数</param>
        /// <returns>The return value between -1 and +1.</returns>
        public static FixFloat Cos(int tAngle)
        {
            tAngle %= 360;
            if (tAngle > 180)
            {
                tAngle -= 360;
            }
            else if (tAngle < -180)
            {
                tAngle += 360;
            }

            // -180
            if (tAngle == 180 || tAngle == -180) return -FixFloat.One;
            if (tAngle < 0) tAngle = -tAngle;
            if (tAngle <= 90) return FixMathTable.Cos[tAngle];
            tAngle = 180 - tAngle;
            return -FixMathTable.Cos[tAngle];
        }


        /// <summary>
        /// Returns the sine of angle f
        /// </summary>
        /// <param name="tAngle">The input angle, in degree.度数</param>
        /// <returns>The return value between -1 and +1.</returns>
        public static FixFloat Sin(int tAngle)
        {
            tAngle %= 360;
            if (tAngle > 180)
            {
                tAngle -= 360;
            }
            else if (tAngle < -180)
            {
                tAngle += 360;
            }

            // -180
            if (tAngle == -180 || tAngle == 180) return FixFloat.Zero;
            bool isOverZero = true;
            if (tAngle < 0)
            {
                isOverZero = false;
                tAngle = -tAngle;
            }

            if (tAngle <= 90) return isOverZero ? FixMathTable.Sin[tAngle] : -FixMathTable.Sin[tAngle];
            tAngle = 180 - tAngle;
            return isOverZero ? FixMathTable.Sin[tAngle] : -FixMathTable.Sin[tAngle];
        }


        public static FixFloat Tan(int tAngle)
        {
            return Sin(tAngle) / Cos(tAngle);
        }


        /// <summary>
        /// 将角度控制在 -180 ~ 180
        /// </summary>
        /// <param name="tAngle"></param>
        /// <returns></returns>
        public static int ForceAngleRange(int tAngle)
        {
            tAngle %= 360;
            if (tAngle > 180)
            {
                tAngle -= 360;
            }
            else if (tAngle < -180)
            {
                tAngle += 360;
            }

            return tAngle;
        }

        /// <summary>
        /// return value range from [min,max]
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FixFloat ForceRange(FixFloat min, FixFloat max, FixFloat value)
        {
            if (value < min) value = min;
            if (value > max) value = max;
            return value;
        }

        /**
     * https://www.zhihu.com/question/24251545
     */
        public static bool RectCircleIntersect(FRect rect, FVec2 circle, FixFloat r)
        {
            FixFloat vx = FixFloat.Abs(circle.x - rect.CenterX);
            FixFloat vy = FixFloat.Abs(circle.y - rect.CenterY);
            FixFloat ux = FixFloat.Max(vx - rect.Width / 2, 0);
            FixFloat uy = FixFloat.Max(vy - rect.Height / 2, 0);
            return ux * ux + uy * uy <= r * r;
        }

    }
}