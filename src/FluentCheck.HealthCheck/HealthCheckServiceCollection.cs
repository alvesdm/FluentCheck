using Microsoft.Extensions.DependencyInjection;

namespace FluentCheck.HealthCheck
{
    public static class HealthCheckServiceCollection
    {
        public static IServiceCollection AddHealthCheck(this IServiceCollection services)
        {
            services.AddTransient<IHealthCheckConfiguration, HealthCheckConfiguration>();
            return services;
        }
    }
}