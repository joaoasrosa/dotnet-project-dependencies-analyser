using System.IO;

namespace Domain.Tests.Unit.Builders
{
    internal class CsProjectBuilder
    {
        #region C# Projects Definitions

        private const string NewProject = "<Project Sdk=\"Microsoft.NET.Sdk\">" +
                                          "  <PropertyGroup>" +
                                          "    <TargetFramework>netstandard2.0</TargetFramework>" +
                                          "  </PropertyGroup>" +
                                          "  <ItemGroup>" +
                                          "    <PackageReference Include=\"Newtonsoft.Json\" Version=\"11.0.1\" />" +
                                          "  </ItemGroup>" +
                                          "</Project>";

        #endregion

        private string _name;

        private bool _newFormat;

        internal CsProjectBuilder WithNewFormat()
        {
            _newFormat = true;
            return this;
        }

        internal CsProjectBuilder WithName(
            string name)
        {
            _name = name;
            return this;
        }

        internal void CreateAt(string folder)
        {
            var subFolder = Path.GetFileNameWithoutExtension(_name);

            var fullFolderPath = Path.Combine(folder, subFolder);

            if (Directory.Exists(fullFolderPath))
                Directory.Delete(fullFolderPath, true);

            Directory.CreateDirectory(fullFolderPath);

            if (_newFormat)
                File.WriteAllText(
                    Path.Combine(fullFolderPath, _name),
                    NewProject);
        }
    }
}