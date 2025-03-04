using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartGym.Core.Application.Abstraction
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddCoreApplicationAbstractionDependencyInjection(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddFluentValidationConfig();
			return services;
		}
		private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
		{
			services
				.AddFluentValidationAutoValidation()
				.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			return services;
		}
	}
}
