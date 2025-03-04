using Microsoft.AspNetCore.Mvc;
using SmartGym.Apis.Controller.Controllers.Base;
using SmartGym.Core.Application.Abstraction.Models.Auth;
using SmartGym.Core.Application.Abstraction.Services;

namespace SmartGym.Apis.Controller.Controllers.Auth
{
	public class AccountController(IAuthService authService) : BaseApiController
	{
		[HttpPost("Register")]
		public async Task<ActionResult<AuthResponse>> Register(SignUpRequest request)
		{
			var result = await authService.SignUpAsync(request);
			return Ok(result);
		}
	}
}
