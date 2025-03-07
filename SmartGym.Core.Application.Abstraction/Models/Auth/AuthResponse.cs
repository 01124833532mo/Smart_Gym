namespace SmartGym.Core.Application.Abstraction.Models.Auth
{
    public record AuthResponse(
         string Id,
         string? Email,
         string? FirstName,
         string? LastName,
         string? RefreshToken,
         DateTime? RefreshTokenExpirationDate
    );
}
