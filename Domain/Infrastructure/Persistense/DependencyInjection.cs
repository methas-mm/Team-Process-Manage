using Application.Interfaces;
using Domain.Entities.ST;
using Domain.Entities.SU;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistense.Identity;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Persistense
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CleanDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("TMPC")), ServiceLifetime.Scoped);
            services.AddScoped<ICleanDbContext>(provider => provider.GetService<CleanDbContext>());
            services.AddIdentityCore<StUser>().AddEntityFrameworkStores<CleanDbContext>();
            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;
            });
            services.AddScoped<IIdentityService, IdentityService>();
            return services;
        }
    }
}
