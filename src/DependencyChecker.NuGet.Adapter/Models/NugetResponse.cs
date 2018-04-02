using System.Collections.Generic;
using System.Linq;
using DotnetProjectDependenciesAnalyser.Domain;
using Newtonsoft.Json;
using Semver;

namespace DependencyChecker.NuGet.Adapter.Models
{
    internal class NugetResponse
    {
        [JsonConstructor]
        internal NugetResponse(
            int totalHits,
            IReadOnlyCollection<Data> data)
        {
            TotalHits = totalHits;
            Data = data;
        }

        [JsonProperty] internal int TotalHits { get; }

        [JsonProperty] internal IReadOnlyCollection<Data> Data { get; }

        internal Dependency? ToDomain()
        {
            var data = Data.FirstOrDefault();

            if (data == null)
                return null;

            return new Dependency(
                (Name) data.Id,
                SemVersion.Parse(data.Version));
        }
    }
}