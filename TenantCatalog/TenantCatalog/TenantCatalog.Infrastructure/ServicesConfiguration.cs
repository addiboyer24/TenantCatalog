using Azure.Messaging.ServiceBus;
using CDMS.CP.Platform.Common.Authentication;
using CDMS.CP.Platform.Common.Connectors.Interfaces;
using CDMS.CP.Platform.Common.Connectors.Standard;
using CDMS.CP.Platform.Common.EventGridHandler.Handlers;
using CDMS.CP.Platform.Common.FailureHandler.Handlers;
using CDMS.CP.Platform.Common.FailureHandler.Interfaces;
using CDMS.CP.Platform.Common.Helpers;
using CDMS.CP.Platform.Common.Helpers.Interfaces;
using CDMS.CP.Platform.Common.Logging.V2.Interfaces;
using CDMS.CP.Platform.Common.ServiceBusConnector.Client;
using CDMS.CP.Platform.Common.ServiceBusConnector.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenantCatalog.Domain.Constants;

namespace TenantCatalog.Infrastructure
{/// <summary>
 /// The services configuration class.
 /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServicesConfiguration
    {
        /// <summary>
        /// The configure services method.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthUserService, UserService>();

            IConfiguration configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services
                .ConfigureValidators()
                .ConfigureContexts()
                .ConfigureRepositories()
                .ConfigureAutoNumber()
                .ConfigureEventBus()
                .ConfigureDocumentDbConnector(configuration);

            Environment.SetEnvironmentVariable(AppTemplateConstants.AzureTenantIdKey, configuration.GetValue<string>(AppTemplateConstants.IdaTenantKey));

            services.ConfigureFeatureFlags();
            services.AddHttpContextAccessor();
        }

        private static IServiceCollection ConfigureContexts(this IServiceCollection services)
        {
            return services;
        }

        private static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            return services;
        }

        private static IServiceCollection ConfigureValidators(this IServiceCollection services)
        {
            return services;
        }

        private static IServiceCollection ConfigureAutoNumber(this IServiceCollection services)
        {
            services.AddSingleton(x =>
            {
                IConfiguration configHelper = x.GetRequiredService<IConfiguration>();
                return new
                EventGridHandler(configHelper.GetValue<string>("failureHandlerTopicEndpoint"), configHelper.GetValue<string>("failureHandlerTopicKey"));
            });

            services.AddSingleton<IFailureHandler, FailureEventGridHandler>();

            services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();

            services.AddSingleton<ITokenHelper, TokenHelper>(x =>
            {
                IConfiguration configHelper = x.GetRequiredService<IConfiguration>();
                string clientId = configHelper.GetValue<string>("ida:ClientId");
                string clientSecret = configHelper.GetValue<string>("ida:ClientSecret");
                string msiConnectionString = configHelper.GetValue<string>("ida:MsiConnectionString")?.ToString(CultureInfo.InvariantCulture);
                string aadInstance = configHelper.GetValue<string>("ida:AadInstance");
                string tenant = configHelper.GetValue<string>("ida:Tenant");
                string mode = configHelper.GetValue<string>("Mode");
                return new TokenHelper(clientId, clientSecret, msiConnectionString, aadInstance, tenant, mode);
            });
            return services;
        }

        private static IServiceCollection ConfigureEventBus(this IServiceCollection services)
        {
            services.AddScoped<IEventBusConnectorFactory, AzureServiceBusConnectorFactory>((s) =>
            {
                IConfiguration configHelper = s.GetRequiredService<IConfiguration>();
                ILogger<AzureServiceBusConnectorFactory> logger = s.GetRequiredService<ILogger<AzureServiceBusConnectorFactory>>();
                string connectionString = configHelper.GetValue<string>("ServiceBusConnectionString");
                ServiceBusClient client = new ServiceBusClient(connectionString);
                var serviceBusFactory = new AzureServiceBusConnectorFactory(logger, client);

                return serviceBusFactory;
            });

            return services;
        }
    }
}
