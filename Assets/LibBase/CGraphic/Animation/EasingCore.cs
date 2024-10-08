using LibBase.MathLite.FixMath;
using UnityEngine;

namespace CGraphic.Animation {
    public enum Ease {
        Linear,
        InBack,
        InBounce,
        InCirc,
        InCubic,
        InElastic,
        InExpo,
        InQuad,
        InQuart,
        InQuint,
        InSine,
        OutBack,
        OutBounce,
        OutCirc,
        OutCubic,
        OutElastic,
        OutExpo,
        OutQuad,
        OutQuart,
        OutQuint,
        OutSine,
        InOutBack,
        InOutBounce,
        InOutCirc,
        InOutCubic,
        InOutElastic,
        InOutExpo,
        InOutQuad,
        InOutQuart,
        InOutQuint,
        InOutSine,
    }


    public static class Easing {
        /// <summary>
        /// Gets the easing function
        /// </summary>
        /// <param name="type">Ease type</param>
        /// <returns>Easing function</returns>
        public static float Get(Ease type, float t) {
            switch (type) {
                case Ease.Linear:
                    return linear(t);
                case Ease.InBack:
                    return inBack(t);
                case Ease.InBounce:
                    return inBounce(t);
                case Ease.InCirc:
                    return inCirc(t);
                case Ease.InCubic:
                    return inCubic(t);
                case Ease.InElastic:
                    return inElastic(t);
                case Ease.InExpo:
                    return inExpo(t);
                case Ease.InQuad:
                    return inQuad(t);
                case Ease.InQuart:
                    return inQuart(t);
                case Ease.InQuint:
                    return inQuint(t);
                case Ease.InSine:
                    return inSine(t);
                case Ease.OutBack:
                    return outBack(t);
                case Ease.OutBounce:
                    return outBounce(t);
                case Ease.OutCirc:
                    return outCirc(t);
                case Ease.OutCubic:
                    return outCubic(t);
                case Ease.OutElastic:
                    return outElastic(t);
                case Ease.OutExpo:
                    return outExpo(t);
                case Ease.OutQuad:
                    return outQuad(t);
                case Ease.OutQuart:
                    return outQuart(t);
                case Ease.OutQuint:
                    return outQuint(t);
                case Ease.OutSine:
                    return outSine(t);
                case Ease.InOutBack:
                    return inOutBack(t);
                case Ease.InOutBounce:
                    return inOutBounce(t);
                case Ease.InOutCirc:
                    return inOutCirc(t);
                case Ease.InOutCubic:
                    return inOutCubic(t);
                case Ease.InOutElastic:
                    return inOutElastic(t);
                case Ease.InOutExpo:
                    return inOutExpo(t);
                case Ease.InOutQuad:
                    return inOutQuad(t);
                case Ease.InOutQuart:
                    return inOutQuart(t);
                case Ease.InOutQuint:
                    return inOutQuint(t);
                case Ease.InOutSine:
                    return inOutSine(t);
                default:
                    return linear(t);
            }
        }

        private static float linear(float t) {
            return t;
        }

        private static float inBack(float t) {
            return t * t * t - t * Mathf.Sin(t * Mathf.PI);
        }

        private static float outBack(float t) {
            return 1f - inBack(1f - t);
        }

        private static float inOutBack(float t) {
            return t < 0.5f ? 0.5f * inBack(2f * t) : 0.5f * outBack(2f * t - 1f) + 0.5f;
        }

        private static float inBounce(float t) {
            return 1f - outBounce(1f - t);
        }

        private static float outBounce(float t) {
            return t < 4f / 11.0f ? (121f * t * t) / 16.0f :
                t < 8f / 11.0f ? (363f / 40.0f * t * t) - (99f / 10.0f * t) + 17f / 5.0f :
                t < 9f / 10.0f ? (4356f / 361.0f * t * t) - (35442f / 1805.0f * t) + 16061f / 1805.0f :
                (54f / 5.0f * t * t) - (513f / 25.0f * t) + 268f / 25.0f;
        }


        private static float inOutBounce(float t) {
            return t < 0.5f ? 0.5f * inBounce(2f * t) : 0.5f * outBounce(2f * t - 1f) + 0.5f;
        }

        private static float inCirc(float t) {
            return 1f - Mathf.Sqrt(1f - (t * t));
        }

        private static float outCirc(float t) {
            return Mathf.Sqrt((2f - t) * t);
        }

        private static float inOutCirc(float t) {
            return t < 0.5f ?
                0.5f * (1 - Mathf.Sqrt(1f - 4f * (t * t))) :
                0.5f * (Mathf.Sqrt(-((2f * t) - 3f) * ((2f * t) - 1f)) + 1f);
        }


        private static float inCubic(float t) {
            return t * t * t;
        }

        private static float outCubic(float t) {
            return inCubic(t - 1f) + 1f;
        }

        private static float inOutCubic(float t) {
            return t < 0.5f ? 4f * t * t * t : 0.5f * inCubic(2f * t - 2f) + 1f;
        }

        private static float inElastic(float t) {
            return Mathf.Sin(13f * (Mathf.PI * 0.5f) * t) * Mathf.Pow(2f, 10f * (t - 1f));
        }

        private static float outElastic(float t) {
            return Mathf.Sin(-13f * (Mathf.PI * 0.5f) * (t + 1)) * Mathf.Pow(2f, -10f * t) + 1f;
        }

        private static float inOutElastic(float t) {
            return t < 0.5f ?
                0.5f * Mathf.Sin(13f * (Mathf.PI * 0.5f) * (2f * t)) * Mathf.Pow(2f, 10f * ((2f * t) - 1f)) :
                0.5f * (Mathf.Sin(-13f * (Mathf.PI * 0.5f) * ((2f * t - 1f) + 1f)) *
                    Mathf.Pow(2f, -10f * (2f * t - 1f)) + 2f);
        }


        private static float inExpo(float t) {
            return Mathf.Approximately(0.0f, t) ? t : Mathf.Pow(2f, 10f * (t - 1f));
        }

        private static float outExpo(float t) {
            return Mathf.Approximately(1.0f, t) ? t : 1f - Mathf.Pow(2f, -10f * t);
        }

        private static float inOutExpo(float v) {
            return Mathf.Approximately(0.0f, v) || Mathf.Approximately(1.0f, v) ? v :
                v < 0.5f ? 0.5f * Mathf.Pow(2f, (20f * v) - 10f) : -0.5f * Mathf.Pow(2f, (-20f * v) + 10f) + 1f;
        }


        private static float inQuad(float t) {
            return t * t;
        }

        private static float outQuad(float t) {
            return -t * (t - 2f);
        }

        private static float inOutQuad(float t) {
            return t < 0.5f ? 2f * t * t : -2f * t * t + 4f * t - 1f;
        }

        private static float inQuart(float t) {
            return t * t * t * t;
        }

        private static float outQuart(float t) {
            var u = t - 1f;
            return u * u * u * (1f - t) + 1f;
        }

        private static float inOutQuart(float t) {
            return t < 0.5f ? 8f * inQuart(t) : -8f * inQuart(t - 1f) + 1f;
        }

        private static float inQuint(float t) {
            return t * t * t * t * t;
        }

        private static float outQuint(float t) {
            return inQuint(t - 1f) + 1f;
        }

        private static float inOutQuint(float t) {
            return t < 0.5f ? 16f * inQuint(t) : 0.5f * inQuint(2f * t - 2f) + 1f;
        }

        private static float inSine(float t) {
            return Mathf.Sin((t - 1f) * (Mathf.PI * 0.5f)) + 1f;
        }

        private static float outSine(float t) {
            return Mathf.Sin(t * (Mathf.PI * 0.5f));
        }

        private static float inOutSine(float t) {
            return 0.5f * (1f - Mathf.Cos(t * Mathf.PI));
        }

        public static FixFloat GetFix(Ease type, FixFloat t) {
            switch (type) {
                case Ease.Linear:
                    return linear(t);
                case Ease.InBack:
                    return inBack(t);
                case Ease.InBounce:
                    return inBounce(t);
                case Ease.InCirc:
                    return inCirc(t);
                case Ease.InCubic:
                    return inCubic(t);
                case Ease.InElastic:
                    return inElastic(t);
                case Ease.InExpo:
                    return inExpo(t);
                case Ease.InQuad:
                    return inQuad(t);
                case Ease.InQuart:
                    return inQuart(t);
                case Ease.InQuint:
                    return inQuint(t);
                case Ease.InSine:
                    return inSine(t);
                case Ease.OutBack:
                    return outBack(t);
                case Ease.OutBounce:
                    return outBounce(t);
                case Ease.OutCirc:
                    return outCirc(t);
                case Ease.OutCubic:
                    return outCubic(t);
                case Ease.OutElastic:
                    return outElastic(t);
                case Ease.OutExpo:
                    return outExpo(t);
                case Ease.OutQuad:
                    return outQuad(t);
                case Ease.OutQuart:
                    return outQuart(t);
                case Ease.OutQuint:
                    return outQuint(t);
                case Ease.OutSine:
                    return outSine(t);
                case Ease.InOutBack:
                    return inOutBack(t);
                case Ease.InOutBounce:
                    return inOutBounce(t);
                case Ease.InOutCirc:
                    return inOutCirc(t);
                case Ease.InOutCubic:
                    return inOutCubic(t);
                case Ease.InOutElastic:
                    return inOutElastic(t);
                case Ease.InOutExpo:
                    return inOutExpo(t);
                case Ease.InOutQuad:
                    return inOutQuad(t);
                case Ease.InOutQuart:
                    return inOutQuart(t);
                case Ease.InOutQuint:
                    return inOutQuint(t);
                case Ease.InOutSine:
                    return inOutSine(t);
                default:
                    return linear(t);
            }
        }

        private static FixFloat linear(FixFloat t) {
            return t;
        }

        private static FixFloat inBack(FixFloat t) {
            return t * t * t - t * FixMath.Sin(t * FixFloat.Pi);
        }

        private static FixFloat outBack(FixFloat t) {
            return FixFloat.One - inBack(FixFloat.One - t);
        }

        private static FixFloat inOutBack(FixFloat t) {
            return t < FixFloat.Half ?
                FixFloat.Half * inBack(2f * t) :
                FixFloat.Half * outBack(2f * t - FixFloat.One) + FixFloat.Half;
        }


        private static FixFloat inBounce(FixFloat t) {
            return FixFloat.One - outBounce(FixFloat.One - t);
        }

        private static FixFloat outBounce(FixFloat t) {
            return t < 4f / 11.0f ? (121f * t * t) / 16.0f :
                t < 8f / 11.0f ? (363f / 40.0f * t * t) - (99f / 10.0f * t) + 17f / 5.0f :
                t < 9f / 10.0f ? (4356f / 361.0f * t * t) - (35442f / 1805.0f * t) + 16061f / 1805.0f :
                (54f / 5.0f * t * t) - (513f / 25.0f * t) + 268f / 25.0f;
        }

        private static FixFloat inOutBounce(FixFloat t) {
            return t < FixFloat.Half ?
                FixFloat.Half * inBounce(2f * t) :
                FixFloat.Half * outBounce(2f * t - 1f) + FixFloat.Half;
        }


        private static FixFloat inCirc(FixFloat t) {
            return FixFloat.One - FixFloat.Sqrt(FixFloat.One - (t * t));
        }

        private static FixFloat outCirc(FixFloat t) {
            return FixFloat.Sqrt((2f - t) * t);
        }

        private static FixFloat inOutCirc(FixFloat t) {
            return t < FixFloat.Half ?
                FixFloat.Half * (1 - FixFloat.Sqrt(1f - 4f * (t * t))) :
                FixFloat.Half * (FixFloat.Sqrt(-((2f * t) - 3f) * ((2f * t) - 1f)) + 1f);
        }

        private static FixFloat inCubic(FixFloat t) {
            return t * t * t;
        }

        private static FixFloat outCubic(FixFloat t) {
            return inCubic(t - 1f) + FixFloat.One;
        }

        private static FixFloat inOutCubic(FixFloat t) {
            return t < FixFloat.Half ? 4f * t * t * t : FixFloat.Half * inCubic(2f * t - 2f) + FixFloat.One;
        }


        private static FixFloat inElastic(FixFloat t) {
            return FixMath.Sin(13f * (FixMath.PI * 0.5f) * t) * FixFloat.Pow(2f, 10f * (t - 1f));
        }


        private static FixFloat outElastic(FixFloat t) {
            return FixMath.Sin(-13f * (FixMath.PI * 0.5f) * (t + 1)) * FixFloat.Pow(2f, -10f * t) + 1f;
        }

        private static FixFloat inOutElastic(FixFloat t) {
            return t < FixFloat.Half ?
                FixFloat.Half * FixMath.Sin(13f * (FixMath.PI * 0.5f) * (2f * t)) *
                FixFloat.Pow(2f, 10f * ((2f * t) - 1f)) :
                FixFloat.Half * (FixMath.Sin(-13f * (FixMath.PI * 0.5f) * ((2f * t - 1f) + 1f)) *
                    FixFloat.Pow(2f, -10f * (2f * t - 1f)) + 2f);
        }

        private static FixFloat inExpo(FixFloat t) {
            return Mathf.Approximately(0.0f, t) ? t : FixFloat.Pow(2f, 10f * (t - 1f));
        }

        private static FixFloat outExpo(FixFloat t) {
            return Mathf.Approximately(1.0f, t) ? t : 1f - FixFloat.Pow(2f, -10f * t);
        }

        private static FixFloat inOutExpo(FixFloat v) {
            return Mathf.Approximately(0.0f, v) || Mathf.Approximately(1.0f, v) ? v :
                v < FixFloat.Half ? FixFloat.Half * FixFloat.Pow(2f, (20f * v) - 10f) :
                -FixFloat.Half * FixFloat.Pow(2f, (-20f * v) + 10f) + FixFloat.One;
        }

        private static FixFloat inQuad(FixFloat t) {
            return t * t;
        }

        private static FixFloat outQuad(FixFloat t) {
            return -t * (t - 2f);
        }

        private static FixFloat inOutQuad(FixFloat t) {
            return t < FixFloat.Half ? 2f * t * t : -2f * t * t + 4f * t - FixFloat.One;
        }

        private static FixFloat inQuart(FixFloat t) {
            return t * t * t * t;
        }

        private static FixFloat outQuart(FixFloat t) {
            var u = t - FixFloat.One;
            return u * u * u * (1f - t) + FixFloat.One;
        }

        private static FixFloat inOutQuart(FixFloat t) {
            return t < FixFloat.Half ? 8f * inQuart(t) : -8f * inQuart(t - 1f) + FixFloat.One;
        }


        private static FixFloat inQuint(FixFloat t) {
            return t * t * t * t * t;
        }

        private static FixFloat outQuint(FixFloat t) {
            return inQuint(t - 1f) + FixFloat.One;
        }

        private static FixFloat inOutQuint(FixFloat t) {
            return t < FixFloat.Half ? 16f * inQuint(t) : FixFloat.Half * inQuint(2f * t - 2f) + FixFloat.One;
        }


        private static FixFloat inSine(FixFloat t) {
            return FixMath.Sin((t - 1f) * (FixMath.PI * 0.5f)) + FixFloat.One;
        }

        private static FixFloat outSine(FixFloat t) {
            return FixMath.Sin(t * (FixMath.PI * 0.5f));
        }

        private static FixFloat inOutSine(FixFloat t) {
            return FixFloat.Half * (FixFloat.One - FixMath.Cos(t * FixMath.PI));
        }
    }
}