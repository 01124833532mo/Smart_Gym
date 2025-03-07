using Microsoft.AspNetCore.Mvc;
using SmartGym.Apis.Controller.Controllers.Base;
using SmartGym.Core.Application.Abstraction.Models.Auth;
using SmartGym.Core.Application.Abstraction.Services;
using SmartGym.Shared.Models.Auth;

namespace SmartGym.Apis.Controller.Controllers.Auth
{
    public class AccountController(IAuthService authService) : BaseApiController
    {
        [HttpPost("Register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] SignUpRequest request)
        {
            var result = await authService.SignUpAsync(request);
            return Ok(result);
        }
        [HttpPost("Get-Refresh-Token")]

        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshDto model)
        {
            var result = await authService.GetRefreshToken(model);
            return Ok(result);
        }

        [HttpPost("Revoke-Refresh-Token")]
        public async Task<ActionResult> RevokeRefreshToken([FromBody] RefreshDto model)
        {
            var result = await authService.RevokeRefreshTokenAsync(model);
            return result is false ? BadRequest("Operation Not Successed") : Ok(result);

        }

    }
}
