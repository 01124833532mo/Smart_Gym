namespace SmartGym.Core.Application.Abstraction.Models.Auth
{
    public record AuthResponse(
         string Id,
         string? Email,
         string? FirstName,
         string? LastName,
         string? Token,
         string? RefreshToken,
         DateTime? RefreshTokenExpirationDate
    );
}
