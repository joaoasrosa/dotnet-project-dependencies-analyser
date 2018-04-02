using System;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    public struct File : IEquatable<File>
    {
        private readonly string _value;

        private File(string value)
        {
            _value = value;
        }

        public bool Equals(File other)
        {
            return _value == other._value;
        }

        public static implicit operator string(File value)
        {
            return value._value;
        }

        public static explicit operator File(string value)
        {
            return new File(value);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            return obj is File value && Equals(value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(File left, File right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(File left, File right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return _value;
        }
    }
}