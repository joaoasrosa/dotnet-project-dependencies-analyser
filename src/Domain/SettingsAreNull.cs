using System;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    public class SettingsAreNull : Exception
    {
        internal SettingsAreNull(string message) : base(message)
        {
        }
    }
}