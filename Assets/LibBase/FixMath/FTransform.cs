using System;
using UnityEngine;

namespace LibBase.MathLite.FixMath
{
    [Serializable]
    public struct FTransform
    {
        [SerializeField]
        public FVec3 postion;
        [SerializeField]
        public FVec3 eulerAngles;
    }
}