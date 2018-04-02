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
    [XmlRoot("Project")]
    public class CsProjFile
    {
        [XmlElement(ElementName = "ItemGroup")]
        public List<ItemGroup> ItemGroups { get; set; }

        internal static CsProjFile Create(
            File file)
        {
            using (var streamReader = new StreamReader(file, Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(CsProjFile));
                var deserializedResult = serializer.Deserialize(streamReader) as CsProjFile;

                return deserializedResult;
            }
        }

        internal IReadOnlyCollection<Dependency> GetDependencies()
        {
            var dependencies = new List<Dependency>(ItemGroups.Sum(x => x.PackageReferences.Count));
            foreach (var itemGroup in ItemGroups)
            foreach (var packageReference in itemGroup.PackageReferences)
                dependencies.Add(new Dependency(
                    (Name) packageReference.Include,
                    SemVersion.Parse(packageReference.Version)));

            return dependencies;
        }
    }

    [Serializable]
    public class ItemGroup
    {
        [XmlElement(ElementName = "PackageReference")]
        public List<PackageReference> PackageReferences { get; set; }
    }

    [Serializable]
    public class PackageReference
    {
        [XmlAttribute(AttributeName = "Include")] 
        public string Include { get; set; }

        [XmlAttribute(AttributeName = "Version")] 
        public string Version { get; set; }
    }
}