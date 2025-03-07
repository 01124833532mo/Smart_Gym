namespace SmartGym.Core.Domain.Contracts.Persestence.DbInitializers
{
    public interface ISmartGymDbInitializer
    {
        Task InitializeAsync();
        Task SeedAsync();
    }
}
