using System;
using System.Linq;
using System.Net.Http;
using DotnetProjectDependenciesAnalyser.Domain;
using FluentAssertions;
using Semver;
using Xunit;

namespace Domain.Tests.Unit
{
    public class WhenAnalysingProject
    {
        private static AnalyseDependencies CreateSut()
        {
            var dependencyChecker = Given.DependencyChecker
                .Build();

            var analyserServicesFactory = Given.AnalyserServicesFactory
                .WithDependencyChecker(dependencyChecker)
                .Build();

            return new AnalyseDependencies(analyserServicesFactory);
        }

        [Fact]
        public void GivenProjectWithOutdatedDependency_ReportContainsOutdatedDependency()
        {
            var subfolder = $"./{Guid.NewGuid()}";

            var settings = Given.AnalyseDependenciesSettings
                .WithProject((File) $"{subfolder}/Dummy/Dummy.csproj")
                .Build();

            Given.CsProject
                .WithNewFormat()
                .WithName("Dummy.csproj")
                .CreateAt(subfolder);

            var sut = CreateSut();

            var result = sut.Execute(settings);

            result.ProjectDependencies.Any(x => x.Dependencies.Any(y => y.HasNewerVersion)).Should().BeTrue();
        }
    }
}