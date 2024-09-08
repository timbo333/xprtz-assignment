using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Extensions.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all services in the infrastructure layer to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add the services to</param>
        /// <param name="config">The config section to use</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration config)
        {
            ArgumentNullException.ThrowIfNull(config);

            services.AddDbContext<ShowDbContext>(options => options.UseSqlServer(config.GetConnectionString("ShowDbContext") 
                ?? throw new InvalidOperationException("Connection string 'ShowDbContext' not found.")));

            var seedingOptions = config.GetSection(nameof(SeedingOptions)).Get<SeedingOptions>() 
                ?? throw new InvalidOperationException("Configuration for 'seedingOptions' not found.");

            services
                .AddScoped<IShowRepository, ShowRepository>()
                .AddHttpClient<ISeedingService, MazeTvSeedingService>(client =>
                {
                    client.BaseAddress = new Uri(seedingOptions.BaseUrl);
                })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new HttpClientHandler()
                    {
                        // Small performance gain when not using a proxy
                        UseProxy = false,
                        // Needed for using the baseurl in the httpClient instead of having the full url in the request send by the repository
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                    };
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy());

            return services;
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,retryAttempt)));
        }
    }
}
