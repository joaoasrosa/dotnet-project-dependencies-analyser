namespace Cake.DependenciesAnalyser
{
    public class DependenciesAnalyserSettings
    {
        /// <summary>
        ///     Gets or sets a value for the folder to scan for C# projects for analysis.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Project for analysis.
        /// </summary>
        public string Project { get; set; }
    }
}