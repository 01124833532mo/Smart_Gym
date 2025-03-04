using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGym.Infrastructure.Persistence.Abstractions.Consts
{
	public static class DefaultRoles
	{
		public const string Admin = nameof(Admin);
		public const string AdminRoleId = "92b75286-d8f8-4061-9995-e6e23ccdee94";
		public const string AdminRoleConcurrencyStamp = "f51e5a91-bced-49c2-8b86-c2e170c0846c";

		public const string User = nameof(User);
		public const string UserRoleId = "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4";
		public const string UserRoleConcurrencyStamp = "5ee6bc12-5cb0-4304-91e7-6a00744e042a";

		public const string Coach = nameof(Coach);
		public const string CoachRoleId = "0574c1f8-3801-4810-8622-bcf466bc4df4";
		public const string CoachRoleConcurrencyStamp = "08867cec-b9f8-44f4-b85c-425a1061ff09";
	}
}
