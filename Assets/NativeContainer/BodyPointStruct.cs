using Unity.Mathematics;

namespace NativeContainer
{
    /// <summary>
    /// 蛇小节点
    /// </summary>
    public struct BodyPointStruct
    {
        public float3 Pos;

        // public int Index;
        public float Radian;
        public int PlayerId;
        public int Num; // 重合的点的数量
        public float alpha;
        public float collideRadius;
        public bool isProtected;
        public float Width;
        public int FrameIndex;
        
        public float SmoothLCos;
        public float SmoothLSin;

        public float SmoothRCos;
        public float SmoothRSin;
        public bool IsSmoothCSDirty;
        
        //记录点到地图边界的最近距离，用于计算残骸生成
        public float NearestDistanceToBounds;
        
        public long Index;
        
        public void CheckSmoothCSRadian() {
            if (IsSmoothCSDirty) 
            {
                var ldir = Radian + math.PI / 2;
                SmoothLCos = math.cos(ldir);
                SmoothLSin = math.sin(ldir);
                var rdir = Radian - math.PI / 2;
                SmoothRCos = math.cos(rdir);
                SmoothRSin = math.sin(rdir);
                IsSmoothCSDirty = false;
            }
        }
    }
}