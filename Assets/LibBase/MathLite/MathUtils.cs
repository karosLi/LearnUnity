using System;
using LibBase.MathLite.FixMath;
using UnityEngine;

namespace LibBase.MathLite
{
    public class MathUtils {
        public static readonly double PI2 = Math.PI * 2;
        public static readonly double PI_Half = Math.PI * 0.5;
        public static readonly double Rad2Deg = 180 / Math.PI;
        private static readonly double float_equals_precision = 0.000001;

        public static bool IsZero(double v) {
            return Math.Abs(v) < float_equals_precision;
        }
        
        public static bool IsEqual(float v1, float v2) {
            return Math.Abs(v1 - v2) < float_equals_precision;
        }
        
        /// <summary>
        /// [0~360]
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static double Angle(Vector3 from, Vector3 to) {
            //两点的x、y值
            float x = to.x - from.x;
            float y = to.y - from.y;

            //斜边长度
            double hypotenuse = Math.Sqrt(Math.Pow(x, 2f) + Math.Pow(y, 2f));

            //求出弧度
            double cos = x / hypotenuse;
            double radian = Math.Acos(cos);

            if (y < 0) {
                radian = -radian;
            } else if (IsZero(y) && (x < 0)) {
                radian = Mathf.PI;
            }
            if (radian < 0) radian += Mathf.PI * 2;
            //用弧度算出角度    
            var angle = Rad2Deg * radian;
            return angle;
        }

        public static int Angle(FVec3 from, FVec3 to) {
            var temp = to - from;
            FixFloat hypotenuse = FixFloat.Sqrt(FixFloat.Pow2(temp.x) + FixFloat.Pow2(temp.y));
            FixFloat cos = temp.x / hypotenuse;
            FixFloat radian = FixFloat.ACos(cos);
            if (temp.y < FixFloat.Zero) {
                radian = -radian;
            } else if (temp.y.IsZero() && (temp.x < FixFloat.Zero)) {
                radian = FixFloat.Pi;
            }
            if (radian < FixFloat.Zero) radian += FixFloat.Pi * 2;
            //用弧度算出角度    
            var angle = FixFloat.Rad2Deg * radian;
            return (int)angle;
        }

        /// <summary>
        /// dir [0 360]
        /// </summary>
        /// <param name="currentDir"></param>
        /// <param name="targetDir"></param>
        /// <returns></returns>
        public static int GetLimitDegree(int currentDir, int targetDir, int maxDegree)
        {
            if (currentDir == targetDir) return targetDir;
            var delta = Math.Abs(currentDir - targetDir);
            if (delta <= maxDegree) return targetDir;
            var delta2 = Math.Abs(delta - 360);
            if (delta2 <= maxDegree) return targetDir;
            if (delta <= 180) return currentDir - targetDir > 0 ? currentDir - maxDegree : currentDir + maxDegree;
            int destDir = currentDir < 180 ? currentDir - maxDegree : currentDir + maxDegree;
            if (destDir < 0) destDir += 360;
            if (destDir >= 360) destDir -= 360;
            return destDir;
        }
        
        
    }
}