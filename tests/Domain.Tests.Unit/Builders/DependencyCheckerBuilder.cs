using DependencyChecker.Nuget.Adapter;
using DotnetProjectDependenciesAnalyser.Domain;

namespace Domain.Tests.Unit.Builders
{
    internal class DependencyCheckerBuilder
    {
        internal IDependencyChecker Build()
        {
            return new NugetDependencyChecker();
        }
    }
}