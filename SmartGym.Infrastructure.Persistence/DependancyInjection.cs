using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartGym.Core.Domain.Contracts.Persestence.DbInitializers;
using SmartGym.Infrastructure.Persistence._Data;

namespace SmartGym.Infrastructure.Persistence
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>((options) =>
            {
                options.UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            });
            services.AddScoped(typeof(ISmartGymDbInitializer), typeof(SmartGymDbInitilizer));
            return services;
        }
    }
}
