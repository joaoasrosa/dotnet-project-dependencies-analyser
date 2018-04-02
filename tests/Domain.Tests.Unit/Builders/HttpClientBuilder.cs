using System.Net.Http;

namespace Domain.Tests.Unit.Builders
{
    internal class HttpClientBuilder
    {
        private HttpMessageHandler _httpMessageHandler;

        internal HttpClientBuilder WithHttpMessageHandler(
            HttpMessageHandler httpMessageHandler)
        {
            _httpMessageHandler = httpMessageHandler;
            return this;
        }

        internal HttpClient Build()
        {
            return new HttpClient(
                _httpMessageHandler,
                true);
        }
    }
}