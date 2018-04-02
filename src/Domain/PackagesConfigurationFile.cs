using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Semver;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    [Serializable]
    [XmlRoot("packages")]
    internal class PackagesConfigurationFile
    {
        internal IReadOnlyCollection<Package> Packages { get; set; }

        internal static PackagesConfigurationFile Create(
            File file)
        {
            using (var streamReader = new StreamReader(file, Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(PackagesConfigurationFile));
                var deserializedResult = serializer.Deserialize(streamReader) as PackagesConfigurationFile;

                return deserializedResult;
            }
        }

        internal IReadOnlyCollection<Dependency> GetDependencies()
        {
            var dependencies = new List<Dependency>(Packages.Count);
            dependencies.AddRange(Packages.Select(package =>
                    new Dependency(
                        (Name) package.Id,
                        SemVersion.Parse(package.Version)
                    )
                )
            );

            return dependencies;
        }
    }

    [Serializable]
    internal class Package
    {
        [XmlElement("id")] 
        internal string Id { get; set; }

        [XmlElement("version")] 
        internal string Version { get; set; }
    }
}