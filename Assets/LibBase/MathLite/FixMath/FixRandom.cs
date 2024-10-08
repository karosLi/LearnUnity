using System;

namespace LibBase.MathLite.FixMath 
{
public class FixRandom {
    private System.Random m_random;

    public FixRandom() {
        m_random = new System.Random();
    }

    public FixRandom(int seed) {
        m_random = new System.Random(seed);
    }

    public int NextInt() {
        return m_random.Next();
    }
    
    public int RangeInt(int min, int max) {
        return m_random.Next(min, max);
    }

    public FixFloat NextFloat() {
        return m_random.Next(0, 10000) / (FixFloat) 10000;
    }
    
    public FixFloat RangeFloat(FixFloat min, FixFloat max) {
        FixFloat factor = NextFloat();
        return min + (max - min) * factor;
    }
}
}