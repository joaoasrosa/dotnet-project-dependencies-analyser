using System.Collections.Generic;
using Semver;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    public class ProjectDependencies
    {
        private readonly List<DependencyReport> _dependencies;

        internal ProjectDependencies(
            DotnetProject project)
        {
            Project = project;
            _dependencies = new List<DependencyReport>();
        }

        public DotnetProject Project { get; }

        public IReadOnlyCollection<DependencyReport> Dependencies => _dependencies;

        internal void AddDependency(
            Dependency dependency,
            SemVersion latestVersion)
        {
            _dependencies.Add(
                new DependencyReport(
                    dependency,
                    latestVersion
                )
            );
        }
    }
}