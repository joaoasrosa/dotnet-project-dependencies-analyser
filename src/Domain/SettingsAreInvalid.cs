using System;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    public class SettingsAreInvalid : Exception
    {
        internal SettingsAreInvalid(string message) : base(message)
        {
        }
    }
}