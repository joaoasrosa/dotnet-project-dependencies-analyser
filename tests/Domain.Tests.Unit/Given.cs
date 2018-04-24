using Domain.Tests.Unit.Builders;

namespace Domain.Tests.Unit
{
    internal static class Given
    {
        internal static AnalyserServicesFactoryBuilder AnalyserServicesFactory =>
            new AnalyserServicesFactoryBuilder();

        internal static DependencyCheckerBuilder DependencyChecker =>
            new DependencyCheckerBuilder();

        internal static AnalyseDependenciesSettingsBuilder AnalyseDependenciesSettings =>
            new AnalyseDependenciesSettingsBuilder();

        internal static DependencyBuilder Dependency =>
            new DependencyBuilder();

        internal static CsProjectBuilder CsProject =>
            new CsProjectBuilder();
    }
}