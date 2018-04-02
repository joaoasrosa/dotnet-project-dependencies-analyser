using System;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    internal class FolderAnalyserService : IAnalyserService
    {
        private readonly IDependencyChecker _dependencyChecker;
        private readonly Folder _folder;

        public FolderAnalyserService(
            Folder folder,
            IDependencyChecker dependencyChecker)
        {
            _folder = folder;
            _dependencyChecker = dependencyChecker;
        }

        public Report Analyse()
        {
            var report = new Report();

            var dotnetProjects = DotnetProject.FindProjects(_folder);

            foreach (var dotnetProject in dotnetProjects)
            {
                report.AddProject(dotnetProject);
                var dependencies = dotnetProject.Dependencies;

                foreach (var dependency in dependencies)
                {
                    var queryedDependency = _dependencyChecker.VerifyLastestVersion(dependency);

                    if (queryedDependency.HasValue == false)
                        throw new Exception("TODO");
                    // TODO: what we should do if the call fails?

                    report.AddDependencyToProject(
                        dependency,
                        queryedDependency.Value.Version,
                        dotnetProject);
                }
            }

            return report;
        }
    }
}