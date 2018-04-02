using System;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    internal class ProjectAnalyserService : IAnalyserService
    {
        private readonly File _project;
        private readonly IDependencyChecker _dependencyChecker;

        public ProjectAnalyserService(
            File project, 
            IDependencyChecker dependencyChecker)
        {
            _project = project;
            _dependencyChecker = dependencyChecker;
        }

        public Report Analyse()
        {
            var report = new Report();

            var dotnetProject = DotnetProject.Create(_project);
            
            report.AddProject(dotnetProject);
            var dependencies = dotnetProject.Dependencies;

            foreach (var dependency in dependencies)
            {
                var queryedDependency = _dependencyChecker.VerifyLastestVersion(dependency);
                    
                if(queryedDependency.HasValue == false)
                    throw new Exception("TODO");
                // TODO: what we should do if the call fails?
                    
                report.AddDependencyToProject(
                    dependency,
                    queryedDependency.Value.Version,
                    dotnetProject);
            }
            
            return report;
        }
    }
}