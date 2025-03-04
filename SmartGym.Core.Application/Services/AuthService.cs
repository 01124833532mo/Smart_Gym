using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartGym.Core.Application.Abstraction.Bases;
using SmartGym.Core.Application.Abstraction.Models.Auth;
using SmartGym.Core.Application.Abstraction.Services;
using SmartGym.Core.Domain._Identity;
using SmartGym.Shared.Errors.Models;

namespace SmartGym.Core.Application.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		public AuthService(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}
		public async Task<AuthResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default)
		{
			var emailIsExist = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

			if (emailIsExist)
				throw new NotFoundExeption("User", request.Email);

			var user = new ApplicationUser
			{
				UserName = request.Email,
				Email = request.Email,
				FirstName = request.FirstName,
				LastName = request.LastName,
				Gender = request.Gender,
				BirthDay = request.BirthDay,
				Height = request.Height,
				Weight = request.Weight,
				IsDisabled = false
			};
			var result = await _userManager.CreateAsync(user, request.Password);
			if (!result.Succeeded)
				throw new ValidationExeption() { Errors = result.Errors.Select(p => p.Description) };

			var response = new AuthResponse(
					Id: user.Id,
					Email: user.Email,
					FirstName: user.FirstName,
					LastName: user.LastName
			);
			return response;
		}
	}
}
