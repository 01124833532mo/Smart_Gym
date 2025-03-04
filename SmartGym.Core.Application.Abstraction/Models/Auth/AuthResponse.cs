using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGym.Core.Application.Abstraction.Models.Auth
{
	public record AuthResponse(
		 string Id,
		 string? Email,
		 string? FirstName,
		 string? LastName
	);
}
