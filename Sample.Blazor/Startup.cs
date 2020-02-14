using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sample.Blazor.ApiCallerFeature;
using Sample.Blazor.Data;

namespace Sample.Blazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts => { opts.LogoutPath = "/logout"; })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.ResponseType = "code";
                    options.UsePkce = true;

                    GetOidcSettingsFromConfiguration(options);

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;
                    options.ClaimActions.MapAllExcept("nbf", "exp", "nonce", "iat", "c_hash");

                    options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = "name"
                        };
                });

            services.AddMvcCore(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddAccessTokenManagement();

            services.AddRazorPages();
            services.AddServerSideBlazor(opts => { opts.DetailedErrors = true; });
            services.AddHttpContextAccessor();
            services.AddSingleton<WeatherForecastService>();
            //services.AddSingleton<IEventAggregator, EventAggregator.Blazor.EventAggregator>();

            services.AddTransient<ApiCallerService>();

            //services.AddScoped<AlertService>();
        }

        private void GetOidcSettingsFromConfiguration(OpenIdConnectOptions options)
        {
            options.Authority = Configuration["OidcSettings:Authority"];
            options.ClientId = Configuration["OidcSettings:ClientId"];
            options.ClientSecret = Configuration["OidcSettings:ClientSecret"];
            foreach (var scope in Configuration.GetSection("OidcSettings:Scopes").Get<string[]>())
            {
                options.Scope.Add(scope);
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
