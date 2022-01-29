using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProcessManagement.Infrastructure.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ProcessManagement.Infrastructure.Auth.AuthenticationHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using ProcessManagement.Core;
using ProcessManagement.Infrastructure.Services;
using ProcessManagement.Core.Services;
using StackExchange.Redis;

namespace ProcessManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddDbContext<ProcessManagementDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("ProcessManagementContext")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddAuthentication(SessionAuthenticationHandler.SchemeName)
                .AddScheme<SessionAuthenticationSchemeOptions, SessionAuthenticationHandler>(SessionAuthenticationHandler.SchemeName, null);

            services.AddScoped<ISessionStore, RedisSessionStore>();
            services.AddHttpContextAccessor();
            services.AddStackExchangeRedis();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = env.IsDevelopment()
                  ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
            });

            return services;
        }

        public static IServiceCollection AddStackExchangeRedis(this IServiceCollection services)
        {
            var conn = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = conn.GetDatabase();
            services.AddSingleton(db);

            return services;
        }
    }
}
