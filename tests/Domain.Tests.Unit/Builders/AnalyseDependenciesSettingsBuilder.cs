using DotnetProjectDependenciesAnalyser.Domain;

namespace Domain.Tests.Unit.Builders
{
    internal class AnalyseDependenciesSettingsBuilder
    {
        private Folder? _folder;
        private File? _project;

        internal AnalyseDependenciesSettingsBuilder WithProject(
            File project)
        {
            _project = project;
            return this;
        }

        internal AnalyseDependenciesSettingsBuilder WithFolder(
            Folder folder)
        {
            _folder = folder;
            return this;
        }

        internal AnalyseDependenciesSettings Build()
        {
            return new AnalyseDependenciesSettings(
                _project,
                _folder);
        }
    }
}