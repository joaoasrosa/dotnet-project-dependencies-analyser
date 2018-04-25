using System;
using System.Net.Http;
using Cake.Core;
using Cake.Core.Annotations;
using DependencyChecker.Nuget.Adapter;
using DotnetProjectDependenciesAnalyser.Domain;

namespace Cake.DependenciesAnalyser
{
    [CakeAliasCategory("DependenciesAnalyser")]
    public static class DependenciesAnalyserAliases
    {
        /// <summary>
        ///     Analyses a the dependencies of a project(s).
        /// </summary>
        /// <example>
        ///     <code>
        /// var settings = new DependenciesAnalyserSettings
        /// {
        ///     Folder = "./src/"
        /// };
        /// AnalyseDependencies(settings);
        ///  </code>
        /// </example>
        /// <param name="context">The Cake context.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Dependencies Analyser")]
        public static void AnalyseDependencies(
            this ICakeContext context,
            DependenciesAnalyserSettings settings)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var analyseDependencies = new AnalyseDependencies(
                new AnalyserServicesFactory(
                    new NugetDependencyChecker()
                )
            );

            var report = analyseDependencies.Execute(
                new AnalyseDependenciesSettings(
                    string.IsNullOrWhiteSpace(settings.Project) ? null : (File?) settings.Project,
                    string.IsNullOrWhiteSpace(settings.Folder) ? null : (Folder?) settings.Folder
                )
            );

            foreach (var reportProjectDependency in report.ProjectDependencies)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine(
                    "Project: {0}",
                    reportProjectDependency.Project);

                foreach (var dependencyReport in reportProjectDependency.Dependencies)
                    Console.WriteLine(
                        "{0} is on version {1}. The dependency is {2}.",
                        dependencyReport.Dependency,
                        dependencyReport.LatestVersion,
                        dependencyReport.HasNewerVersion ? "outdated" : "up-to-date"
                    );

                Console.WriteLine("---------------------------------");
                Console.WriteLine();
            }
        }
    }
}