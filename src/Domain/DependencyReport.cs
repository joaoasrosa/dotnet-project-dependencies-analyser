using Semver;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    public class DependencyReport
    {
        internal DependencyReport(
            Dependency dependency,
            SemVersion latestVersion)
        {
            Dependency = dependency;
            LatestVersion = latestVersion;
            HasNewerVersion = LatestVersion.CompareTo(Dependency.Version) > 0;
        }

        public Dependency Dependency { get; }
        public SemVersion LatestVersion { get; }
        public bool HasNewerVersion { get; }
    }
}