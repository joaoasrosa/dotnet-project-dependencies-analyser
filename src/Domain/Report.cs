using System;
using System.Collections.Generic;
using System.Linq;
using Semver;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    public class Report
    {
        private readonly List<ProjectDependencies> _projectsDependencies;

        internal Report()
        {
            _projectsDependencies = new List<ProjectDependencies>();
        }

        public IReadOnlyCollection<ProjectDependencies> ProjectDependencies => _projectsDependencies;

        internal void AddProject(
            DotnetProject project)
        {
            if (_projectsDependencies.Any(x => x.Project == project))
                throw new Exception("TODO");

            _projectsDependencies.Add(
                new ProjectDependencies(
                    project));
        }

        internal void AddDependencyToProject(
            Dependency dependency,
            SemVersion version,
            DotnetProject project)
        {
            var projectDependencies = _projectsDependencies.SingleOrDefault(
                x => x.Project == project);

            if (projectDependencies == null)
                throw new Exception("TODO");

            projectDependencies.AddDependency(
                dependency,
                version
            );
        }
    }
}