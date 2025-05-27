using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using TenantCatalog.Events;
using CDMS.CP.Platform.Common.FunctionStartup;
using System.Diagnostics.CodeAnalysis;
using TenantCatalog.Infrastructure;

[assembly: FunctionsStartup(typeof(Startup))]

namespace TenantCatalog.Events
{
    /// <summary>
    /// The startup class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// The configure method.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder
                .Setup()
                .SetupMediatR("TenantCatalog.Application")
                .Services.ConfigureServices();
        }

        /// <summary>
        /// The configure app configuration.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.SetupConfiguration();
        }
    }
}