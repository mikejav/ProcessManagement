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

namespace ProcessManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddDbContext<ProcessManagementDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("ProcessManagementContext")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAuthentication(SessionAuthenticationHandler.SchemeName)
                .AddScheme<SessionAuthenticationSchemeOptions, SessionAuthenticationHandler>(SessionAuthenticationHandler.SchemeName, null); // TODO:  slidingExpiration  in config

            services.AddSingleton<ISessionStore, InMemorySessionStore>(); // TODO: change to scoped after migrate to Redis

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = env.IsDevelopment()
                  ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
            });

            //services.AddControllers().AddNewtonsoftJson(x =>
            //    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            return services;
        }
    }
}
