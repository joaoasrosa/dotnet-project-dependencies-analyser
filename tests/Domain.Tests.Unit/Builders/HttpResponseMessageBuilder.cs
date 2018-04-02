using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace Domain.Tests.Unit.Builders
{
    internal class HttpResponseMessageBuilder
    {
        private object _content;
        private HttpStatusCode _httpStatusCode;

        internal HttpResponseMessageBuilder WithOkAsHttpStatusCode()
        {
            _httpStatusCode = HttpStatusCode.OK;
            return this;
        }

        internal HttpResponseMessageBuilder WithContent(
            object content)
        {
            _content = content;
            return this;
        }

        internal HttpResponseMessage Build()
        {
            return new HttpResponseMessage(_httpStatusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(_content))
            };
        }
    }
}