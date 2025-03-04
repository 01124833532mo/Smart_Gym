using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartGym.Shared.Settings;


namespace SmartGym.Shared
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddSharedDependency(this IServiceCollection services, IConfiguration configuration)
        {


            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));


            return services;
        }
    }
}
