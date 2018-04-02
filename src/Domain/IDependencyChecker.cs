namespace DotnetProjectDependenciesAnalyser.Domain
{
    public interface IDependencyChecker
    {
        Dependency? VerifyLastestVersion(Dependency dependency);
    }
}