using System.Net.Http;
using DependencyChecker.NuGet.Adapter;
using DotnetProjectDependenciesAnalyser.Domain;

namespace Domain.Tests.Unit.Builders
{
    internal class DependencyCheckerBuilder
    {
        private HttpClient _httpClient;

        internal DependencyCheckerBuilder WithHttpClient(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
            return this;
        }

        internal IDependencyChecker Build()
        {
            return new NugetDependencyChecker(
                _httpClient);
        }
    }
}