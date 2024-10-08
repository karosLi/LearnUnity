using System;
using UnityEngine;

namespace LibBase.MathLite.Decimal
{
    public class DMath
    {
        public static readonly DFloat PI = DFloat.Pi;
        public static readonly DFloat Deg2Rad = PI / 180;
        public static readonly DFloat Rad2Deg = 180 / PI;
        public static readonly DFloat PI2 = PI * 2;
        public static readonly DFloat PI_Half = PI * 0.5;


        public static DFloat Cos(int degree)
        {
            degree %= 360;
            if (degree > 360) degree -= 360;
            if (degree < 0) degree += 360;
            return DMathTable.Cos[degree];
        }

        public static DFloat Sin(int degree)
        {
            degree %= 360;
            if (degree > 360) degree -= 360;
            if (degree < 0) degree += 360;
            return DMathTable.Sin[degree];
        }

        
        /// <summary>
        /// return value range from [min,max]
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DFloat Clamp(DFloat min, DFloat max, DFloat value)
        {
            if (value < min) value = min;
            else if (value > max) value = max;
            return value;
        }

        public static int Clamp(int min, int max, int value)
        {
            if (value < min) value = min;
            else if (value > max) value = max;
            return value;
        }

        public static DFloat Pow2(DFloat a)
        {
            return a * a;
        }

        public static long Pow2(int a)
        {
            return a * a;
        }

        public static long DisPow2(DVec2Int from, DVec2Int to)
        {
            var temp = from - to;
            return temp.x * temp.x + temp.y * temp.y;
        }

        public static long DisPow2(DVec3Int from, DVec3Int to)
        {
            var temp = from - to;
            return temp.x * temp.x + temp.y * temp.y + temp.z * temp.z;
        }
        
        public static DFloat DisPow2(DVec3 from, DVec3 to)
        {
            var temp = from - to;
            return temp.x * temp.x + temp.y * temp.y + temp.z * temp.z;
        }

        public static DFloat Min(DFloat a, DFloat b)
        {
            return a < b ? a : b;
        }

        public static DFloat Max(DFloat a, DFloat b)
        {
            return a < b ? b : a;
        }
        
        public static int Min(int a, int b)
        {
            return a < b ? a : b;
        }

        public static int Max(int a, int b)
        {
            return a < b ? b : a;
        }

        public static int Angle(DVec3Int from, DVec3Int to)
        {
            var temp = to - from;
            DFloat hypotenuse = DFloat.Sqrt(DFloat.Pow2(temp.x) + DFloat.Pow2(temp.y));
            DFloat cos = temp.x / hypotenuse;
            DFloat radian = DFloat.ACos(cos);
            if (temp.y < DFloat.Zero)
            {
                radian = -radian;
            }
            else if (temp.y == 0 && (temp.x < 0))
            {
                radian = DFloat.Pi;
            }

            if (radian < 0) radian += DFloat.Pi * 2;
            //用弧度算出角度    
            var angle = DFloat.Rad2Deg * radian;
            return (int) angle;
        }

        public static int Angle(DVec3 from, DVec3 to)
        {
            var temp = to - from;
            DFloat hypotenuse = DFloat.Sqrt(DFloat.Pow2(temp.x) + DFloat.Pow2(temp.y));
            DFloat cos = temp.x / hypotenuse;
            DFloat radian = DFloat.ACos(cos);
            if (temp.y < DFloat.Zero)
            {
                radian = -radian;
            }
            else if (temp.y == DFloat.Zero && (temp.x < DFloat.Zero))
            {
                radian = DFloat.Pi;
            }

            if (radian < DFloat.Zero) radian += DFloat.TwoPi;
            //用弧度算出角度    
            var angle = DFloat.Rad2Deg * radian;
            return (int) angle;
        }

        /**
     * https://www.zhihu.com/question/24251545
     */
        public static bool RectCircleIntersect(DRect rect, DVec2 circle, DFloat r)
        {
            DFloat vx = DFloat.Abs(circle.x - rect.CenterX);
            DFloat vy = DFloat.Abs(circle.y - rect.CenterY);
            DFloat ux = DFloat.Max(vx - rect.Width / 2, 0);
            DFloat uy = DFloat.Max(vy - rect.Height / 2, 0);
            return ux * ux + uy * uy <= r * r;
        }

        public static bool RectCircleIntersect(DRectInt rect, DVec2Int circle, int r)
        {
            int vx = Math.Abs(circle.x - rect.CenterX);
            int vy = Math.Abs(circle.y - rect.CenterY);
            int ux = Math.Max(vx - rect.Width / 2, 0);
            int uy = Math.Max(vy - rect.Height / 2, 0);
            return ux * ux + uy * uy <= r * r;
        }
    }
}