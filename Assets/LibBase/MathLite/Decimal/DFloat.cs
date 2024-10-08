using System;
// using ProtoBuf;
using UnityEngine;

namespace LibBase.MathLite.Decimal
{
    [Serializable]
    // [ProtoContract]
    public struct DFloat : IEquatable<DFloat>, IComparable<DFloat>
    {
        private const int OFFSET_COUNT = 10;
        private const long ONE = 1L << OFFSET_COUNT;
        public static readonly DFloat Infinity = new DFloat(Mathf.Infinity); //正无穷
        public static readonly DFloat NegativeInfinity = new DFloat(Mathf.NegativeInfinity); //负无穷
        private static readonly float PI = Mathf.PI;
        public static readonly DFloat Pi = new DFloat(PI);
        public static readonly DFloat TwoPi = new DFloat(PI * 2);
        public static readonly DFloat Pi90 = Pi / 2;
        public static readonly DFloat Pi270 = Pi * 3 / 2;
        public static readonly DFloat Millesimal = new DFloat(0.001f);//千分之一
        public static readonly DFloat Deg2Rad = PI / new DFloat(180);
        public static readonly DFloat Rad2Deg = new DFloat(180) / PI;

        [SerializeField] 
        // [ProtoMember(1)]
        private long _value;

        DFloat(long value)
        {
            _value = value * ONE;
        }

        public DFloat(float value)
        {
            this._value = (long) (value * ONE);
        }

        public DFloat(int value)
        {
            this._value = value * ONE;
        }

        public DFloat(double value)
        {
            this._value = (long) (value * ONE);
        }

        public DFloat(int nom, int den)
        {
            this._value = nom * ONE / den;
        }

        public static DFloat Zero = new DFloat(0);

        public static DFloat One = new DFloat(1);

        public static DFloat Half = new DFloat(0.5f);

        //----------------- - -------------------
        public static DFloat operator -(DFloat t1, DFloat t2)
        {
            DFloat temp;
            temp._value = t1._value - t2._value;
            return temp;
        }

        public static DFloat operator -(DFloat t)
        {
            DFloat temp;
            temp._value = -t._value;
            return temp;
        }

        public static DFloat operator -(DFloat t1, int t2)
        {
            DFloat temp;
            temp._value = t1._value - t2 * ONE;
            return temp;
        }

        public static DFloat operator -(int t1, DFloat t2)
        {
            DFloat temp;
            temp._value = t1 * ONE - t2._value;
            return temp;
        }

        public static DFloat operator -(DFloat t1, float t2)
        {
            DFloat temp;
            temp._value = t1._value - (long) (t2 * ONE);
            return temp;
        }

        public static DFloat operator -(float t1, DFloat t2)
        {
            DFloat temp;
            temp._value = (long) (t1 * ONE) - t2._value;
            return temp;
        }

        public static DFloat operator -(DFloat t1, double t2)
        {
            DFloat temp;
            temp._value = t1._value - (long) (t2 * ONE);
            return temp;
        }

        public static DFloat operator -(double t1, DFloat t2)
        {
            DFloat temp;
            temp._value = (long) (t1 * ONE) - t2._value;
            return temp;
        }

        //-----------------  +  -------------
        public static DFloat operator +(DFloat t1, DFloat t2)
        {
            DFloat temp;
            temp._value = t1._value + t2._value;
            return temp;
        }

        public static DFloat operator +(DFloat t1, int t2)
        {
            DFloat temp;
            temp._value = t1._value + t2 * ONE;
            return temp;
        }

        public static DFloat operator +(int t1, DFloat t2)
        {
            return t2 + t1;
        }

        public static DFloat operator +(DFloat t1, float t2)
        {
            DFloat temp;
            temp._value = t1._value + (long) (t2 * ONE);
            return temp;
        }

        public static DFloat operator +(float t1, DFloat t2)
        {
            return t2 + t1;
        }

        //-------------------- * -------------------
        public static DFloat operator *(DFloat t1, DFloat t2)
        {
            DFloat temp;
            temp._value = (t1._value * t2._value) >> OFFSET_COUNT;
            return temp;
        }

        public static DFloat operator *(DFloat t1, int t2)
        {
            DFloat temp;
            temp._value = t1._value * t2;
            return temp;
        }

        public static DFloat operator *(int t1, DFloat t2)
        {
            return t2 * t1;
        }

        public static DFloat operator *(DFloat t1, float t2)
        {
            DFloat temp;
            temp._value = (t1._value * (long) (t2 * ONE)) >> OFFSET_COUNT;
            return temp;
        }

        public static DFloat operator *(float t1, DFloat t2)
        {
            return t2 * t1;
        }

        public static DFloat operator *(DFloat t1, double t2)
        {
            DFloat temp;
            temp._value = (t1._value * (long) (t2 * ONE)) >> OFFSET_COUNT;
            return temp;
        }

        public static DFloat operator *(double t1, DFloat t2)
        {
            return t2 * t1;
        }

        public static DFloat operator /(DFloat t1, DFloat t2)
        {
            DFloat temp;
            if (t2 == Zero)
            {
                return t1._value > 0 ? Infinity : NegativeInfinity;
            }

            temp._value = (t1._value * ONE) / t2._value;
            return temp;
        }

        public static DFloat operator /(DFloat t1, int t2)
        {
            DFloat temp;
            if (t2 == 0)
            {
                return t1 > Zero ? Infinity : NegativeInfinity;
            }

            temp._value = t1._value / t2;
            return temp;
        }

        public static DFloat operator /(int t1, DFloat t2)
        {
            DFloat temp;
            if (t2 == Zero)
            {
                return t1 > 0 ? Infinity : NegativeInfinity;
            }

            temp._value = ((long) t1 << OFFSET_COUNT << OFFSET_COUNT) / t2._value;
            return temp;
        }

        public static DFloat operator /(DFloat t1, float t2)
        {
            DFloat temp;
            if (LiteMath.IsZero(t2))
            {
                return t1 > Zero ? Infinity : NegativeInfinity;
            }

            temp._value = (t1._value << OFFSET_COUNT) / (long) (t2 * ONE);
            return temp;
        }

        public static DFloat operator /(float t1, DFloat t2)
        {
            DFloat temp;
            if (t2 == Zero)
            {
                return t1 > 0 ? Infinity : NegativeInfinity;
            }

            temp._value = (long) (t1 * ((long) 1 << OFFSET_COUNT << OFFSET_COUNT)) / t2._value;
            return temp;
        }

        public static DFloat operator /(DFloat t1, double t2)
        {
            DFloat temp;
            if (LiteMath.IsZero(t2))
            {
                return t1 > Zero ? Infinity : NegativeInfinity;
            }

            temp._value = (t1._value << OFFSET_COUNT) / (long) (t2 * ONE);
            return temp;
        }

        public static DFloat operator /(double t1, DFloat t2)
        {
            DFloat temp;
            if (t2 == Zero)
            {
                return t1 > 0 ? Infinity : NegativeInfinity;
            }

            temp._value = (long) (t1 * ((long) 1 << OFFSET_COUNT << OFFSET_COUNT)) / t2._value;
            return temp;
        }

        public static bool operator ==(DFloat t1, DFloat t2)
        {
            return t1._value == t2._value;
        }

        public static bool operator !=(DFloat t1, DFloat t2)
        {
            return t1._value != t2._value;
        }

        public static bool operator >(DFloat t1, DFloat t2)
        {
            return t1._value > t2._value;
        }

        public static bool operator <(DFloat t1, DFloat t2)
        {
            return t1._value < t2._value;
        }

        public static bool operator >=(DFloat t1, DFloat t2)
        {
            return t1._value >= t2._value;
        }

        public static bool operator <=(DFloat t1, DFloat t2)
        {
            return t1._value <= t2._value;
        }

        public bool IsZero()
        {
            return this == Zero;
        }

        public static DFloat Sqrt(DFloat value)
        {
            return Math.Sqrt((double) value);
        }

        public static DFloat Pow2(DFloat value)
        {
            return value * value;
        }

        public static DFloat Pow(DFloat value, DFloat exponent)
        {
            return Math.Pow(value, exponent);
        }

        public static DFloat ACos(DFloat value)
        {
            value = DMath.Clamp(-One, One, value);
            return Math.Acos((double) value);
        }

        public static DFloat ASin(DFloat value)
        {
            value = DMath.Clamp(-One, One, value);
            return Math.Asin((double) value);
        }

        public static DFloat ATan(DFloat value)
        {
            return Math.Atan((double) value);
        }

        public static DFloat Abs(DFloat value)
        {
            return value < Zero ? -value : value;
        }

        public static DFloat Min(DFloat t1, DFloat t2)
        {
            return t1._value >= t2._value ? t2 : t1;
        }

        public static DFloat Max(DFloat t1, DFloat t2)
        {
            return t1._value >= t2._value ? t1 : t2;
        }

        public static DFloat Round(DFloat value)
        {
            return Math.Round((double) value);
        }

        public static DFloat Floor(DFloat value)
        {
            return Math.Floor((double) value);
        }

        public bool Equals(DFloat other)
        {
            return _value == other._value;
        }

        public int CompareTo(DFloat other)
        {
            return _value.CompareTo(other._value);
        }

        public override bool Equals(object obj)
        {
            if (obj is DFloat)
            {
                return _value == ((DFloat) obj)._value;
            }

            return false;
        }


        public long DValue
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

        public static implicit operator float(DFloat DFloat)
        {
            return (float) DFloat._value / ONE;
        }

        public static explicit operator int(DFloat DFloat)
        {
            return Convert.ToInt32(DFloat._value >> OFFSET_COUNT);
        }

        public static explicit operator long(DFloat DFloat)
        {
            return DFloat._value >> OFFSET_COUNT;
        }

        public static explicit operator double(DFloat DFloat)
        {
            return (double) DFloat._value / ONE;
        }


        public static implicit operator DFloat(float value)
        {
            return new DFloat(value);
        }

        public static implicit operator DFloat(int value)
        {
            return new DFloat(value);
        }

        public static implicit operator DFloat(long value)
        {
            return new DFloat(value);
        }

        public static implicit operator DFloat(double value)
        {
            return new DFloat(value);
        }
    }
}