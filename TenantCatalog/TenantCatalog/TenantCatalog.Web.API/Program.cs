using CDMS.CP.Platform.Common.ApiStartup;
using Finbuckle.MultiTenant;
using System.Diagnostics.CodeAnalysis;
using TenantCatalog.Infrastructure;
using TenantCatalog.Infrastructure.Persistence;

namespace TenantCatalog.Web.API
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        private static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMultiTenant<CustomTenantInfo>()
    .WithConfigurationStore()
    .WithHeaderStrategy("x-tenant-id");

            builder
                .SetupConfiguration();

            builder.Setup()
            .SetupMediatR("TenantCatalog.Application");

            builder.Services.ConfigureServices();

            WebApplication app = builder
                .Build();

            app
                .Setup();
            app.
                UseMultiTenant();
            app.
                Run();
        }
    }
}
