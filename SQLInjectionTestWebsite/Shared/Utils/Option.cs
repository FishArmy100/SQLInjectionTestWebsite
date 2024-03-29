﻿namespace SQLInjectionTestWebsite.Shared.Utils
{
    class NullOptionExeption : Exception { }

    public struct Option<T>
    {
        private readonly T m_Value;
        private readonly bool m_HasValue;

        public Option(T value)
        {
            if (typeof(T).IsValueType)
            {
                m_Value = value;
                m_HasValue = true;
            }
            else
            {
                m_Value = value;
                m_HasValue = value != null;
            }
        }

        public static implicit operator Option<T>(T value)
        {
            return new Option<T>(value);
        }

        public static explicit operator T(Option<T> option)
        {
            return option.Value;
        }

        public T Value
        {
            get
            {
                if (HasValue())
                    return m_Value;
                else
                    throw new NullOptionExeption();
            }
        }

        public bool HasValue() => m_HasValue;

        public void Match(Action<T> okFunc, Action failFunc)
        {
            if (HasValue())
                okFunc(Value);
            else
                failFunc();
        }

        public void Match(Action<T> okFunc)
        {
            if (HasValue())
                okFunc(Value);
        }

        public TReturn Match<TReturn>(Func<T, TReturn> okFunc, Func<TReturn> failFunc)
        {
            if (HasValue())
                return okFunc(Value);
            else
                return failFunc();
        }

        public override bool Equals(object? obj)
        {
            return obj is Option<T> option &&
                   EqualityComparer<T>.Default.Equals(m_Value, option.m_Value) &&
                   m_HasValue == option.m_HasValue;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(m_Value, m_HasValue);
        }

        public static bool operator ==(Option<T> left, Option<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Option<T> left, Option<T> right)
        {
            return !(left == right);
        }
    }
}
