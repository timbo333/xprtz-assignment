using Domain.Services;
using Microsoft.Extensions.Configuration;

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
        public static IServiceCollection AddDomainLayer(this IServiceCollection services, IConfiguration config)
        {
            if (config is null)
                throw new ArgumentNullException(nameof(config));

            services
                .AddMemoryCache()
                .AddSingleton<ICachingService, CachingService>()
                .AddScoped<IShowService, ShowService>()
                .Decorate<IShowService, CachedShowService>();

            return services;
        }
    }
}
