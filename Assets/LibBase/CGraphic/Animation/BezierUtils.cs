using LibBase.MathLite.FixMath;
using UnityEngine;

namespace CGraphic.Animation {
    public class BezierUtils {

        /// <summary>
        /// 线性贝塞尔公式(B(t) = P0 + (P1 - P0)t = (1-t)P0 + tP1 ,t = [0-1])
        /// </summary>
        /// <param name="t">[0,1]</param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static float CalculateLinearBezier(float t, float p0, float p1) {
            return (1 - t) * p0 + t * p1;
        }

        /// <summary>
        /// 线性贝塞尔公式Fix 线性贝塞尔公式(B(t) = P0 + (P1 - P0)t = (1-t)P0 + tP1 ,t = [0-1])
        /// </summary>
        /// <param name="t">[0,1]</param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static FixFloat CalculateLinearBezier(FixFloat t, FixFloat p0, FixFloat p1) {
            return (1 - t) * p0 + t * p1; 
        }

        /// <summary>
        /// 线性贝塞尔公式(B(t) = P0 + (P1 - P0)t = (1-t)P0 + tP1 ,t = [0-1])
        /// </summary>
        /// <param name="t">[0,1]</param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static Vector3 CalculateLinearBezier(float t, Vector3 p0, Vector3 p1) {
            return (1 - t) * p0 + t * p1;
        }

        /// <summary>
        /// 二次方贝兹曲线的路径由给定点P0、P1、P2控制，(B(t) = (1 - t) * (1 - t) * p0 + 2 * t * (1 - t) * p1 + t * t * p2)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float CalculateCubicBezier(float t, float p0, float p1, float p2) {
            return (1 - t) * (1 - t) * p0 + 2 * t * (1 - t) * p1 + t * t * p2;
        }
        
        public static FixFloat CalculateCubicBezier(FixFloat t, FixFloat p0, FixFloat p1, FixFloat p2) {
            return (1 - t) * (1 - t) * p0 + 2 * t * (1 - t) * p1 + t * t * p2;
        }
        
        public static Vector3 CalculateCubicBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2) {
            return (1 - t) * (1 - t) * p0 + 2 * t * (1 - t) * p1 + t * t * p2;
        }
        
        /// <summary>
        /// P0、P1、P2、P3四个点在平面或在三维空间中定义了三次方贝兹曲线(B(t) = p0 * (1 - t) * (1 - t) * (1 - t) + 3 * p1 * t * (1 - t) * (1 - t) + 3 * p2 * t * t * (1 - t) + p3 * t * t * t)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public static float CalculateCubicBezier(float t, float p0, float p1, float p2, float p3) {
            return p0 * (1 - t) * (1 - t) * (1 - t) + 3 * p1 * t * (1 - t) * (1 - t) + 3 * p2 * t * t * (1 - t) + p3 * t * t * t;
        }
        
        public static FixFloat CalculateCubicBezier(FixFloat t, FixFloat p0, FixFloat p1, FixFloat p2, FixFloat p3) {
            return p0 * (1 - t) * (1 - t) * (1 - t) + 3 * p1 * t * (1 - t) * (1 - t) + 3 * p2 * t * t * (1 - t) + p3 * t * t * t;
        }
        
        public static Vector3 CalculateCubicBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
            return p0 * ((1 - t) * (1 - t) * (1 - t)) + p1 * (3 * t * (1 - t) * (1 - t)) + p2 * (3 * t * t * (1 - t)) + p3 * (t * t * t);
        }
    }
}