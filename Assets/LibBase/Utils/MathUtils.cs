using System;
using System.Collections.Generic;
using LibBase.MathLite;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LibBase.Utils
{
    public enum WPPointQuadrant : uint
    {
        Quadrant1 = 1,
        Quadrant2 = 2,
        Quadrant3 = 3,
        Quadrant4 = 4
    }
    
    public struct MathLine
    {
        public Vector2 Start;
        public Vector2 End;

        public MathLine(Vector2 a, Vector2 b)
        {
            Start = a;
            End = b;
        }
    }
    
    
    public static class MathUtils
    {
        public static bool UseRandSeed = true;
        private static RandR RandR;
        public static long FandTimes;
        private static uint FandSeed;
        public static int LastFandResult;
        
        private const float PI = Mathf.PI;
        private const float PI2 = Mathf.PI * 2.0f;
        private const float PI_HALF = Mathf.PI / 2.0f;
        private static int mulFactor;
        private static int addFactor;
        private static List<MathLine> mathLines = new List<MathLine>();
        
        static MathUtils()
        {
            mulFactor =  UnityEngine.Random.Range(5, 15);
            addFactor = UnityEngine.Random.Range(0, 1000);
        }
        
        public static void SRand(uint seed)
        {
            if (UseRandSeed)
            {
                FandTimes = 0;
                FandSeed = seed;
                RandR.seed = seed;
            }
        }

        
        public static int Fand()
        {
            FandTimes++;
            int fand = RandR.Next();
            LastFandResult = fand < 0 ? -fand : fand;
            return LastFandResult;
        }

        
        public static int Arc4Random()
        {
            if (UseRandSeed)
            {
                return Fand();
            }

            var rand = Random.Range(0, 0x7fffffff);
            return rand;
        }
        
        public static WPPointQuadrant GetQuadrant(float x, float y, float oX, float oY, float oDir)
        {
            oDir -= PI_HALF;
            float rad = Mathf.Atan2(y - oY, x - oX);
            rad -= oDir;
            if (rad > PI2)
            {
                rad -= PI2;
            }

            if (rad < 0)
            {
                rad += PI2;
            }

            uint ret = (uint)(rad / PI_HALF) + 1;
            if (ret >= 5)
            {
                ret = 4;
            }

            return (WPPointQuadrant)ret;
        }

        
        public static WPPointQuadrant GetDirQuadrant(float rad)
        {
            if (rad > PI2)
            {
                rad -= PI2;
            }

            if (rad < 0)
            {
                rad += PI2;
            }

            uint ret = (uint)(rad / PI_HALF) + 1;
            if (ret >= 5)
            {
                ret = 4;
            }

            return (WPPointQuadrant)ret;
        }

        
        public static float GET_VALID_DIR(float dir)
        {
            dir = dir > Mathf.PI ? dir - Mathf.PI * 2 : dir;
            dir = dir <= -Mathf.PI ? dir + Mathf.PI * 2 : dir;
            return dir;
        }

        
        public static Rect InsetRect(Rect rect, float dx, float dy)
        {
            return new Rect(rect.x + dx, rect.y + dy, rect.width - 2 * dx, rect.height - 2 * dy);
        }
        
        
        public static Rect RectIntersectsRect(Rect rect1, Rect rect2)
        {
            if (rect1.xMin < rect2.xMax && rect1.xMax > rect2.xMin &&
                rect1.yMin < rect2.yMax && rect1.yMax > rect2.yMin)
            {
                float xMin = Mathf.Max(rect1.xMin, rect2.xMin);
                float xMax = Mathf.Min(rect1.xMax, rect2.xMax);
                float yMin = Mathf.Max(rect1.yMin, rect2.yMin);
                float yMax = Mathf.Min(rect1.yMax, rect2.yMax);

                // Create and return the intersection Rect
                return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
            }

            return Rect.zero;
        }
        
        
        public static float RandomWithMinMax(float min, float max)
        {
            int m = 1000000;
            float random = (Arc4Random() % m) * 1.0f / m;
            return min + (max - min) * random;
        }

        // -PI-PI
        
        public static float RandomDirectionWithDirection(float direction, float offset)
        {
            float newDirection = RandomWithMinMax(direction - offset, direction + offset);
            float remainder = (newDirection + Mathf.PI) % (2 * Mathf.PI);
            return remainder - Mathf.PI;
        }

        // 0 - 360 度
        
        public static int RandomDirectionWithDirection(int direction, int offset)
        {
            float newDirection = RandomWithMinMax(direction - offset, direction + offset);
            float remainder = newDirection % 360;
            if (remainder < 0)
            {
                remainder += 360;
            }
            return (int)remainder;
        }
        
        
        public static int RandomDegree()
        {
            int random = Arc4Random() % 360;
            return random - 179;
        }

        
        public static float DirectionWithDegree(int degree)
        {
            if (degree <= -180 || degree > 180)
                throw new ArgumentException("Invalid degree value");
            return degree * Mathf.PI / 180;
        }

        
        public static float DistanceWithX1Y1X2Y2(float x1, float y1, float x2, float y2)
        {
            return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
        }

        
        public static float DistanceWithPoints(Vector2 point1, Vector2 point2)
        {
            return Mathf.Sqrt(Mathf.Pow(point1.x - point2.x, 2) + Mathf.Pow(point1.y - point2.y, 2));
        }

        
        public static float SquareDistanceWithX1Y1X2Y2(float x1, float y1, float x2, float y2)
        {
            return Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2);
        }

        
        public static float SquareDistanceWithPoints(Vector2 point1, Vector2 point2)
        {
            return Mathf.Pow(point1.x - point2.x, 2) + Mathf.Pow(point1.y - point2.y, 2);
        }

        
        public static float MinDirectionBetweenDirection1Direction2(float direction1, float direction2)
        {
            float delta = Mathf.Abs(direction1 - direction2);
            if (delta <= Mathf.PI)
                return delta;

            direction1 = direction1 > 0 ? direction1 : direction1 + Mathf.PI * 2;
            direction2 = direction2 > 0 ? direction2 : direction2 + Mathf.PI * 2;
            return Mathf.Abs(direction1 - direction2);
        }

        
        public static float DirectionFromDirectionToDirectionLimit(float direction, float toDirection, float limit)
        {
            float delta = MinDirectionBetweenDirection1Direction2(direction, toDirection);
            if (delta <= limit)
                return toDirection;

            float dest1 = GET_VALID_DIR(direction + limit);
            float dest2 = GET_VALID_DIR(direction - limit);
            return MinDirectionBetweenDirection1Direction2(dest1, toDirection) <
                   MinDirectionBetweenDirection1Direction2(dest2, toDirection)
                ? dest1
                : dest2;
        }

        
        public static Vector2 RandomPointInRect(UnityEngine.Rect rect)
        {
            float x = RandomWithMinMax(0, rect.width);
            float y = RandomWithMinMax(0, rect.height);
            return new Vector2(rect.x + x, rect.y + y);
        }

        
        public static Vector2 RandomPointInnerRect(UnityEngine.Rect innerRect, UnityEngine.Rect outerRect)
        {
            float gapWidth = outerRect.width - innerRect.width;
            float x = RandomWithMinMax(0, gapWidth);
            if (x < innerRect.x - outerRect.x)
            {
                x = outerRect.x + x;
            }
            else
            {
                x = outerRect.x + x + innerRect.width;
            }

            float gapHeight = outerRect.height - innerRect.height;
            float y = RandomWithMinMax(0, gapHeight);
            if (y < innerRect.y - outerRect.y)
            {
                y = outerRect.y + y;
            }
            else
            {
                y = outerRect.y + y + innerRect.height;
            }

            return new Vector2(x, y);
        }

        
        public static int EncryptNumber(int number)
        {
            return number * mulFactor + addFactor;
        }

        
        public static int DecryptNumber(int number)
        {
            return (number - addFactor) / mulFactor;
        }

        /// 以矩形来构建一个4个线段
        
        public static List<MathLine> LinesWithRect(UnityEngine.Rect rect, Vector2 padding)
        {
            // Apply padding
            rect.xMin += padding.x;
            rect.xMax -= padding.x;
            rect.yMin += padding.y;
            rect.yMax -= padding.y;

            // 从左下开始顺时针的四个角ABCD
            // B C
            // A D
            Vector2 A = new Vector2(rect.xMin, rect.yMin);
            Vector2 B = new Vector2(rect.xMin, rect.yMax);
            Vector2 C = new Vector2(rect.xMax, rect.yMax);
            Vector2 D = new Vector2(rect.xMax, rect.yMin);

            mathLines.Clear();
            mathLines.Add(new MathLine(A, B));
            mathLines.Add(new MathLine(B, C));
            mathLines.Add(new MathLine(C, D));
            mathLines.Add(new MathLine(D, A));

            return mathLines;
        }

        /// 以中心点和大小来构建一个4个线段
        
        public static List<MathLine> LinesWithCenter(Vector2 center, Vector2 size, Vector2 padding)
        {
            // Calculate rectangle from center and size
            UnityEngine.Rect rect = new UnityEngine.Rect(center.x - size.x / 2, center.y - size.y / 2, size.x, size.y);
            return LinesWithRect(rect, padding);
        }

        
        public static UnityEngine.Rect PipeRectWithRects(UnityEngine.Rect rect, UnityEngine.Rect other,
            float pipeDiameter, float pipeLength)
        {
            // 通道宽度为0时返回空值
            if (pipeDiameter <= 0)
                return UnityEngine.Rect.zero;

            // Calculate mid-points and boundaries
            float centerX = rect.x + rect.width / 2;
            float centerY = rect.y + rect.height / 2;
            float halfPipeDiameter = pipeDiameter / 2.0f;

            // Determine the pipe's placement based on the relative position of 'other'
            if (other.xMin > rect.xMax) // 说明other在rect右边
            {
                return new UnityEngine.Rect(rect.xMax, centerY - halfPipeDiameter, pipeLength, pipeDiameter);
            }
            else if (other.xMax < rect.xMin) // 说明other在rect左边
            {
                return new UnityEngine.Rect(rect.xMin - pipeLength, centerY - halfPipeDiameter, pipeLength,
                    pipeDiameter);
            }
            else if (other.yMax < rect.yMin) // 说明other在rect上边
            {
                return new UnityEngine.Rect(centerX - halfPipeDiameter, rect.yMin - pipeLength, pipeDiameter,
                    pipeLength);
            }
            else if (other.yMin > rect.yMax) // 说明other在rect下边
            {
                return new UnityEngine.Rect(centerX - halfPipeDiameter, rect.yMax, pipeDiameter, pipeLength);
            }

            return UnityEngine.Rect.zero;
        }

        // 参考链接： http://www-cs.ccny.cuny.edu/~wolberg/capstone/intersection/Intersection%20point%20of%20two%20lines.html
        
        public static bool HasLineCrossing(
            Vector2 point1,
            Vector2 point2,
            Vector2 point3,
            Vector2 point4,
            out Vector2 interPoint)
        {
            float denominator = (point4.y - point3.y) * (point2.x - point1.x) -
                                (point4.x - point3.x) * (point2.y - point1.y);

            if (denominator == 0)
            {
                // Lines are parallel or coincident
                interPoint = Vector2.zero;
                return false;
            }

            float u1 = ((point4.x - point3.x) * (point1.y - point3.y) - (point4.y - point3.y) * (point1.x - point3.x)) /
                       denominator;
            float u2 = ((point2.x - point1.x) * (point1.y - point3.y) - (point2.y - point1.y) * (point1.x - point3.x)) /
                       denominator;

            if (u1 >= 0 && u1 <= 1 && u2 >= 0 && u2 <= 1)
            {
                interPoint = new Vector2(
                    point1.x + u1 * (point2.x - point1.x),
                    point1.y + u1 * (point2.y - point1.y)
                );
                return true;
            }

            interPoint = Vector2.zero;
            return false;
        }


        /**
         点到直线的距离

         参数计算：
         A=y2-y1；
         B=x1-x2；
         C=x2y1-x1y2;
         1.点到直线的距离公式：
         d= ( Ax0 + By0 + C ) / sqrt ( AA + BB );
         2.垂足C（x，y）计算公式：
         x = ( BBx0 - ABy0 - AC ) / ( AA + BB );
         y = ( -ABx0 + AAy0 – BC ) / ( AA + BB );
         */
        
        public static float DistanceToLine(
            Vector2 point,
            Vector2 p1,
            Vector2 p2,
            out Vector2 interPoint)
        {
            float a = p2.y - p1.y;
            float b = p1.x - p2.x;
            float c = p2.x * p1.y - p1.x * p2.y;

            float denominator = a * a + b * b;

            if (denominator == 0)
            {
                // p1 and p2 are the same point
                interPoint = new Vector2(p1.x, p1.y);
                return Mathf.Sqrt((point.x - p1.x) * (point.x - p1.x) + (point.y - p1.y) * (point.y - p1.y));
            }

            float x = (b * b * point.x - a * b * point.y - a * c) / denominator;
            float y = (-a * b * point.x + a * a * point.y - b * c) / denominator;

            interPoint = new Vector2(x, y);

            return Mathf.Sqrt((point.x - x) * (point.x - x) + (point.y - y) * (point.y - y));
        }


        /// 线性插值：获取某一帧的中间状态的值
        /// 举个例子，这个两个数组表示，scale 会从 0.0 动画(动画时间0.16s)到 1.05，然后从 1.05 动画(动画时间0.08s)到 0.98，然后从 0.98 动画(动画时间0.08s)到 1.0
        /// GLfloat scaleArr[] = {0.0, 1.05, 0.98, 1.0};
        /// GLfloat durationArr[] = {0.16, 0.08, 0.08};
        ///
        /// param renderIndex 当前帧
        /// param animationValues 动画值的变化数组
        /// param animationDurations 每个变化的动画持续时间
        /// param durationLength 动画持续时间的长度
        /// param defaultValue 超出动画时间的范围后需要使用的默认值
        /// param reverseAnimation 是否需要倒放动画
        /// param animationRepeat 动画是否需要重复播放
        /// param animationCompleted 动画是否播放完成
        
        public static float LerpValueWithRenderIndex(int renderIndex,
            float[] animationValues,
            float[] animationDurations,
            int durationLength,
            float defaultValue,
            bool reverseAnimation,
            bool animationRepeat,
            out bool animationCompleted
        )
        {
            // 动画是否完全播完
            bool isAnimationCompleted = false;
            float value = defaultValue;
            float totalDuration = 0;

            // 计算总动画时间
            for (int i = 0; i < durationLength; i++)
            {
                totalDuration += animationDurations[i];
            }

            // 每帧时间
            float msPerFrame = 1.0f / 60; // 1 帧需要 16 毫秒
            // 计算总动画帧数
            int totalDurationTurnCount = (int)Mathf.Floor(totalDuration * 60);
            // 当前动画帧
            int curRenderIndex = animationRepeat ? renderIndex % totalDurationTurnCount : renderIndex;

            if (reverseAnimation)
            {
                // 动画倒放
                float curTime = (totalDurationTurnCount - curRenderIndex) * msPerFrame;

                float animationEndTime = totalDuration; // 动画结束时间
                for (int i = durationLength - 1; i >= 0; i--)
                {
                    // 计算每个阶段的动画开始时间和结束时间
                    float animationDuration = animationDurations[i];
                    float animationStartTime = animationEndTime - animationDuration;

                    // 如果当前帧的时间处于阶段时间范围内，就需要做线性插值
                    if (curTime >= animationStartTime && curTime <= animationEndTime)
                    {
                        float time = (animationEndTime - curTime) / animationDuration;
                        float from = animationValues[i + 1];
                        float to = animationValues[i];

                        // 线性插值
                        value = (1 - time) * from + time * to;
                        break;
                    }

                    animationEndTime -= animationDuration;
                }

                if (curTime <= msPerFrame)
                {
                    isAnimationCompleted = true;
                }
            }
            else
            {
                // 动画顺放
                float curTime = curRenderIndex * msPerFrame;

                float animationStartTime = 0; // 动画开始时间
                for (int i = 0; i < durationLength; i++)
                {
                    // 计算每个阶段的动画开始时间和结束时间
                    float animationDuration = animationDurations[i];
                    float animationEndTime = animationStartTime + animationDuration;

                    // 如果当前帧的时间处于阶段时间范围内，就需要做线性插值
                    if (curTime >= animationStartTime && curTime <= animationEndTime)
                    {
                        float time = (curTime - animationStartTime) / animationDuration;
                        float from = animationValues[i];
                        float to = animationValues[i + 1];

                        // 线性插值
                        value = (1 - time) * from + time * to;
                        break;
                    }

                    animationStartTime += animationDuration;
                }

                if ((curTime + msPerFrame) >= totalDuration)
                {
                    isAnimationCompleted = true;
                }
            }

            animationCompleted = isAnimationCompleted;

            return value;
        }

        // 点是否在 rect 里面
        
        public static bool PointInRect(Vector2 point, UnityEngine.Rect rect)
        {
            return point.x >= rect.xMin && point.x <= rect.xMax &&
                   point.y >= rect.yMin && point.y <= rect.yMax;
        }

        // 归一化向量
        
        public static Vector2 NormalizeVector(Vector2 vector)
        {
            float len = (float)Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
            return new Vector2(vector.x / len, vector.y / len);
        }

        // 安全点
        
        public static Vector2 SafePoint(Vector2 point, UnityEngine.Rect rect, Vector2 padding)
        {
            UnityEngine.Rect paddingRect = InsetRect(rect, padding.x, padding.y);

            float x = Mathf.Max(point.x, paddingRect.xMin);
            x = Mathf.Min(x, paddingRect.xMax);

            float y = Mathf.Max(point.y, paddingRect.yMin);
            y = Mathf.Min(y, paddingRect.yMax);

            return new Vector2(x, y);
        }

        // 回字形内随机一个点
        
        public static Vector2 RandomPointInRect(UnityEngine.Rect rect, Vector2 rectPadding,
            UnityEngine.Rect excludeRect, Vector2 excludeRectPadding)
        {
            UnityEngine.Rect paddingRect = InsetRect(rect, rectPadding.x, rectPadding.y);
            UnityEngine.Rect excludePaddingRect = InsetRect(excludeRect, excludeRectPadding.x, excludeRectPadding.y);

            float left = paddingRect.xMin;
            float right = paddingRect.xMax;
            float top = paddingRect.yMax;
            float bottom = paddingRect.yMin;

            float excludeLeft = excludePaddingRect.xMin;
            float excludeRight = excludePaddingRect.xMax;
            float excludeTop = excludePaddingRect.yMax;
            float excludeBottom = excludePaddingRect.yMin;

            float x = 0;
            float y = 0;

            // 把回字形划分成4个区域
            int random = Random.Range(0, 4);
            if (random == 0)
            {
                // 左侧
                x = RandomWithMinMax(left, excludeLeft);
                y = RandomWithMinMax(bottom, top);
            }
            else if (random == 1)
            {
                // 右侧
                x = RandomWithMinMax(excludeRight, right);
                y = RandomWithMinMax(bottom, top);
            }
            else if (random == 2)
            {
                // 上侧
                x = RandomWithMinMax(left, right);
                y = RandomWithMinMax(excludeTop, top);
            }
            else
            {
                // 下侧
                x = RandomWithMinMax(left, right);
                y = RandomWithMinMax(bottom, excludeBottom);
            }

            return new Vector2(x, y);
        }

        // 简单的线性插值
        
        public static float Lerp(float startValue, float endValue, float currentTime, float duration)
        {
            return startValue + (endValue - startValue) * (currentTime / duration);
        }
        
        public static float Cos(float number)
        {
            return TruncateFloat(Mathf.Cos(number));
        }
        
        public static float Sin(float number)
        {
            return TruncateFloat(Mathf.Sin(number));
        }
        
        public static float Tan(float number)
        {
            return TruncateFloat(Mathf.Tan(number));
        }
        
        public static float Atan2(float y, float x)
        {
            return TruncateFloat(Mathf.Atan2(y, x));
        }
        
        public static float TruncateFloat(float number, int digits = 4)
        {
            float multiplier = Mathf.Pow(10, digits);
            number = Mathf.Floor(number * multiplier) / multiplier;
            return number;
        }
    }
}