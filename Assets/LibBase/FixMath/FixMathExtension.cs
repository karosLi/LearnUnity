using UnityEngine;

namespace LibBase.MathLite.FixMath
{
    public static class FixMathExtension
    {
        public static Vector3 ToVector3(this FVec2 pos)
        {
            return new Vector3(FixFloat.ToFloat(pos.x),  FixFloat.ToFloat(pos.y), 0);
        }
        
        public static Vector3 ToVector3(this FVec3 pos)
        {
            return new Vector3(FixFloat.ToFloat(pos.x),  FixFloat.ToFloat(pos.y), FixFloat.ToFloat(pos.z));
        }
        
        public static float ToFloat(this FixFloat value)
        {
            return FixFloat.ToFloat(value);
        }

        public static bool CheckPointDistance(FVec3 point1, FVec3 point2, FixFloat radius1, FixFloat radius2)
        {
            FixFloat radiusPow2 = (radius1 + radius2) * (radius1 + radius2);
            FixFloat disPow2 = (point1.x - point2.x) * (point1.x - point2.x) + (point1.y - point2.y) * (point1.y - point2.y);
            return disPow2 <= radiusPow2;
        }
    }
}