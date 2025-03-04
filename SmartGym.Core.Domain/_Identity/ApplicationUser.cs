using Microsoft.AspNetCore.Identity;

namespace SmartGym.Core.Domain._Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateOnly BirthDay { get; set; }
        public int Height { get; set; }
        public decimal Weight { get; set; }
        public bool IsDisabled { get; set; }
        public Types Type { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();


    }
}
