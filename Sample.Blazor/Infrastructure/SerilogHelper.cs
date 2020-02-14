using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Debugging;
using Serilog.Enrichers.AspnetcoreHttpcontext;
using Serilog.Formatting.Compact;

namespace Sample.Blazor.Infrastructure
{
    public static class SerilogHelper
    {
        public static void WithCustomConfiguration(this LoggerConfiguration loggerConfig, 
            IServiceProvider provider, IConfiguration config)
        {
            var selfLogFile = !string.IsNullOrWhiteSpace(config.GetValue<string>("Logging:SelfLogFile")) ? config.GetValue<string>("Logging:SelfLogFile") : config.GetValue<string>("Logging_SelfLogFile");
            if (!string.IsNullOrEmpty(selfLogFile))
            {
                SelfLog.Enable(File.CreateText(selfLogFile));
            }

            var name = Assembly.GetEntryAssembly()?.GetName();

            loggerConfig
                .ReadFrom.Configuration(config) // minimum levels defined per project in json files 
                .Enrich.WithAspnetcoreHttpcontext(provider, AddCustomContextDetails)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Assembly", name?.Name)
                .Enrich.WithProperty("Version", name?.Version);

            var logFilePath = config.GetValue<string>("Logging:FilePath");

            loggerConfig.WriteTo.File(new CompactJsonFormatter(), logFilePath, rollingInterval: RollingInterval.Day);
        }

        private static UserInfo AddCustomContextDetails(IHttpContextAccessor hca)
        {
            var excluded = new List<string> {"nbf", "exp", "auth_time", "amr", "sub", "aud", "jti", "at_hash", "s_hash" };
            const string userIdClaimType = "sub";

            var context = hca.HttpContext;
            var user = context?.User.Identity;
            if (user == null || !user.IsAuthenticated) return null;
            
            var userId = context.User.Claims.FirstOrDefault(a => a.Type == userIdClaimType)?.Value;
            var userInfo = new UserInfo
            {
                UserName = user.Name,
                UserId = userId,
                UserClaims = new Dictionary<string, List<string>>()
            };
            foreach (var distinctClaimType in context.User.Claims
                .Where(a => excluded.All(ex => ex != a.Type))
                .Select(a => a.Type)
                .Distinct())
            {
                userInfo.UserClaims[distinctClaimType] = context.User.Claims
                    .Where(a => a.Type == distinctClaimType)
                    .Select(c => c.Value)
                    .ToList();
            }                
            return userInfo;
        }
    }
}
