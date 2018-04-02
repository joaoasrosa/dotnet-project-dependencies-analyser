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
            if(context == null)
                throw new ArgumentNullException(nameof(context));

            if(settings == null)
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
                    string.IsNullOrWhiteSpace(settings.Project) ? (File?)settings.Project : null,
                    string.IsNullOrWhiteSpace(settings.Folder) ? (Folder?)settings.Folder : null
                    )
                );
            
            // TODO: how to display the report?
        }
    }
}