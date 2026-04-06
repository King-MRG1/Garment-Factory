using Database.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Interfaces
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RefreshTokenStore CreateRefreshTokenStore(string userId)
        {
            var refreshTokenExpiryDays = int.TryParse(
                _configuration["JWT:RefreshTokenExpiryDays"], 
                out var days) ? days : 7;

            return new RefreshTokenStore
            {
                Token = GenerateRefreshToken(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false,
                ExpiryDate = DateTime.UtcNow.AddDays(refreshTokenExpiryDays)
            };
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];
            var key = _configuration["JWT:Key"];
            var expirationInMinutes = _configuration["JWT:ExpirationInMinutes"];

            if (string.IsNullOrWhiteSpace(issuer))
                throw new InvalidOperationException("JWT:Issuer is not configured");

            if (string.IsNullOrWhiteSpace(audience))
                throw new InvalidOperationException("JWT:Audience is not configured");

            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("JWT:Key is not configured");

            if (string.IsNullOrWhiteSpace(expirationInMinutes))
                throw new InvalidOperationException("JWT:ExpirationInMinutes is not configured");

            if (key.Length < 32)
                throw new InvalidOperationException("JWT:Key must be at least 32 characters long");

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim("FullName", user.Full_name ?? string.Empty),
                new Claim("PhoneNumber", user.PhoneNumber ?? string.Empty)
            };

            // ✅ JWT: Short-lived (60 minutes by default)
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(expirationInMinutes)),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}