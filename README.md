# Cake.DependenciesAnalyser

A Cake AddIn to analyse the NuGet dependencies of a C# project.

[![Build status](https://ci.appveyor.com/api/projects/status/00iuy33dm740btny?svg=true)](https://ci.appveyor.com/project/joaoasrosa/dotnet-project-dependencies-analyser)
[![NuGet](https://img.shields.io/nuget/v/Cake.DependenciesAnalyser.svg)](https://www.nuget.org/packages/Cake.DependenciesAnalyser)
[![cakebuild.net](https://img.shields.io/badge/WWW-cakebuild.net-blue.svg)](http://cakebuild.net)

The addin uses the information on the `csproj` or `packages.config` for the NuGet packages used in the project, doing a analysis:
* Using the version of the package, verifies if a newer version is available
* Produces a inline report with the NuGet dependencies versions listed

### Report Sample

```

---------------------------------
Project: ./src/Cake.DependenciesAnalyser/Cake.DependenciesAnalyser.csproj
Cake.Core is on version 0.26.1. The dependency is up-to-date.
Newtonsoft.Json is on version 11.0.2. The dependency is up-to-date.
---------------------------------

---------------------------------
Project: ./src/DependencyChecker.NuGet.Adapter/DependencyChecker.NuGet.Adapter.csproj
Newtonsoft.Json is on version 11.0.2. The dependency is up-to-date.
Semver is on version 2.0.4. The dependency is up-to-date.
---------------------------------

---------------------------------
Project: ./src/Domain/Domain.csproj
Semver is on version 2.0.4. The dependency is up-to-date.
---------------------------------
```

### Known limitations

* At the moment the addin *only* supports the public NuGet feed. We are working to bring another feeds.
* We pretend to add a solution analyses to verify package references vs DLL references

## Usage

### Including the addin

To include the addin, add the following to the beginning of the `cake` script:
```
#addin "Cake.DependenciesAnalyser"
```

### Use the addin

To use the addin, you need to configure the settings and run the `AnalyseDependencies` alias:
```
#addin "Cake.DependenciesAnalyser"
...
Task("Dependencies-Analyse")
    .Description("Runs the Dependencies Analyser on the solution.")
    .Does(() => 
    {
        var settings = new DependenciesAnalyserSettings
        {
            Folder = "./src/"
        };

        AnalyseDependencies(settings);
    });
...
```

### Settings

The `DependenciesAnalyserSettings` have *one* mandatory option from *two* possible options, `Folder` and `Project`.
* `Folder` - The addin will recursive scan the folder for `csproj` and analyse them
* `Project` - The addin will analyse the `csproj`

## Built With

* [.NET Framework 4.6 & .NET Core 2.0](https://www.microsoft.com/net/download) - The Framework(s)
* [NuGet](https://www.nuget.org/) - Dependency Management
* [Cake](http://cakebuild.net/) - Cross Platform Build Automation System
* [AppVeyor](https://www.appveyor.com/) - Continuous Integration & Delivery Service

## Contributing

Please read [CONTRIBUTING.md](https://github.com/joaoasrosa/dotnet-project-dependencies-analyser/blob/master/CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/joaoasrosa/dotnet-project-dependencies-analyser/tags).

## Authors

* **João Rosa** - *Initial work* - [joaoasrosa](https://github.com/joaoasrosa) [![Follow @joaoasrosa](https://img.shields.io/badge/Twitter-Follow%20%40joaoasrosa-blue.svg)](https://twitter.com/intent/follow?screen_name=joaoasrosa) 

See also the list of [contributors](https://github.com/joaoasrosa/dotnet-project-dependencies-analyser/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details