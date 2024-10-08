namespace LibBase.MathLite {
    public class CRandom {
        #region 默认实例

        public static CRandom Default = new CRandom();

        #endregion

        #region 线性同余参数

        //线性同余随机数生成算法
        private const int PrimeA = 214013;
        private const int PrimeB = 2531011;

        #endregion

        //归一化
        private const float Mask15Bit_1 = 1.0f / 0x7fff;
        private const int Mask15Bit = 0x7fff;

        private int _seed = 0;

        public int Seed {
            set { _seed = value; }
            get { return _seed; }
        }

        /// <summary>
        /// 采用线性同余算法产生一个0~1之间的随机小数
        /// </summary>
        /// <returns></returns>
        public float Random() {
            float val = ((((_seed = _seed * PrimeA + PrimeB) >> 16) & Mask15Bit) - 1) * Mask15Bit_1;
            return (val > 0.99999f ? 0.99999f : val);
        }

        public float Range(float min, float max) {
            return min + Random() * (max - min);
        }

        public int Range(int min, int max) {
            return (int) (min + Random() * (max - min));
        }


        public float Range(float max) {
            return Range(0, max);
        }

        public int Range(int max) {
            return Range(0, max);
        }
    }
}