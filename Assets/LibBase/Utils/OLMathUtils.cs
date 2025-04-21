using System;
using System.Collections.Generic;
using LibBase.MathLite;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LibBase.Utils
{
    public static class OLMathUtils
    {
        private static RandR RandR;
        public static long FandTimes;
        private static uint FandSeed;
        public static int LastFandResult;
        
        private const double PI = math.PI;
        private const double PI2 = math.PI * 2.0f;
        private const double PI_HALF = math.PI / 2.0f;
        
        public static void SRand(uint seed)
        {
            FandTimes = 0;
            FandSeed = seed;
            RandR.Seed = seed;
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
            return Fand();

            // var rand = Random.Range(0, 0x7fffffff);
            // return rand;
        }
        
        public static double RandomWithMinMax(double min, double max)
        {
            int m = 1000000;
            double random = (Arc4Random() % m) * 1.0f / m;
            return min + (max - min) * random;
        }

        // -PI-PI
        
        public static double RandomDirectionWithDirection(double direction, double offset)
        {
            double newDirection = RandomWithMinMax(direction - offset, direction + offset);
            double remainder = (newDirection + math.PI) % (2 * math.PI);
            return remainder - math.PI;
        }

        // 0 - 360 度
        
        public static int RandomDirectionWithDirection(int direction, int offset)
        {
            double newDirection = RandomWithMinMax(direction - offset, direction + offset);
            double remainder = newDirection % 360;
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

        
        public static double DirectionWithDegree(int degree)
        {
            if (degree <= -180 || degree > 180)
                throw new ArgumentException("Invalid degree value");
            return degree * math.PI / 180;
        }

        
        public static double DistanceWithX1Y1X2Y2(double x1, double y1, double x2, double y2)
        {
            return math.sqrt(math.pow(x1 - x2, 2) + math.pow(y1 - y2, 2));
        }

        
        public static double DistanceWithPoints(Vector2 point1, Vector2 point2)
        {
            return Mathf.Sqrt(math.pow(point1.x - point2.x, 2) + math.pow(point1.y - point2.y, 2));
        }

        
        public static double SquareDistanceWithX1Y1X2Y2(double x1, double y1, double x2, double y2)
        {
            return math.pow(x1 - x2, 2) + math.pow(y1 - y2, 2);
        }

        
        public static double SquareDistanceWithPoints(Vector2 point1, Vector2 point2)
        {
            return math.pow(point1.x - point2.x, 2) + math.pow(point1.y - point2.y, 2);
        }
        
        public static double MinDirectionBetweenDirection1Direction2(double direction1, double direction2)
        {
            double delta = math.abs(direction1 - direction2);
            if (delta <= math.PI)
                return delta;

            direction1 = direction1 > 0 ? direction1 : direction1 + math.PI * 2;
            direction2 = direction2 > 0 ? direction2 : direction2 + math.PI * 2;
            return math.abs(direction1 - direction2);
        }
        
        public static double Cos(double number)
        {
            return Truncatedouble(math.cos(number));
        }
        
        public static double Sin(double number)
        {
            return Truncatedouble(math.sin(number));
        }
        
        public static double Tan(double number)
        {
            return Truncatedouble(math.tan(number));
        }
        
        public static double Atan2(double y, double x)
        {
            return Truncatedouble(math.atan2(y, x));
        }
        
        public static double Truncatedouble(double number, int digits = 4)
        {
            double multiplier = math.pow(10, digits);
            number = math.floor(number * multiplier) / multiplier;
            return number;
        }
    }
}