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
        private static AnalyseDependencies CreateSut(
            params Tuple<Uri, HttpResponseMessage>[] httpResponseMessages)
        {
            var httpMessageHandlerBuilder = Given.HttpMessageHandler;

            foreach (var httpResponseMessage in httpResponseMessages)
                httpMessageHandlerBuilder.WithHttpResponseMessageForUri(
                    httpResponseMessage.Item2,
                    httpResponseMessage.Item1);

            var httpMessageHandler = httpMessageHandlerBuilder.Build();

            var httpClient = Given.HttpClient
                .WithHttpMessageHandler(httpMessageHandler)
                .Build();

            var dependencyChecker = Given.DependencyChecker
                .WithHttpClient(httpClient)
                .Build();

            var analyserServicesFactory = Given.AnalyserServicesFactory
                .WithDependencyChecker(dependencyChecker)
                .Build();

            return new AnalyseDependencies(analyserServicesFactory);
        }

        private static Tuple<Uri, HttpResponseMessage> CreateHttpResponseMessageTuple(
            Name name,
            SemVersion version)
        {
            var dependency = Given.Dependency
                .WithName(name)
                .WithVersion(version)
                .Build();

            var nugetResponseData = Given.NugetResponseData
                .WithId(dependency.Name)
                .WithVersion(dependency.Version.ToString())
                .Build();

            var nugetResponse = Given.NugetResponse
                .WithData(nugetResponseData)
                .Build();

            var httpResponseMessage = Given.HttpResponseMessage
                .WithOkAsHttpStatusCode()
                .WithContent(nugetResponse)
                .Build();

            var uri = Given.Uri
                .WithUrl($"https://api-v2v3search-0.nuget.org/query?q={dependency.Name}")
                .Build();

            var tuple = new Tuple<Uri, HttpResponseMessage>(
                uri,
                httpResponseMessage);

            return tuple;
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

            var jsonPackage = CreateHttpResponseMessageTuple(
                (Name) "Newtonsoft.Json",
                SemVersion.Parse("11.0.3"));

            var sut = CreateSut(jsonPackage);

            var result = sut.Execute(settings);

            result.ProjectDependencies.Any(x => x.Dependencies.Any(y => y.HasNewerVersion)).Should().BeTrue();
        }
    }
}