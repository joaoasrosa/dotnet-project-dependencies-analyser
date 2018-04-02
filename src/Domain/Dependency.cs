using System;
using Semver;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    public struct Dependency
    {
        public Dependency(
            Name name,
            SemVersion version)
        {
            Name = name;
            Version = version;
        }

        public Name Name { get; }
        internal SemVersion Version { get; }
    }
}