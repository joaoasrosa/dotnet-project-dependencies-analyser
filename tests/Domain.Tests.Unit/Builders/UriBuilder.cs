using System;

namespace Domain.Tests.Unit.Builders
{
    internal class UriBuilder
    {
        private string _url;

        public UriBuilder WithUrl(string url)
        {
            _url = url;
            return this;
        }

        public Uri Build()
        {
            return new Uri(_url);
        }
    }
}