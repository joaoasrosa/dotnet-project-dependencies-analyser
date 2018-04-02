using System;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    public struct Project : IEquatable<Project>
    {
        private readonly string _value;

        private Project(string value)
        {
            _value = value;
        }

        public bool Equals(Project other)
        {
            return _value == other._value;
        }

        public static implicit operator string(Project value)
        {
            return value._value;
        }

        public static explicit operator Project(string value)
        {
            return new Project(value);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            return obj is Project value && Equals(value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(Project left, Project right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Project left, Project right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return _value;
        }
    }
}