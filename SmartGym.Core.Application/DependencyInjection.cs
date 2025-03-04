using Microsoft.Extensions.DependencyInjection;
using SmartGym.Core.Application.Abstraction.Services;
using SmartGym.Core.Application.Mapping;
using SmartGym.Core.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGym.Core.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddCoreApplicationDependencyInjection(this IServiceCollection services)
		{
			services.AddScoped<IAuthService, AuthService>();



			services.AddAutoMapper(mapper => mapper.AddProfile(new MappingProfile()));
			services.AddAutoMapper(typeof(MappingProfile));
			return services;
		}
	}
}
