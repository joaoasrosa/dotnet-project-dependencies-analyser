using System;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    public struct Name : IEquatable<Name>
    {
        private readonly string _value;

        private Name(string value)
        {
            _value = value;
        }

        public bool Equals(Name other)
        {
            return _value == other._value;
        }

        public static implicit operator string(Name value)
        {
            return value._value;
        }

        public static explicit operator Name(string value)
        {
            return new Name(value);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            return obj is Name value && Equals(value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(Name left, Name right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Name left, Name right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return _value;
        }
    }
}