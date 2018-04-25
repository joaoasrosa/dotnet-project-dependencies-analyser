using System;
using System.Linq;
using System.Net.Http;
using DotnetProjectDependenciesAnalyser.Domain;
using FluentAssertions;
using Semver;
using Xunit;

namespace Domain.Tests.Unit
{
    public class WhenAnalysingFolder
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
        public void GivenAnalyseDependenciesSettingsAreInvalid_ThrowsSettingsAreInvalidException()
        {
            var settings = Given.AnalyseDependenciesSettings
                .Build();

            var sut = CreateSut();

            var result = Record.Exception(() => sut.Execute(settings));

            result.Should().BeOfType<SettingsAreInvalid>();
        }

        [Fact]
        public void GivenAnalyseDependenciesSettingsAreNull_ThrowsSettingsAreNullException()
        {
            var sut = CreateSut();

            var result = Record.Exception(() => sut.Execute(null));

            result.Should().BeOfType<SettingsAreNull>();
        }

        [Fact]
        public void GivenFolderContainsProjectWithOutdatedDependency_ReportContainsOutdatedDependency()
        {
            var settings = Given.AnalyseDependenciesSettings
                .WithFolder((Folder) $"./{Guid.NewGuid()}/")
                .Build();

            Given.CsProject
                .WithNewFormat()
                .WithName("Dummy.csproj")
                .CreateAt(settings.Folder.Value);

            var sut = CreateSut();

            var result = sut.Execute(settings);

            result.ProjectDependencies.Any(x => x.Dependencies.Any(y => y.HasNewerVersion)).Should().BeTrue();
        }

        [Fact]
        public void GivenFolderContainsTwoProjects_ReportContainsTwoProjects()
        {
            var settings = Given.AnalyseDependenciesSettings
                .WithFolder((Folder) $"./{Guid.NewGuid()}/")
                .Build();

            Given.CsProject
                .WithNewFormat()
                .WithName("Foo.csproj")
                .CreateAt(settings.Folder.Value);

            Given.CsProject
                .WithNewFormat()
                .WithName("Bar.csproj")
                .CreateAt(settings.Folder.Value);

            var sut = CreateSut();

            var result = sut.Execute(settings);

            result.ProjectDependencies.Should().HaveCount(2);
        }
    }
}