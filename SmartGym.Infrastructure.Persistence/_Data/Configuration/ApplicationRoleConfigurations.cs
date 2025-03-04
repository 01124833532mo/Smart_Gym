using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SmartGym.Core.Domain._Identity;
using SmartGym.Infrastructure.Persistence.Abstractions.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGym.Infrastructure.Persistence._Data.Configuration
{
    class ApplicationRoleConfigurations : IEntityTypeConfiguration<ApplicationRole>
	{
		public void Configure(EntityTypeBuilder<ApplicationRole> builder)
		{
			//Default Data
			builder.HasData([
				new ApplicationRole
				{
					Id = DefaultRoles.AdminRoleId,
					Name = DefaultRoles.Admin,
					NormalizedName = DefaultRoles.Admin.ToUpper(),
					ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp
				},
				new ApplicationRole
				{
					Id = DefaultRoles.UserRoleId,
					Name = DefaultRoles.User,
					NormalizedName = DefaultRoles.User.ToUpper(),
					ConcurrencyStamp = DefaultRoles.UserRoleConcurrencyStamp,
					IsDefault = true
				},
				new ApplicationRole
				{
					Id = DefaultRoles.CoachRoleId,
					Name = DefaultRoles.Coach,
					NormalizedName = DefaultRoles.Coach.ToUpper(),
					ConcurrencyStamp = DefaultRoles.CoachRoleConcurrencyStamp,
					IsDefault = true
				}
			]);
		}
	}
}
