using System.Collections.Generic;
using DependencyChecker.NuGet.Adapter.Models;

namespace Domain.Tests.Unit.Builders
{
    internal class NugetResponseBuilder
    {
        private IReadOnlyCollection<Data> _data;

        internal NugetResponseBuilder WithData(
            IReadOnlyCollection<Data> data)
        {
            _data = data;
            return this;
        }

        internal NugetResponse Build()
        {
            return new NugetResponse(
                _data.Count,
                _data);
        }
    }
}