using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartGym.Core.Application.Abstraction.Models.Auth;
using SmartGym.Core.Application.Abstraction.Services;
using SmartGym.Core.Domain._Identity;
using SmartGym.Shared.Errors.Models;
using SmartGym.Shared.Models.Auth;
using SmartGym.Shared.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SmartGym.Core.Application.Services
{
    public class AuthService(IOptions<JwtSettings> jwtsettings, UserManager<ApplicationUser> _userManager,
        ILogger<AuthService> logger,
        SignInManager<ApplicationUser> signInManager) : IAuthService
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

            //var roleResult = await _userManager.AddToRoleAsync(user, Types.User.ToString());
            //if (!roleResult.Succeeded)
            //    throw new ValidationExeption() { Errors = roleResult.Errors.Select(E => E.Description) };



            var response = new AuthResponse(
                    Id: user.Id,
                    Email: user.Email,
                    FirstName: user.FirstName,
                    Token: await GenerateTokenAsync(user),
                    LastName: user.LastName,
                    RefreshTokenExpirationDate: DateTime.Now,  // set on this real refresh token ya ziad 
                    RefreshToken: ""
            );
            return response;
        }

        public async Task<AuthResponse> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                logger.LogWarning("Invalid Login Attempt For Email {Email}", loginDto.Email);
                throw new UnAuthorizedExeption("Invalid Login");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                logger.LogWarning("Email is Locked Out {Email}", loginDto.Email);
                throw new UnAuthorizedExeption("Email is Locked Out");

            }

            if (!result.Succeeded)
            {
                logger.LogWarning("Invalid Login Attempt For Email {Email}", loginDto.Email);
                throw new UnAuthorizedExeption("Invalid Login");

            }

            // Initialize the response with default values
            var response = new AuthResponse(
                Id: user.Id,
                Email: user.Email,
                FirstName: user.FirstName,
                Token: await GenerateTokenAsync(user),
                LastName: user.LastName,
                RefreshToken: null, // Default value
                RefreshTokenExpirationDate: null // Default value
            );

            response = await CheckRefreshToken(_userManager, user, response);

            logger.LogInformation("User with Email {Email} logged in", loginDto.Email);

            return response;
        }

        private async Task<AuthResponse> CheckRefreshToken(UserManager<ApplicationUser> _userManager, ApplicationUser? user, AuthResponse response)
        {
            if (user.RefreshTokens.Any(t => t.IsActice))
            {
                var activeToken = user.RefreshTokens.FirstOrDefault(x => x.IsActice);
                response = response with
                {
                    RefreshToken = activeToken!.Token,
                    RefreshTokenExpirationDate = activeToken.ExpireOn
                };
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                response = response with
                {
                    RefreshToken = refreshToken.Token,
                    RefreshTokenExpirationDate = refreshToken.ExpireOn
                };

                user.RefreshTokens.Add(new RefreshToken()
                {
                    Token = refreshToken.Token,
                    ExpireOn = refreshToken.ExpireOn,

                });

                await _userManager.UpdateAsync(user);
            }

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
        private string? ValidateToken(string token)
        {
            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtsettings.Key));

            var tokenhandler = new JwtSecurityTokenHandler();

            try
            {
                tokenhandler.ValidateToken(token, new TokenValidationParameters()
                {
                    IssuerSigningKey = authkey,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero,
                }, out SecurityToken securityToken);

                var securitytokenobj = (JwtSecurityToken)securityToken;

                return securitytokenobj.Claims.First(x => x.Type == ClaimTypes.PrimarySid).Value;
            }
            catch (Exception)
            {

                return null;
            }
        }
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            var genrator = new RNGCryptoServiceProvider();

            genrator.GetBytes(randomNumber);

            return new RefreshToken()
            {
                Token = Convert.ToBase64String(randomNumber),
                CreatedOn = DateTime.UtcNow,
                ExpireOn = DateTime.UtcNow.AddDays(_jwtsettings.JWTRefreshTokenExpire)


            };


        }

        public async Task<AuthResponse> GetRefreshToken(RefreshDto refreshDto, CancellationToken cancellationToken = default)
        {
            var userId = ValidateToken(refreshDto.Token);

            if (userId is null) throw new NotFoundExeption("User id Not Found", nameof(userId));

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) throw new NotFoundExeption("User Do Not Exists", nameof(user.Id));

            var UserRefreshToken = user!.RefreshTokens.SingleOrDefault(x => x.Token == refreshDto.RefreshToken && x.IsActice);

            if (UserRefreshToken is null) throw new NotFoundExeption("Invalid Token", nameof(userId));

            UserRefreshToken.RevokedOn = DateTime.UtcNow;

            var newtoken = await GenerateTokenAsync(user);

            var newrefreshtoken = GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken()
            {
                Token = newrefreshtoken.Token,
                ExpireOn = newrefreshtoken.ExpireOn
            });

            await _userManager.UpdateAsync(user);

            var response = new AuthResponse(
                              Id: user.Id,
                              Email: user.Email,
                              FirstName: user.FirstName,
                              LastName: user.LastName,
                              Token: newtoken,
                              RefreshToken: newrefreshtoken.Token,
                              RefreshTokenExpirationDate: newrefreshtoken.ExpireOn

                      );
            return response;
        }

        public async Task<bool> RevokeRefreshTokenAsync(RefreshDto refreshDto, CancellationToken cancellationToken = default)
        {
            var userId = ValidateToken(refreshDto.Token);

            if (userId is null) return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return false;

            var UserRefreshToken = user!.RefreshTokens.SingleOrDefault(x => x.Token == refreshDto.RefreshToken && x.IsActice);

            if (UserRefreshToken is null) return false;

            UserRefreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
            return true;
        }


    }
}
