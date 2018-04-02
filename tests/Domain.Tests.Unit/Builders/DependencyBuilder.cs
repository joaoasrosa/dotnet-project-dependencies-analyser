using DotnetProjectDependenciesAnalyser.Domain;
using Semver;

namespace Domain.Tests.Unit.Builders
{
    internal class DependencyBuilder
    {
        private Name _name;
        private SemVersion _version;

        internal DependencyBuilder WithName(
            Name name)
        {
            _name = name;
            return this;
        }

        internal DependencyBuilder WithVersion(
            SemVersion version)
        {
            _version = version;
            return this;
        }

        internal Dependency Build()
        {
            return new Dependency(
                _name,
                _version);
        }
    }
}