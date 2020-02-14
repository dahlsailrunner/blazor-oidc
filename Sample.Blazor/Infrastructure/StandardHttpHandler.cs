using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Blazor.Infrastructure
{
    public class StandardHttpHandler : DelegatingHandler
    {
        private readonly HttpContext _httpContext;

        public StandardHttpHandler(HttpContext httpContext)
        {
            _httpContext = httpContext;
            InnerHandler = new SocketsHttpHandler();
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token = await _httpContext.GetUserAccessTokenAsync();

            request.Headers.Add("Authorization", $"Bearer {token}");
            var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var ex = new Exception("API Failure");
                ex.Data.Add("API Route", $"GET {request.RequestUri}");
                ex.Data.Add("API Status", (int)response.StatusCode);

                var apiResponse = await response.Content.ReadAsStringAsync();
                ex.Data.Add("API Response", apiResponse);
                throw ex;
            }
            return response;
        }
    }
}
