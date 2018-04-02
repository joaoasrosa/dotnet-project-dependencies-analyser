using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Tests.Unit.Builders
{
    internal class HttpMessageHandlerBuilder
    {
        private readonly HttpMessageHandlerStub _httpMessageHandlerStub;

        internal HttpMessageHandlerBuilder()
        {
            _httpMessageHandlerStub = new HttpMessageHandlerStub();
        }

        internal HttpMessageHandlerBuilder WithHttpResponseMessageForUri(
            HttpResponseMessage httpResponseMessage,
            Uri uri)
        {
            _httpMessageHandlerStub.AddResponse(
                uri,
                httpResponseMessage);
            return this;
        }

        internal HttpMessageHandler Build()
        {
            return _httpMessageHandlerStub;
        }
    }

    internal class HttpMessageHandlerStub : HttpMessageHandler
    {
        private readonly IDictionary<Uri, HttpResponseMessage> _responses;

        internal HttpMessageHandlerStub()
        {
            _responses = new Dictionary<Uri, HttpResponseMessage>();
        }

        internal void AddResponse(
            Uri uri,
            HttpResponseMessage httpResponseMessage)
        {
            _responses.Add(uri, httpResponseMessage);
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            _responses.TryGetValue(
                request.RequestUri,
                out var response);

            return Task.FromResult(response);
        }
    }
}