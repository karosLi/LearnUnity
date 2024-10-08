using System;
using System.Collections.Generic;
using Unity.Collections;

namespace LibBase.MathLite.FixMath
{
    public partial struct Random
    {
        public ulong randSeed;

        public Random(uint seed = 17)
        {
            randSeed = seed;
        }

        public FixFloat value => new FixFloat(Range(0, 1000));

        public uint Next()
        {
            randSeed = randSeed * 1103515245 + 36153;
            return (uint) (randSeed / 65536);
        }

        public ulong NextLong()
        {
            randSeed = randSeed * 1103515245 + 36153;
            return randSeed / 65536;
        }
        
        // range:[0 ~(max-1)]
        public uint Next(uint max)
        {
            return Next() % max;
        }

        public int Next(int max)
        {
            return (int) (Next() % max);
        }

        // range:[min~(max-1)]
        public uint Range(uint min, uint max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException("minValue",
                    string.Format("'{0}' cannot be greater than {1}.", min, max));

            uint num = max - min;
            return this.Next(num) + min;
        }

        public int Range(int min, int max)
        {
            if (min >= max - 1)
                return min;
            int num = max - min;

            return this.Next(num) + min;
        }

        // public FixFloat Next(FixFloat minValue, FixFloat maxValue)
        // {
        //     minValue = minValue * FixFloat.Thousand;
        //     maxValue = maxValue * FixFloat.Thousand;
        //     ulong num = (ulong) ((maxValue - minValue).AsLong());
        //     FixFloat result = (minValue + (NextLong() % num)) / FixFloat.Thousand;
        //     return result;
        // }
        //
        public FixFloat NextFixFloat() {
            return ((FixFloat) Next()) / (MaxRandomInt);
        }
        
        public static int MaxRandomInt { get { return 0x7fffffff; } }
        
        // public FVec2 Range(FVec2 min, FVec2 max)
        // {
        //     return new FVec2(Next(min.x, max.x), Next(min.y, max.y));
        // }
        //
        // public FVec2 RangeInsideUnitCircle(FixFloat radius)
        // {
        //     int angle = Range(0, 360);
        //     FixFloat x = radius * FixMath.Cos(angle, FixMathUtil.CosTable);
        //     FixFloat y = radius * FixMath.Sin(angle, FixMathUtil.SinTable);
        //     FixFloat s = new FixFloat(Range(0, 1000), 1000);
        //     return new FVec2(x * s, y * s);
        // }

        // public List<FVec2> RangeInsideUnitCircles(FixFloat radius, FixFloat[] circleRadius)
        // {
        //     //存储已随机出来的圆心点
        //     List<FVec2> circles = new List<FVec2>(circleRadius.Length);
        //     int loopCount = 0;
        //     while (circles.Count < circleRadius.Length && loopCount < 10000)
        //     {
        //         FVec2 pos = RangeInsideUnitCircle(radius - circleRadius[0]);
        //         bool collider = false;
        //         for (int i = 0; i < circles.Count; i++)
        //         {
        //             FVec2 d = pos - circles[i];
        //             if (d.magnitude < (circleRadius[i] + circleRadius[circles.Count]))
        //             {
        //                 collider = true;
        //                 break;
        //             }
        //         }
        //
        //         if (!collider)
        //         {
        //             circles.Add(pos);
        //         }
        //
        //         loopCount++;
        //     }
        //
        //     if (circles.Count < circleRadius.Length)
        //     {
        //         Log.Warning("随机了10000次，还是放不下去了");
        //         int addNum = circleRadius.Length - circles.Count;
        //         for (int i = 0; i < addNum; i++)
        //         {
        //             circles.Add(circles.Count > 0 ? circles[0] : FVec2.zero);
        //         }
        //     }
        //
        //     return circles;
        // }

        //圆圈上
        /*public FVec2 RangeInsideUnitCircle(FixFloat radius)
        {
            FixFloat radin = Range(FixFloat.Zero, new FixFloat(2) * FixMath.Pi);
            FixFloat x = radius * FixMath.Cos(radin * FixMath.Rad2Deg);
            FixFloat y = radius * FixMath.Sin(radin * FixMath.Rad2Deg);
            return new FVec2(x, y);
        }*/

        public void ListRandom<T>(List<T> sources)
        {
            int index = 0;
            T temp;
            for (int i = 0; i < sources.Count; i++)
            {
                index = this.Range(0, sources.Count);
                if (index != i)
                {
                    temp = sources[i];
                    sources[i] = sources[index];
                    sources[index] = temp;
                }
            }
        }
    }
#if false
    public class LRandom {
        private static Random _i = new Random(3274);
        public static LFloat value => _i.value;
        public static uint Next(){return _i.Next();}
        public static uint Next(uint max){return _i.Next(max);}
        public static int Next(int max){return _i.Next(max);}
        public static uint Range(uint min, uint max){return _i.Range(min, max);}
        public static int Range(int min, int max){return _i.Range(min, max);}
        public static LFloat Range(LFloat min, LFloat max){return _i.Range(min, max);}
    }
#endif
}