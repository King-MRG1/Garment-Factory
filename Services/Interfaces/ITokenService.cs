using Database.Models;

namespace Services.Interfaces
{
    public interface ITokenService
    {
        public string GenerateJwtToken(ApplicationUser user);
        public string GenerateRefreshToken();
        public RefreshTokenStore CreateRefreshTokenStore(string userId);
    }
}