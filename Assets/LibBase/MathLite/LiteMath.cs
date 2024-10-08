using System;
using UnityEngine;

namespace LibBase.MathLite
{
    public class LiteMath
    {
        public static readonly float PI2 = Mathf.PI * 2;
        public static readonly float PI_Half = Mathf.PI * 0.5f;
        public static readonly float Rad2Deg = 180 / Mathf.PI;
        public static readonly float Deg2Rad = Mathf.PI / 180;
        private static readonly double float_equals_precision = 0.000001;

        public static bool IsZero(double v) 
        {
            return Math.Abs(v) < float_equals_precision;
        }

        public static bool IsEqual(float v1, float v2)
        {
            return Math.Abs(v1 - v2) < float_equals_precision;
        }

        /// <summary>
        /// [0~360]
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static int GetDegree(Vector2 from, Vector2 to)
        {
            return (int) (GetRadian(from, to) * Mathf.Rad2Deg);
        }

        public static float GetRadian(Vector2 from, Vector2 to)
        {
            var radian = Mathf.Atan2(from.y - to.y, to.x - from.x);
            if (radian < 0) radian += PI2;
            if (radian > PI2) radian -= PI2;
            return PI2 - radian;
        }

        //返回弧度 0~2PI
        public static float GetLerpAngle(Vector3 from, Vector3 to)
        {
            float dx = to.x - from.x;
            float dy = from.y - to.y;
            float r = GetVectorRadians(dx, dy, 1, 0);
            if (dy < 0) r = PI2 - r;
            return PI2 - r;
        }

        private static float GetVectorRadians(float x1, float y1, float x2, float y2)
        {
            var dotProduct = x1 * x2 + y1 * y2;
            var s1 = Mathf.Sqrt(x1 * x1 + y1 * y1);
            var s2 = Mathf.Sqrt(x2 * x2 + y2 * y2);
            var cosa = dotProduct / (s1 * s2);
            return Mathf.Acos(cosa);
        }
    }
}