namespace DotnetProjectDependenciesAnalyser.Domain
{
    public class AnalyseDependenciesSettings
    {
        public AnalyseDependenciesSettings(
            File? project,
            Folder? folder)
        {
            Project = project;
            Folder = folder;
        }

        internal File? Project { get; }
        internal Folder? Folder { get; }

        internal bool SettingsAreValid()
        {
            return Project.HasValue || Folder.HasValue;
        }
    }
}