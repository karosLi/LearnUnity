namespace LibBase.MathLite
{
    public struct RandR
    {
        public uint seed;

        public RandR(uint initialSeed)
        {
            seed = initialSeed;
        }

        public int Next1()
        {
            uint next = seed;
            int result;

            next = next * 1103515245 + 12345;
            result = (int)((next / 65536) % 2048);

            next = next * 1103515245 + 12345;
            result <<= 10;
            result ^= (int)((next / 65536) % 1024);

            next = next * 1103515245 + 12345;
            result <<= 10;
            result ^= (int)((next / 65536) % 1024);

            seed = next; // Update the seed in the struct
            return result;
        }

        // 和 iOS 的 rand_r 实现一致
        public int Next()
        {
            long k;
            long s = seed;
            if (s == 0)
                s = 0x12345987;
            k = s / 127773;
            s = 16807 * (s - k * 127773) - 2836 * k;
            if (s < 0)
                s += 2147483647;
            seed = (uint)s;
            return (int)(seed & 0x7fffffff); // RAND_MAX = 0x7fffffff
        }
    }
}