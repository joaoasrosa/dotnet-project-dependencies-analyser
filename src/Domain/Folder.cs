using System;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    public struct Folder : IEquatable<Folder>
    {
        private readonly string _value;

        private Folder(string value)
        {
            _value = value;
        }

        public bool Equals(Folder other)
        {
            return _value == other._value;
        }

        public static implicit operator string(Folder value)
        {
            return value._value;
        }

        public static explicit operator Folder(string value)
        {
            return new Folder(value);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            return obj is Folder value && Equals(value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(Folder left, Folder right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Folder left, Folder right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return _value;
        }
    }
}