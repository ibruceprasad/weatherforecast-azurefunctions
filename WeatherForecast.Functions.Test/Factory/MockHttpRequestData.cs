using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Functions.Test.Factory
{
    public class MockHttpRequestData : HttpRequestData
    {
        private readonly FunctionContext Context;
        public MockHttpRequestData(FunctionContext context, Uri url=null) : base(context)
        {
            this.Context = context;
            Url = url;
        }

        public override HttpResponseData CreateResponse()
        {
            return new MockHttpResponseData(Context);
        }

        public override Stream Body { get; }
        public override HttpHeadersCollection Headers { get; }
        public override IReadOnlyCollection<IHttpCookie> Cookies { get; }
        public override Uri Url { get; }
        public override IEnumerable<ClaimsIdentity> Identities { get; }
        public override string Method { get; }
    }
}
