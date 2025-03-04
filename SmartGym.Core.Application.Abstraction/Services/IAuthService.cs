using SmartGym.Core.Application.Abstraction.Models.Auth;
using SmartGym.Shared.Models.Auth;

namespace SmartGym.Core.Application.Abstraction.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);
        Task<AuthResponse> GetRefreshToken(RefreshDto refreshDto, CancellationToken cancellationToken = default);

        Task<bool> RevokeRefreshTokenAsync(RefreshDto refreshDto, CancellationToken cancellationToken = default);

    }
}
