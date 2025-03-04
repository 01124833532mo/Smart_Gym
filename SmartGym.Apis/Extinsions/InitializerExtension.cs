﻿using SmartGym.Core.Domain.Contracts.Persestence.DbInitializers;

namespace SmartGym.Apis.Extinsions
{
    public static class InitializerExtension
    {
        public async static Task<WebApplication> InitializerEventManagmentContextAsync(this WebApplication app)
        {

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var EventManagmentContextIntializer = services.GetRequiredService<ISmartGymDbInitializer>();
            var LoggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await EventManagmentContextIntializer.InitializeAsync();
                await EventManagmentContextIntializer.SeedAsync();
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "an error has been occured during applaying migrations");
            }

            return app;
        }
    }
}
