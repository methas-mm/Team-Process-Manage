using Application.Common.Constants;
using Application.Interfaces;
using Application.Interfaces.Email;
using Infrastructure.Services;
using Infrastructure.Services.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IExcelDocumentService, ExcelDocumentService>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddScoped<IEmailSetting,EmailSetting>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddTransient<IContentService, ContentService>();
            services.AddTransient((provider) =>
            {
                return new Func<ContentType, IContentService>(
                    (type) => new ContentService(provider.GetService<ICleanDbContext>(), type)
                );
            });
            var reportUrl = configuration.GetValue<string>("ReportUrl");
            services.AddHttpClient<IReportService, ReportService>(x =>
            {
                x.BaseAddress = new Uri(reportUrl);
            });
            return services;
        }
    }
}
