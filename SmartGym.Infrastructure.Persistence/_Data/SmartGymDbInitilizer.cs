using Microsoft.EntityFrameworkCore;
using SmartGym.Core.Domain.Contracts.Persestence.DbInitializers;

namespace SmartGym.Infrastructure.Persistence._Data
{
    public class SmartGymDbInitilizer(ApplicationDbContext dbContext) : ISmartGymDbInitializer
    {
        public async Task InitializeAsync()
        {
            var pendingmigration = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingmigration.Any())
            {
                await dbContext.Database.MigrateAsync();
            }
        }

        public async Task SeedAsync()
        {


        }
    }
}
