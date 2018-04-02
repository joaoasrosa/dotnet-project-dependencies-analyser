using Newtonsoft.Json;

namespace DependencyChecker.NuGet.Adapter.Models
{
    internal class Data
    {
        [JsonConstructor]
        internal Data(
            string id,
            string version)
        {
            Id = id;
            Version = version;
        }

        [JsonProperty] internal string Id { get; }

        [JsonProperty] internal string Version { get; }
    }
}