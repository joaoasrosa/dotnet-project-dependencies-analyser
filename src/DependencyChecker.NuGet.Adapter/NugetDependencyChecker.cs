using System.Net.Http;
using DependencyChecker.NuGet.Adapter.Models;
using DotnetProjectDependenciesAnalyser.Domain;
using Newtonsoft.Json;

namespace DependencyChecker.NuGet.Adapter
{
    public class NugetDependencyChecker : IDependencyChecker
    {
        private readonly HttpClient _httpClient;

        public NugetDependencyChecker(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
            // TODO: add telemetry (opt-in or opt-out?)
        }

        public Dependency? VerifyLastestVersion(
            Dependency dependency)
        {
            // TODO: get all the feeds in the system, and use them.
            // TODO: add resilience
            var response = _httpClient.GetAsync(
                    $"https://api-v2v3search-0.nuget.org/query?q={dependency.Name}"
                )
                .Result;

            // TODO: and if it fails?
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<NugetResponse>(
                response.Content.ReadAsStringAsync().Result
            );

            return result?.ToDomain();
        }
    }
}