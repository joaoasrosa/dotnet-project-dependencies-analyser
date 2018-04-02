#tool "nuget:?package=GitVersion.CommandLine"
#addin "nuget:?package=Cake.DependenciesAnalyser&version=0.1.3"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");
var verbosity = Argument<string>("verbosity", "Minimal");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var sourceDir = Directory("./src");
var testsDir = Directory("./tests");

var solutions = GetFiles("./**/*.sln");

var unitTestsProjects = GetFiles(testsDir.Path + "/**/*.Tests.Unit.csproj");

GitVersion gitVersion = null;

// USED TO CREATE NUGET PACKAGES
var createPackage = false;

// BUILD OUTPUT DIRECTORIES
var artifactsDir = Directory("./artifacts");
var publishDir = Directory("./publish/");

// VERBOSITY
var dotNetCoreVerbosity = Cake.Common.Tools.DotNetCore.DotNetCoreVerbosity.Detailed;
if (!Enum.TryParse(verbosity, true, out dotNetCoreVerbosity))
{	
    Warning("Verbosity could not be parsed into type 'Cake.Common.Tools.DotNetCore.DotNetCoreVerbosity'. Defaulting to {0}", 
        dotNetCoreVerbosity); 
}

///////////////////////////////////////////////////////////////////////////////
// COMMON FUNCTION DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

void Test(FilePathCollection testProjects)
{
    var settings = new DotNetCoreTestSettings
	{
		Configuration = configuration,
		NoBuild = true,
        NoRestore = true,
        Verbosity = dotNetCoreVerbosity
	};

	foreach(var testProject in testProjects)
    {
		Information("Testing '{0}'...",  testProject.FullPath);
		DotNetCoreTest(testProject.FullPath, settings);
		Information("'{0}' has been tested.", testProject.FullPath);
	}
}

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
    // Executed BEFORE the first task.
	EnsureDirectoryExists(artifactsDir);
    Information("Running tasks...");
});

Teardown(ctx =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Description("Cleans all directories that are used during the build process.")
    .Does(() =>
    {
        foreach(var solution in solutions)
        {
            Information("Cleaning {0}", solution.FullPath);
            CleanDirectories(sourceDir.Path + "/**/bin");
            CleanDirectories(sourceDir.Path + "/**/obj");
            CleanDirectories(testsDir.Path + "/**/bin");
            CleanDirectories(testsDir.Path + "/**/obj");
            Information("{0} was clean.", solution.FullPath);
        }

        CleanDirectory(artifactsDir);
        CleanDirectory(publishDir);
    });


Task("Restore")
	.Description("Restores all the NuGet packages that are used by the specified solution.")
	.Does(() => 
    {
        var settings = new DotNetCoreRestoreSettings
        {
            DisableParallel = false,
            NoCache = true,
            Verbosity = dotNetCoreVerbosity
        };
        
        foreach(var solution in solutions)
        {
            Information("Restoring NuGet packages for '{0}'...", solution);
            DotNetCoreRestore(solution.FullPath, settings);
            Information("NuGet packages restored for '{0}'.", solution);
        }
    });

Task("SemVer")
    .Description("Applies the SemVer to all the different parts of the project.")
    .Does(() =>
    {
        var settings = new GitVersionSettings 
        {
            UpdateAssemblyInfo = true
        };

        gitVersion = GitVersion(settings);

        Information("NuGet v2: " + gitVersion.NuGetVersionV2);
        Information("Full SemVer: " + gitVersion.FullSemVer);
        Information("Major Minor Patch: " + gitVersion.MajorMinorPatch);
        
        var csProjects = GetFiles("**/*.csproj");

        foreach(var project in csProjects)
        {
            Information("Applying SemVer to '{0}'...", project);

            if(XmlPeek(project, "/Project/PropertyGroup/PackageVersion") != null)
                XmlPoke(project, "/Project/PropertyGroup/PackageVersion", gitVersion.NuGetVersionV2);
            XmlPoke(project, "/Project/PropertyGroup/AssemblyVersion", gitVersion.MajorMinorPatch);
            XmlPoke(project, "/Project/PropertyGroup/FileVersion", gitVersion.MajorMinorPatch);
            XmlPoke(project, "/Project/PropertyGroup/Version", gitVersion.FullSemVer);
            
            Information("SemVer applied to '{0}'...", project);
        }

        Information("Applying SemVer to 'nuspec'...");
        XmlPoke(
            sourceDir.Path + "/Cake.DependenciesAnalyser/Cake.DependenciesAnalyser.nuspec", 
            "/package/metadata/version", 
            gitVersion.NuGetVersionV2);
        Information("SemVer applied to 'nuspec'...");
        
    });

Task("Build")
	.Description("Builds all the different parts of the project.")
	.Does(() => 
    {
        var msBuildSettings = new DotNetCoreMSBuildSettings 
        {
            TreatAllWarningsAs = MSBuildTreatAllWarningsAs.Error,
            Verbosity = dotNetCoreVerbosity
        };

        var settings = new DotNetCoreBuildSettings 
        {
            Configuration = configuration,
            MSBuildSettings = msBuildSettings
        };

        foreach(var solution in solutions)
        {
            Information("Building '{0}'...", solution);
            DotNetCoreBuild(solution.FullPath, settings);
            Information("'{0}' has been built.", solution);
        }
    });

Task("Test-Unit")
    .Description("Runs all your unit tests, using dotnet CLI.")
    .Does(() => { Test(unitTestsProjects); });

Task("Dependencies-Analyse")
    .Description("Runs the Dependencies Analyser on the solution.")
    .Does(() => 
    {
        var settings = new DependenciesAnalyserSettings
        {
            Folder = sourceDir.Path.ToString()
        };

        Information("Analysing dependencies on folder '{0}'...", sourceDir.Path);
        AnalyseDependencies(settings);
        Information("'{0}' dependencies for the projects had been analysed.", sourceDir.Path);
    });

Task("AppVeyor-Pack")
    .Description("Prepares to pack the project, using AppVeyor.")
    .Does(() =>
    {
        var tagBuildEnvVar = EnvironmentVariable("APPVEYOR_REPO_TAG");
        bool.TryParse(tagBuildEnvVar, out createPackage);
    });

Task("Local-Pack")
    .Description("Prepares to pack the project, using local environment.")
    .Does(() =>
    {
        createPackage = true;
    });

Task("Publish")
    .Description("Publish the Web Application.")
    .Does(() => 
    {
        if(!createPackage)
        {
            Information("Skipping the Publish step.");
            return;
        }

	    var settings = new DotNetCorePublishSettings
		{
			Configuration = configuration,
            NoRestore = true,
            Framework = "netstandard2.0",
			OutputDirectory = publishDir.Path + "/lib/netstandard2.0"
		};

        DotNetCorePublish(
            sourceDir.Path + "/Cake.DependenciesAnalyser/Cake.DependenciesAnalyser.csproj", 
            settings);

        DeleteFiles(publishDir.Path + "/**/*.pdb");
        MoveFile(
            publishDir.Path + "/lib/netstandard2.0/Cake.DependenciesAnalyser.nuspec",
            publishDir.Path + "/Cake.DependenciesAnalyser.nuspec"
        );
    });

Task("Pack")
	.Description("Packs all the different parts of the project.")
	.Does(() => 
    {
        if(!createPackage)
        {
            Information("Skipping the NuGet pack step.");
            return;
        }

        var settings = new NuGetPackSettings
		{
			OutputDirectory = artifactsDir,
			Version = gitVersion.NuGetVersionV2
		};

        Information("Packing '{0}'...", publishDir);
	    NuGetPack(publishDir.Path + "/Cake.DependenciesAnalyser.nuspec", settings);
        Information("'{0}' has been packed.", publishDir);
    });

///////////////////////////////////////////////////////////////////////////////
// COMBINATIONS - let's make life easier...
///////////////////////////////////////////////////////////////////////////////

Task("Build+Test")
    .Description("First runs Build, then Test targets.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("SemVer")
    .IsDependentOn("Build")
    .IsDependentOn("Test-Unit")
    .Does(() => { Information("Ran Build+Test target"); });

Task("Rebuild")
    .Description("Runs a full Clean+Restore+Build build.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("SemVer")
    .IsDependentOn("Build")
    .Does(() => { Information("Rebuilt everything"); });

Task("Test-All")
    .Description("Runs all your tests.")
    .IsDependentOn("Test-Unit")
    .Does(() => { Information("Tested everything"); });

Task("Analyse")
    .Description("Analyse the solution.")
    .IsDependentOn("Build+Test")
    .IsDependentOn("Dependencies-Analyse")
    .Does(() => { Information("Analyses done"); });    

Task("LocalPack")
    .Description("Runs locally.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("SemVer")
    .IsDependentOn("Build")
    .IsDependentOn("Test-Unit")
    //.IsDependentOn("Dependencies-Analyse")
    .IsDependentOn("Local-Pack")
    .IsDependentOn("Publish")
    .IsDependentOn("Pack")
    .Does(() => { Information("Everything is done! Well done AppVeyor."); });

Task("AppVeyor")
    .Description("Runs on AppVeyor after 'merging master'.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("SemVer")
    .IsDependentOn("Build")
    .IsDependentOn("Test-Unit")
    //.IsDependentOn("Dependencies-Analyse")
    .IsDependentOn("AppVeyor-Pack")
    .IsDependentOn("Publish")
    .IsDependentOn("Pack")
    .Does(() => { Information("Everything is done! Well done AppVeyor."); });

///////////////////////////////////////////////////////////////////////////////
// DEFAULT TARGET
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .Description("This is the default task which will run if no specific target is passed in.")
    .IsDependentOn("Build+Test")
    .Does(() => { Warning("No 'Target' was passed in, so we ran the 'Build+Test' operation."); });

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);