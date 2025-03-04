using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartGym.Core.Application.Abstraction.Models.Auth;
using SmartGym.Core.Application.Abstraction.Services;
using SmartGym.Core.Domain._Identity;
using SmartGym.Shared.Errors.Models;
using SmartGym.Shared.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartGym.Core.Application.Services
{
    public class AuthService(IOptions<JwtSettings> jwtsettings, UserManager<ApplicationUser> _userManager) : IAuthService
    {
        private readonly JwtSettings _jwtsettings = jwtsettings.Value;

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

        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var userclaims = await _userManager.GetClaimsAsync(user);

            var userrolesclaims = new List<Claim>();

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                userrolesclaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            IEnumerable<Claim> claims;

            claims = new List<Claim>()
                {
                new Claim(ClaimTypes.PrimarySid,user.Id),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.NameIdentifier,user.FirstName!),
                new Claim("LastName",user.LastName!),
                new Claim("Height",user.Height.ToString()!),
                new Claim("Weight",user.Weight.ToString()!),
                new Claim("IsDisabled",user.IsDisabled.ToString()),

                new Claim("Type",user.Type.ToString()),
                }
           .Union(userclaims)
           .Union(userrolesclaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtsettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var tokenObj = new JwtSecurityToken(

                issuer: _jwtsettings.Issuer,
                audience: _jwtsettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtsettings.DurationInMinitutes),
                claims: claims,
                signingCredentials: signingCredentials
                );


            return new JwtSecurityTokenHandler().WriteToken(tokenObj);
        }
    }
}
