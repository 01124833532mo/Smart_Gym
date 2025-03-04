using SmartGym.Core.Application.Abstraction.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGym.Core.Application.Abstraction.Services
{
	public interface IAuthService
	{
		Task<AuthResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);
	}
}
