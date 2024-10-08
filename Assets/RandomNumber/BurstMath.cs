using LibBase.MathLite;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace RandomNumber
{
    public unsafe struct RandomPtr
    {
        public Random* Ptr;
    }

    [BurstCompile]
    public unsafe static class BurstMath
    {
        public static readonly RandomPtr RandomPtr = NewRandomPtr();

        public static RandomPtr NewRandomPtr()
        {
            Unity.Mathematics.Random random = new Unity.Mathematics.Random(1000);
            return new RandomPtr()
            {
                Ptr = (Unity.Mathematics.Random*)UnsafeUtility.AddressOf(ref random)
            };
        }

        // private static readonly RandR RandR = new RandR(1000);
        
        [BurstCompile]
        public static float RandomWithMinMax(ref uint seed, float min, float max)
        {
            Random r = new Random(seed);
            int m = 1000000;
            float result = (r.NextFloat(0, m)) * 1.0f / m;
            seed = r.state;
            return min + (max - min) * result;
        }
        
        [BurstCompile]
        public static float RandomWithMinMax(float min, float max)
        {
            int m = 1000000;
            float result = (RandomPtr.Ptr->NextFloat(0, m)) * 1.0f / m;
            return min + (max - min) * result;
        }
    }
}


