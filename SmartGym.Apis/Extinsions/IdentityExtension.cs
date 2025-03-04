using Microsoft.AspNetCore.Identity;
using SmartGym.Core.Domain._Identity;
using SmartGym.Infrastructure.Persistence._Data;

namespace SmartGym.Apis.Extinsions
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddIdentity<ApplicationUser, IdentityRole>((identityOptions) =>
            {
                //identityOptions.SignIn.RequireConfirmedPhoneNumber = true;
                identityOptions.Password.RequireDigit = true;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Lockout.AllowedForNewUsers = true;
                identityOptions.Lockout.MaxFailedAccessAttempts = 5;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(5);
                identityOptions.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();







            return services;
        }
    }
}
