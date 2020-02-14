using Microsoft.AspNetCore.Http;
using Sample.Blazor.Infrastructure;
using Serilog;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sample.Blazor.ApiCallerFeature
{
    public class ApiCallerService
    {
        private readonly HttpContext _httpContext;

        public ApiCallerService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<string> GetResultFromApi()
        {
            Log.Information("Calling the Product API");

            using (var http = new HttpClient(new StandardHttpHandler(_httpContext)))
            {
                var url = $"https://demo.identityserver.io/api/test";
                var result = await http.GetAsync(url);
                return await result.Content.ReadAsStringAsync();
            }
        }

        public void ThrowErrorFromService()
        {
            throw new Exception("This exception should be logged!");
        }
    }
}
