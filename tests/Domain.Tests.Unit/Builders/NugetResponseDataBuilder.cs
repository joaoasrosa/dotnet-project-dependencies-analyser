using System.Collections.Generic;
using DependencyChecker.NuGet.Adapter.Models;

namespace Domain.Tests.Unit.Builders
{
    internal class NugetResponseDataBuilder
    {
        private string _id;
        private string _version;

        internal NugetResponseDataBuilder WithId(
            string id)
        {
            _id = id;
            return this;
        }

        internal NugetResponseDataBuilder WithVersion(
            string version)
        {
            _version = version;
            return this;
        }

        internal IReadOnlyCollection<Data> Build()
        {
            return new[]
            {
                new Data(_id, _version)
            };
        }
    }
}