using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    public class DotnetProject
    {
        private DotnetProject(
            File file)
        {
            File = file;
            Dependencies = GetProjectDependencies();

            IReadOnlyCollection<Dependency> GetProjectDependencies()
            {
                var directory = Path.GetDirectoryName(
                    File);

                var packagesConfigFile = Path.Combine(
                    directory,
                    "packages.config");

                var packagesFileExists = System.IO.File.Exists(
                    packagesConfigFile);

                return packagesFileExists
                    ? GetProjectDependenciesFromConfigFile()
                    : GetProjectDependenciesFromProjectFile();

                IReadOnlyCollection<Dependency> GetProjectDependenciesFromConfigFile()
                {
                    var packagesConfigurationFile = PackagesConfigurationFile.Create(
                        (File) packagesConfigFile);

                    return packagesConfigurationFile.GetDependencies();
                }

                IReadOnlyCollection<Dependency> GetProjectDependenciesFromProjectFile()
                {
                    var csProjFile = CsProjFile.Create(
                        File);

                    return csProjFile.GetDependencies();
                }
            }
        }

        internal File File { get; }
        internal IReadOnlyCollection<Dependency> Dependencies { get; }

        internal static IReadOnlyCollection<DotnetProject> FindProjects(
            Folder folder)
        {
            var projects = Directory
                .EnumerateFiles(folder, "*.csproj", SearchOption.AllDirectories)
                .ToArray();

            var dotnetProjects = new List<DotnetProject>(
                projects.Length);

            dotnetProjects.AddRange(
                projects.Select(project =>
                    new DotnetProject(
                        (File) project)
                )
            );

            return dotnetProjects;
        }

        internal static DotnetProject Create(File project)
        {
            return new DotnetProject(project);
        }

        public override string ToString()
        {
            return File.ToString();
        }
    }
}