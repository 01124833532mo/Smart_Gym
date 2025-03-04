using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGym.Core.Application.Abstraction.Models.Auth
{
	public record SignUpRequest
	(
		 string FirstName,
		 string LastName,
		 string Email,
		 string Password,
		 string Gender,
		 int Height,
		 decimal Weight,
		 DateOnly BirthDay
	);
}
