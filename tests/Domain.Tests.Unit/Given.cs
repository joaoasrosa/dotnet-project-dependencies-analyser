using Domain.Tests.Unit.Builders;

namespace Domain.Tests.Unit
{
    internal static class Given
    {
        internal static AnalyserServicesFactoryBuilder AnalyserServicesFactory =>
            new AnalyserServicesFactoryBuilder();

        internal static DependencyCheckerBuilder DependencyChecker =>
            new DependencyCheckerBuilder();

        internal static HttpClientBuilder HttpClient =>
            new HttpClientBuilder();

        internal static HttpMessageHandlerBuilder HttpMessageHandler =>
            new HttpMessageHandlerBuilder();

        internal static AnalyseDependenciesSettingsBuilder AnalyseDependenciesSettings =>
            new AnalyseDependenciesSettingsBuilder();

        internal static HttpResponseMessageBuilder HttpResponseMessage =>
            new HttpResponseMessageBuilder();

        internal static DependencyBuilder Dependency =>
            new DependencyBuilder();

        internal static NugetResponseBuilder NugetResponse =>
            new NugetResponseBuilder();

        internal static NugetResponseDataBuilder NugetResponseData =>
            new NugetResponseDataBuilder();

        internal static UriBuilder Uri =>
            new UriBuilder();

        internal static CsProjectBuilder CsProject =>
            new CsProjectBuilder();
    }
}