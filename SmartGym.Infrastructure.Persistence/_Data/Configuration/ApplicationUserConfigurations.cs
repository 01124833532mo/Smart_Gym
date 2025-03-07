using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGym.Core.Domain._Identity;
using SmartGym.Infrastructure.Persistence.Abstractions.Consts;

namespace SmartGym.Infrastructure.Persistence._Data.Configuration
{
    internal class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.FirstName).HasMaxLength(100);
            builder.Property(x => x.LastName).HasMaxLength(100);
            builder.Property(x => x.Weight).HasColumnType("decimal(8,2)");

            builder.Property(user => user.Type)
                .HasConversion
                (
                (UStatus) => UStatus.ToString(),
                (UStatus) => (Types)Enum.Parse(typeof(Types), UStatus)
                );



            builder.HasData(new ApplicationUser
            {
                Id = DefaultUsers.AdminId,
                FirstName = "Cinemate System",
                LastName = "Admin",
                UserName = DefaultUsers.AdminEmail,
                BirthDay = new DateOnly(2000, 1, 1),
                NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
                Email = DefaultUsers.AdminEmail,
                NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
                SecurityStamp = DefaultUsers.AdminSecurityStamp,
                ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEAR2V+bcDJAlzUiuTRqKkLj/Uv4ibKCWikvvMF1g75/iOokLhV1l9SedoJOqspT0mA=="
            });
        }
    }
}
