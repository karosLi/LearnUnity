using System;
using LibBase.MathLite;
using LibBase.MathLite;
using UnityEngine;

namespace LibBase.MathLite.FixMath
{
    [Serializable]
    public struct FixFloat : IEquatable<FixFloat>, IComparable<FixFloat>
    {
        private const int OFFSET_COUNT = 16;
        private const long ONE = 1L << OFFSET_COUNT;
        public static readonly FixFloat Infinity = new FixFloat(Mathf.Infinity); //正无穷
        public static readonly FixFloat NegativeInfinity = new FixFloat(Mathf.NegativeInfinity); //负无穷
        private static readonly float PI = Mathf.PI;
        public static readonly FixFloat Pi = new FixFloat(PI);
        public static readonly FixFloat Pi90 = Pi / 2;
        public static readonly FixFloat Pi270 = Pi * 3 / 2;

        public static readonly FixFloat Deg2Rad = PI / new FixFloat(180);
        public static readonly FixFloat Rad2Deg = new FixFloat(180) / PI;

        [SerializeField] private long _value;

        FixFloat(long value)
        {
            _value = value;
        }

        public FixFloat(float value)
        {
            this._value = (long) (value * ONE);
        }

        public FixFloat(int value)
        {
            this._value = value * ONE;
        }

        public FixFloat(double value)
        {
            this._value = (long) (value * ONE);
        }

        public FixFloat(int nom, int den)
        {
            this._value = nom * ONE / den;
        }

        public static FixFloat Zero = new FixFloat(0);

        public static FixFloat One = new FixFloat(1);

        public static FixFloat Half = new FixFloat(0.5f);

        //----------------- - -------------------
        public static FixFloat operator -(FixFloat t1, FixFloat t2)
        {
            FixFloat temp;
            temp._value = t1._value - t2._value;
            return temp;
        }

        public static FixFloat operator -(FixFloat t)
        {
            FixFloat temp;
            temp._value = -t._value;
            return temp;
        }

        public static FixFloat operator -(FixFloat t1, int t2)
        {
            FixFloat temp;
            temp._value = t1._value - t2 * ONE;
            return temp;
        }

        public static FixFloat operator -(int t1, FixFloat t2)
        {
            FixFloat temp;
            temp._value = t1 * ONE - t2._value;
            return temp;
        }

        public static FixFloat operator -(FixFloat t1, float t2)
        {
            FixFloat temp;
            temp._value = t1._value - (long) (t2 * ONE);
            return temp;
        }

        public static FixFloat operator -(float t1, FixFloat t2)
        {
            FixFloat temp;
            temp._value = (long) (t1 * ONE) - t2._value;
            return temp;
        }

        public static FixFloat operator -(FixFloat t1, double t2)
        {
            FixFloat temp;
            temp._value = t1._value - (long) (t2 * ONE);
            return temp;
        }

        public static FixFloat operator -(double t1, FixFloat t2)
        {
            FixFloat temp;
            temp._value = (long) (t1 * ONE) - t2._value;
            return temp;
        }

        //-----------------  +  -------------
        public static FixFloat operator +(FixFloat t1, FixFloat t2)
        {
            FixFloat temp;
            temp._value = t1._value + t2._value;
            return temp;
        }

        public static FixFloat operator +(FixFloat t1, int t2)
        {
            FixFloat temp;
            temp._value = t1._value + t2 * ONE;
            return temp;
        }

        public static FixFloat operator +(int t1, FixFloat t2)
        {
            return t2 + t1;
        }

        public static FixFloat operator +(FixFloat t1, float t2)
        {
            FixFloat temp;
            temp._value = t1._value + (long) (t2 * ONE);
            return temp;
        }

        public static FixFloat operator +(float t1, FixFloat t2)
        {
            return t2 + t1;
        }

        //-------------------- * -------------------
        public static FixFloat operator *(FixFloat t1, FixFloat t2)
        {
            FixFloat temp;
            temp._value = (t1._value * t2._value) >> OFFSET_COUNT;
            return temp;
        }

        public static FixFloat operator *(FixFloat t1, int t2)
        {
            FixFloat temp;
            temp._value = t1._value * t2;
            return temp;
        }

        public static FixFloat operator *(int t1, FixFloat t2)
        {
            return t2 * t1;
        }

        public static FixFloat operator *(FixFloat t1, float t2)
        {
            FixFloat temp;
            temp._value = (t1._value * (long) (t2 * ONE)) >> OFFSET_COUNT;
            return temp;
        }

        public static FixFloat operator *(float t1, FixFloat t2)
        {
            return t2 * t1;
        }

        public static FixFloat operator *(FixFloat t1, double t2)
        {
            FixFloat temp;
            temp._value = (t1._value * (long) (t2 * ONE)) >> OFFSET_COUNT;
            return temp;
        }

        public static FixFloat operator *(double t1, FixFloat t2)
        {
            return t2 * t1;
        }

        public static FixFloat operator /(FixFloat t1, FixFloat t2)
        {
            FixFloat temp;
            if (t2 == Zero)
            {
                return t1._value > 0 ? Infinity : NegativeInfinity;
            }

            temp._value = (t1._value * ONE) / t2._value;
            return temp;
        }

        public static FixFloat operator /(FixFloat t1, int t2)
        {
            FixFloat temp;
            if (t2 == 0)
            {
                return t1 > Zero ? Infinity : NegativeInfinity;
            }

            temp._value = t1._value / t2;
            return temp;
        }

        public static FixFloat operator /(int t1, FixFloat t2)
        {
            FixFloat temp;
            if (t2 == Zero)
            {
                return t1 > 0 ? Infinity : NegativeInfinity;
            }

            temp._value = ((long) t1 << OFFSET_COUNT << OFFSET_COUNT) / t2._value;
            return temp;
        }

        public static FixFloat operator /(FixFloat t1, float t2)
        {
            FixFloat temp;
            if (LiteMath.IsZero(t2))
            {
                return t1 > Zero ? Infinity : NegativeInfinity;
            }

            temp._value = (t1._value << OFFSET_COUNT) / (long) (t2 * ONE);
            return temp;
        }

        public static FixFloat operator /(float t1, FixFloat t2)
        {
            FixFloat temp;
            if (t2 == Zero)
            {
                return t1 > 0 ? Infinity : NegativeInfinity;
            }

            temp._value = (long) (t1 * ((long) 1 << OFFSET_COUNT << OFFSET_COUNT)) / t2._value;
            return temp;
        }

        public static FixFloat operator /(FixFloat t1, double t2)
        {
            FixFloat temp;
            if (LiteMath.IsZero(t2))
            {
                return t1 > Zero ? Infinity : NegativeInfinity;
            }

            temp._value = (t1._value << OFFSET_COUNT) / (long) (t2 * ONE);
            return temp;
        }

        public static FixFloat operator /(double t1, FixFloat t2)
        {
            FixFloat temp;
            if (t2 == Zero)
            {
                return t1 > 0 ? Infinity : NegativeInfinity;
            }

            temp._value = (long) (t1 * ((long) 1 << OFFSET_COUNT << OFFSET_COUNT)) / t2._value;
            return temp;
        }

        public static bool operator ==(FixFloat t1, FixFloat t2)
        {
            return t1._value == t2._value;
        }

        public static bool operator !=(FixFloat t1, FixFloat t2)
        {
            return t1._value != t2._value;
        }

        public static bool operator >(FixFloat t1, FixFloat t2)
        {
            return t1._value > t2._value;
        }

        public static bool operator <(FixFloat t1, FixFloat t2)
        {
            return t1._value < t2._value;
        }

        public static bool operator >=(FixFloat t1, FixFloat t2)
        {
            return t1._value >= t2._value;
        }

        public static bool operator <=(FixFloat t1, FixFloat t2)
        {
            return t1._value <= t2._value;
        }

        public bool IsZero()
        {
            return this == Zero;
        }

        public static FixFloat Sqrt(FixFloat value)
        {
            return Math.Sqrt((double) value);
        }

        public static FixFloat Pow2(FixFloat value)
        {
            return value * value;
        }

        public static FixFloat Pow(FixFloat value, FixFloat exponent)
        {
            return Math.Pow(value, exponent);
        }

        public static FixFloat ACos(FixFloat value)
        {
            value = FixMath.ForceRange(-One, One, value);
            return Math.Acos((double) value);
        }

        public static FixFloat ASin(FixFloat value)
        {
            value = FixMath.ForceRange(-One, One, value);
            return Math.Asin((double) value);
        }

        public static FixFloat ATan(FixFloat value)
        {
            return Math.Atan((double) value);
        }

        public static FixFloat Abs(FixFloat value)
        {
            return value < Zero ? -value : value;
        }

        public static FixFloat Min(FixFloat t1, FixFloat t2)
        {
            return t1._value >= t2._value ? t2 : t1;
        }

        public static FixFloat Max(FixFloat t1, FixFloat t2)
        {
            return t1._value >= t2._value ? t1 : t2;
        }

        public static FixFloat Round(FixFloat value)
        {
            return Math.Round((double) value);
        }

        public static FixFloat Floor(FixFloat value)
        {
            return Math.Floor((double) value);
        }

        public bool Equals(FixFloat other)
        {
            return _value == other._value;
        }

        public int CompareTo(FixFloat other)
        {
            return _value.CompareTo(other._value);
        }

        public override bool Equals(object obj)
        {
            if (obj is FixFloat)
            {
                return _value == ((FixFloat) obj)._value;
            }

            return false;
        }


        public long FixValue
        {
            get { return _value; }
        }

        public override string ToString()
        {
            return ((float) this).ToString();
        }

        public string ToString(string fmt)
        {
            return ((float) this).ToString(fmt);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static implicit operator float(FixFloat fixFloat)
        {
            return (float) fixFloat._value / ONE;
        }

        public static explicit operator int(FixFloat fixFloat)
        {
            return Convert.ToInt32(fixFloat._value >> OFFSET_COUNT);
        }

        public static explicit operator long(FixFloat fixFloat)
        {
            return fixFloat._value >> OFFSET_COUNT;
        }

        public static explicit operator double(FixFloat fixFloat)
        {
            return (double) fixFloat._value / ONE;
        }


        public static implicit operator FixFloat(float value)
        {
            return new FixFloat(value);
        }

        public static implicit operator FixFloat(int value)
        {
            return new FixFloat(value);
        }

        public static implicit operator FixFloat(long value)
        {
            return new FixFloat(value);
        }

        public static implicit operator FixFloat(double value)
        {
            return new FixFloat(value);
        }
    }
}