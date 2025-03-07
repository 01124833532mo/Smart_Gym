using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SmartGym.Infrastructure.Persistence.Abstractions.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGym.Infrastructure.Persistence._Data.Configuration
{
	public class UserRoleConfigration : IEntityTypeConfiguration<IdentityUserRole<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
		{
			builder.HasData(new IdentityUserRole<string>
			{
				UserId = DefaultUsers.AdminId,
				RoleId = DefaultRoles.AdminRoleId
			});
		}
	}
}
