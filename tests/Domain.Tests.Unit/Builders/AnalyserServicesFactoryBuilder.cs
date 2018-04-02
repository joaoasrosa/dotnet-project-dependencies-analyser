using DotnetProjectDependenciesAnalyser.Domain;

namespace Domain.Tests.Unit.Builders
{
    internal class AnalyserServicesFactoryBuilder
    {
        private IDependencyChecker _dependencyChecker;

        internal AnalyserServicesFactoryBuilder WithDependencyChecker(
            IDependencyChecker dependencyChecker)
        {
            _dependencyChecker = dependencyChecker;
            return this;
        }

        internal AnalyserServicesFactory Build()
        {
            return new AnalyserServicesFactory(
                _dependencyChecker);
        }
    }
}