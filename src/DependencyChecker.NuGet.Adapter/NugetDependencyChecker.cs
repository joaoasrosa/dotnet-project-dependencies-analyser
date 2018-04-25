using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using DotnetProjectDependenciesAnalyser.Domain;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using Semver;

namespace DependencyChecker.Nuget.Adapter
{
    public class NugetDependencyChecker : IDependencyChecker
    {
        public Dependency? VerifyLastestVersion(
            Dependency dependency)
        {
            var providers = new List<Lazy<INuGetResourceProvider>>();
            providers.AddRange(
                Repository.Provider.GetCoreV3()
            );

            var settings = Settings.LoadDefaultSettings(
                Directory.GetCurrentDirectory(),
                null,
                new XPlatMachineWideSetting());

            var packageSourceProvider = new PackageSourceProvider(settings);
            var packageSources = packageSourceProvider.LoadPackageSources();

            var results = new List<IPackageSearchMetadata>();

            foreach (var packageSource in packageSources)
            {
                var sourceRepository = new SourceRepository(
                    packageSource,
                    providers
                );

                var packageMetadataResource = sourceRepository.GetResourceAsync<PackageMetadataResource>(
                    CancellationToken.None
                ).Result;

                var result = packageMetadataResource.GetMetadataAsync(
                    dependency.Name,
                    false,
                    true,
                    new NullSourceCacheContext(),
                    NullLogger.Instance,
                    CancellationToken.None
                ).Result;

                results.AddRange(result);
            }

            var latest = results.OrderByDescending(x => x.Identity.Version).FirstOrDefault();

            if (latest == null)
                return null;

            return new Dependency(
                (Name) latest.Identity.Id,
                SemVersion.Parse(latest.Identity.Version.ToNormalizedString()));
        }
    }
}