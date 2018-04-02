using System;
using System.Net.Http;
using Cake.Core;
using Cake.Core.Annotations;
using DependencyChecker.NuGet.Adapter;
using DotnetProjectDependenciesAnalyser.Domain;

namespace Cake.DependenciesChecker
{
    [CakeAliasCategory("DependenciesChecker")]
    public static class DependenciesCheckerAliases
    {
        /// <summary>
        ///     Analyses a the dependencies of a project(s).
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void AnalyseDependencies(
            this ICakeContext context,
            DependenciesCheckerSettings settings)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var analyseDependencies = new AnalyseDependencies(
                new AnalyserServicesFactory(
                    new NugetDependencyChecker(
                        new HttpClient()
                    )
                )
            );

            var report = analyseDependencies.Execute(
                new AnalyseDependenciesSettings(
                    string.IsNullOrWhiteSpace(settings.Project) ? (File?) settings.Project : null,
                    string.IsNullOrWhiteSpace(settings.Folder) ? (Folder?) settings.Folder : null
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