using System;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    public class AnalyserServicesFactory
    {
        private readonly IDependencyChecker _dependencyChecker;

        public AnalyserServicesFactory(
            IDependencyChecker dependencyChecker)
        {
            _dependencyChecker = dependencyChecker;
        }
        
        internal IAnalyserService CreateService(
            AnalyseDependenciesSettings settings)
        {
            if (settings.Folder.HasValue)
                return new FolderAnalyserService(
                    settings.Folder.Value,
                    _dependencyChecker);
            
            if (settings.Project.HasValue)
                return new ProjectAnalyserService(
                    settings.Project.Value,
                    _dependencyChecker);

            throw new AnalyserServiceIsMissing();
        }
    }
}